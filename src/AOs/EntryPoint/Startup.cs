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
                new TerminalColor($"> {contents.Key}", ConsoleColor.DarkGray);
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
