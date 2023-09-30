partial class EntryPoint
{
    private void CheckRootPackages()
    {
        string[] DirectoryList = LoadRootPackagesData().Item1;
        string[] FileList = LoadRootPackagesData().Item2;

        foreach (string path in DirectoryList)
        {
            string full_path = Path.Combine(Obsidian.root_dir, path);

            LogRootPackagesHealth(full_path, false);
            FileIO.FolderSystem.Create(full_path);
        }

        foreach (string path in FileList)
        {
            string full_path = Path.Combine(Obsidian.root_dir, path);

            LogRootPackagesHealth(full_path, true);
            FileIO.FileSystem.Create(full_path);
        }
    }

    private (string[], string[]) LoadRootPackagesData()
    {
        string[] DirectoryList = new string[]
        {
            "Files.x72\\etc\\PowerToys",
            "Files.x72\\etc\\tmp",
            "Files.x72\\etc\\Startup"
        };

        string[] FileList = new string[]
        {
            "Files.x72\\etc\\Startup\\.startlist",
            "Files.x72\\root\\.history",
            "Files.x72\\root\\log\\BOOT.log",
            "Files.x72\\root\\log\\Crashreport.log"
        };

        return (DirectoryList, FileList);
    }

    private void LogRootPackagesHealth(string full_path, bool is_file)
    {
        StartupLog($"Checking {full_path}");

        if ((is_file && File.Exists(full_path)) || (!is_file && Directory.Exists(full_path)))
        {
            StartupLog($"Found {full_path}");
            return;
        }

        StartupLog($"Not found {full_path}");
        StartupLog($"Creating {full_path}");
    }
}
