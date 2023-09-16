class EntryPoint
{
    private readonly Obsidian AOs;
    private readonly string[] args;
    private readonly Action<Obsidian, List<(string, string[])>> run_method;

    public EntryPoint(string[] args, Action<Obsidian, List<(string, string[])>> run_method)
    {
        this.args = args;
        this.run_method = run_method;
        this.AOs = new Obsidian();

        Startup();
    }

    private void Run(List<(string cmd, string[] args)> input)
    {
        try
        {
            run_method(AOs, input);
        }

        catch (Exception e)
        {
            new Error(e.Message);
        }
    }

    private void Startup()
    {
        string[] argv = Utils.Array.Filter(args);

        var parser = new Argparse("AOs", "A Command-line utility for improved efficiency and productivity.");
        parser.Add(new string[] {"-h", "--help"}, "Display all supported arguments", is_flag: true);
        parser.Add(new string[] {"-a", "--admin"}, "Run as administrator", is_flag: true);
        parser.Add(new string[] {"-c", "--cmd"}, "Program passed in as string");

        var parsed_args = parser.Parse(argv);

        if (parsed_args.Count() == 0 && argv.Length == 0)
        {
            string startlist_path = Path.Combine(Obsidian.root_dir, "Files.x72\\root\\StartUp\\.startlist");
            bool isEmpty = Utils.String.IsEmpty(FileIO.FileSystem.ReadAllText(startlist_path));

            if (File.Exists(startlist_path) && !isEmpty)
            {
                AOs.Entrypoint(false);
                foreach (string appname in File.ReadLines(startlist_path))
                {
                    // break if "." is in place of appname
                    // all apps after the dot will be marked as disabled.
                    if (appname == ".")
                        break;

                    else if (appname.EndsWith(".aos"))
                    {
                        foreach (string current_line in FileIO.FileSystem.ReadAllLines( Path.Combine(Path.GetDirectoryName(startlist_path), appname) ))
                            Run(AOs.TakeInput(current_line));
                    }
                }
            }

            else
            {
                AOs.Entrypoint();
                while (true)
                    Run(AOs.TakeInput());
            }
        }

        else
        {
            for (int i = 0; i < parsed_args.ToArray().Length; i++)
            {
                var arg = parsed_args[i];

                if (Argparse.IsAskingForHelp(arg.Names))
                    parser.PrintHelp();

                else if (arg.Names.Contains("-a"))
                {
                    SystemUtils sys_utils = new();

                    string AOsBinaryFilepath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                    sys_utils.StartApp(AOsBinaryFilepath, is_admin: true);
                }

                else if (arg.Names.Contains("-c"))
                {
                    if (arg.Value == null || Utils.String.IsEmpty(arg.Value))
                        return;

                    AOs.Entrypoint(false);
                    Run(AOs.TakeInput(arg.Value));
                }

                else if (!arg.Names.First().EndsWith(".aos") && i == 0)
                {
                    new Error($"{arg.Names.First()}: File format not recognized. File must have '.aos' extension");
                    return;
                }

                else if (!File.Exists(arg.Names.First()) && i == 0)
                {
                    new Error($"{arg.Names.First()}: No such file.");
                    return;
                }

                else if (arg.Names.First().EndsWith(".aos") && i == 0)
                {
                    AOs.Entrypoint(false);
                    List<string> program_args = new();
                    for (int j = 0; j < parsed_args.ToArray().Length; j++)
                    {
                        if (parsed_args[j].KnownType == "Unknown")
                            program_args.Add(parsed_args[j].Names.First());
                    }

                    foreach (string current_line in FileIO.FileSystem.ReadAllLines(arg.Names.First()))
                    {
                        List<string[]> ListOfToks = new Lexer(current_line).Tokens;
                        foreach (string[] Toks in ListOfToks)
                        {
                            for (int k = 0; k < Toks.Length; k++)
                            {
                                if (Toks[k].StartsWith("$"))
                                {
                                    if (int.TryParse(Toks[k].Substring(1), out int arg_index) && arg_index < this.args.Length)
                                        Toks[k] = this.args[arg_index];
                                }
                            }

                            Run(AOs.TakeInput(string.Join("", Toks)));
                        }
                    }

                    return;
                }
            }
        }
    }

    public static void RootPackages()
    {
        string[] DirectoryList = new string[]
        {
            "Files.x72\\etc\\PowerToys",
            "Files.x72\\etc\\tmp",
            "Files.x72\\etc\\StartUp"
        };

        string[] FileList = new string[]
        {
            "Files.x72\\etc\\StartUp\\.startlist",
            "Files.x72\\root\\.history",
            "Files.x72\\root\\log\\BOOT.log",
            "Files.x72\\root\\log\\Crashreport.log"
        };

        foreach (string path in DirectoryList)
            FileIO.FolderSystem.Create(Path.Combine(Obsidian.root_dir, path));

        foreach (string path in FileList)
            FileIO.FileSystem.Create(Path.Combine(Obsidian.root_dir, path));
    }

    public static void AskPass()
    {
        string Path = $"{Obsidian.root_dir}\\Files.x72\\root\\User.set";
        if (!File.Exists(Path))
            return;

        while (true)
        {
            Console.Write("Enter password: ");
            string Password = Console.ReadLine();
            if (Password != FileIO.FileSystem.ReadAllText(Path))
                new Error("Incorrect password.");

            else
                break;
        }
    }
}
