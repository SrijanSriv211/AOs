partial class EntryPoint
{
    private void Startup()
    {
        if (this.args.Length == 0)
        {
            this.AOs.ClearConsole();

            string[] filenames = LoadStartlist();
            List<string[]> all_startup_apps_content = ReadAllStartupApps(filenames);

            foreach (string[] contents in all_startup_apps_content)
                Execute(contents);
            
            Console.WriteLine();
            Execute();
        }

        else if (this.args.First().EndsWith(".aos"))
        {
        }
    }

    private void Execute()
    {
        while (true)
            this.run_method(this.AOs, AOs.TakeInput());
    }

    private void Execute(string[] inputs)
    {
        foreach (string input in inputs)
            this.run_method(this.AOs, AOs.TakeInput(input));
    }

    private string[] LoadStartlist()
    {
        string startlist_path = Path.Combine(Obsidian.root_dir, "Files.x72\\etc\\Startup\\.startlist");
        return FileIO.FileSystem.ReadAllLines(startlist_path);
    }

    private List<string[]> ReadAllStartupApps(string[] filenames)
    {
        List<string[]> StartupAppsContent = new();

        foreach (string filename in filenames)
        {
            if (filename == ".")
                break;

            else if (filename.EndsWith(".aos"))
            {
                string startup_file_data = Path.Combine(Obsidian.root_dir, "Files.x72\\etc\\Startup\\", filename);
                StartupAppsContent.Add(FileIO.FileSystem.ReadAllLines(startup_file_data));
            }
        }

        return StartupAppsContent;
    }
}
