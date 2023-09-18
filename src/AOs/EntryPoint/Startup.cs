partial class EntryPoint
{
    private void Startup()
    {
        if (this.args.Length == 0)
        {
            this.AOs.ClearConsole();

            string[] filenames = LoadStartlist();
            Dictionary<string, string[]> all_startup_apps_content = ReadAllStartupApps(filenames);

            foreach (var contents in all_startup_apps_content)
            {
                new TerminalColor($"> ", ConsoleColor.DarkGray, false);
                new TerminalColor($"{contents.Key}", ConsoleColor.White);
                Execute(contents.Value);
            }

            Execute();
        }

        else if (this.args.First().ToLower().EndsWith(".aos"))
        {
            string filename = this.args.First();

            if (!File.Exists(filename))
            {
                new Error($"Can't open file '{filename}': No such file or directory");
                Environment.Exit(1);
            }

            foreach (string line in FileIO.FileSystem.ReadAllLines(filename))
            {
                List<string[]> ListOfToks = new Lexer(line).Tokens;

                foreach (string[] Toks in ListOfToks)
                {
                    for (int k = 0; k < Toks.Length; k++)
                    {
                        if (Toks[k].StartsWith("$"))
                        {
                            Toks[k] = int.TryParse(Toks[k].Substring(1), out int arg_index) && arg_index < this.args.Length ? this.args[arg_index] : "";

                            if (Toks[k].Contains(" "))
                                Toks[k] = $"\"{Toks[k]}\"";
                        }
                    }

                    Execute(string.Join("", Toks));
                }
            }
        }

        else
        {
            var parser = new Argparse("AOs", "A Developer Command-line Tool Built for Developers by a Developer.", Error.UnrecognizedArgs);
            parser.Add(new string[] {"-h", "--help"}, "Display all supported arguments", is_flag: true);
            parser.Add(new string[] {"-a", "--admin"}, "Run as administrator", is_flag: true);
            parser.Add(new string[] {"-c", "--cmd"}, "Program passed in as string");
            var parsed_args = parser.Parse(args);

            foreach (var arg in parsed_args)
            {
                if (Argparse.IsAskingForHelp(arg.Names))
                    parser.PrintHelp();

                else if (arg.Names.Contains("-a"))
                {
                    SystemUtils sys_utils = new();

                    string AOsBinaryFilepath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                    sys_utils.StartApp(AOsBinaryFilepath, is_admin: true);
                    Environment.Exit(0);
                }

                else if (arg.Names.Contains("-c"))
                {
                    if (arg.Value == null || Utils.String.IsEmpty(arg.Value))
                    {
                        new Error($"Argument expected for the {string.Join(", ", arg.Names)} option.");
                        Environment.Exit(1);
                    }

                    Execute(arg.Value);
                }

                else
                {
                    new Error($"{arg.Names.First()}: File format not recognized. File must have '.aos' extension");
                    Environment.Exit(1);
                }
            }
        }
    }

    private void Execute()
    {
        while (true)
            this.run_method(this.AOs, AOs.TakeInput());
    }

    private void Execute(string input)
    {
        if (!Utils.String.IsEmpty(input))
            this.run_method(this.AOs, AOs.TakeInput(input));
    }

    private void Execute(string[] inputs)
    {
        foreach (string input in inputs)
        {
            if (!Utils.String.IsEmpty(input))
                this.run_method(this.AOs, AOs.TakeInput(input));
        }
    }

    private string[] LoadStartlist()
    {
        string startlist_path = Path.Combine(Obsidian.root_dir, "Files.x72\\etc\\Startup\\.startlist");
        return FileIO.FileSystem.ReadAllLines(startlist_path);
    }

    private Dictionary<string, string[]> ReadAllStartupApps(string[] filenames)
    {
        Dictionary<string, string[]> StartupAppsContent = new();

        foreach (string filename in filenames)
        {
            if (filename == ".")
                break;

            else if (filename.EndsWith(".aos"))
            {
                string startup_file_data = Path.Combine(Obsidian.root_dir, "Files.x72\\etc\\Startup\\", filename);
                StartupAppsContent.Add(filename, FileIO.FileSystem.ReadAllLines(startup_file_data));
            }
        }

        return StartupAppsContent;
    }
}
