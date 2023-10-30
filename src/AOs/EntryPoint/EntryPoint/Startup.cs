partial class EntryPoint
{
    private void Startup()
    {
        this.features.ChangeTitle(new string[]{"AOs"});
        if (this.args.Length == 0)
            RunAOs();

        else if (this.args.First().ToLower().EndsWith(".aos"))
            RunAOsFile();

        else
            RunAOsCLA();
    }

    private void RunAOs()
    {
        this.AOs.ClearConsole();

        string[] filenames = LoadStartlist();
        Dictionary<string, string[]> all_startup_apps_content = ReadAllStartupApps(filenames);

        foreach (var contents in all_startup_apps_content)
        {
            TerminalColor.Print($"> ", ConsoleColor.DarkGray, false);
            TerminalColor.Print($"{contents.Key}", ConsoleColor.White);
            Execute(contents.Value);
        }

        Execute();
    }

    private void RunAOsFile()
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

    // CLA -> Command Line Arguments
    private void RunAOsCLA()
    {
        var parser = new Argparse("AOs", Obsidian.about_AOs, Error.UnrecognizedArgs);
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
                string AOsBinaryFilepath = Obsidian.AOs_binary_path;
                this.sys_utils.StartApp(AOsBinaryFilepath, is_admin: true);
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
