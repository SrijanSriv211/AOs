partial class EntryPoint
{
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
