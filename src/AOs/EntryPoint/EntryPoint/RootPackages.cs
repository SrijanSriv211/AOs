partial class EntryPoint
{
    private static void CheckRootPackages()
    {
        string[] DirectoryList = [
            "Files.x72\\etc\\PowerToys",
            "Files.x72\\etc\\Startup",
            "Files.x72\\root\\log"
        ];

        string[] FileList = [
            "Files.x72\\root\\history.json",
            "Files.x72\\root\\settings.json",
            "Files.x72\\root\\log\\Crashreport.log"
        ];

        // If the any of the directories or files doesn't exist then create it.
        // They are necessary because they will be used to store data such as,
        // command history, startup apps, boot log, crashreport, extentions, temp files, etc.
        foreach (string path in DirectoryList)
            FileIO.FolderSystem.Create(Path.Combine(Obsidian.root_dir, path));

        foreach (string path in FileList)
            FileIO.FileSystem.Create(Path.Combine(Obsidian.root_dir, path));

        // If 'settings.json' file doesn't exist then throw error.
        // This file will contain all the command and it's properties
        // that will be used by AOs to load the features.
        if (!File.Exists(Path.Combine(Obsidian.root_dir, "Files.x72\\root\\settings.json")))
        {
            _ = new Error("settings.json: File not found", "boot error");
            Environment.Exit(1);
        }

        // Else get all the commands from the 'settings.json' file.
        LoadSettings();
    }
}
