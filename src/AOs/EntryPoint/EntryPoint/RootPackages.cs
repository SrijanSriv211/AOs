partial class EntryPoint
{
    private static void CheckRootPackages()
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

        foreach (string path in DirectoryList)
            FileIO.FolderSystem.Create(Path.Combine(Obsidian.root_dir, path));

        foreach (string path in FileList)
            FileIO.FileSystem.Create(Path.Combine(Obsidian.root_dir, path));
    }
}
