partial class EntryPoint
{
    private readonly Obsidian AOs;
    private readonly string[] args;
    private readonly Action<Obsidian, List<(string, string[])>> run_method;

    public EntryPoint(string[] args, Action<Obsidian, List<(string, string[])>> run_method)
    {
        this.args = args;
        this.run_method = run_method;
        this.AOs = new Obsidian();

        CreateRootPackages();
        AskPassword();
        Startup();
    }

    public static void CreateRootPackages()
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

    public static void AskPassword()
    {
        string Path = $"{Obsidian.root_dir}\\Files.x72\\root\\User.set";
        if (!File.Exists(Path))
            return;

        Console.Write("Enter password: ");
        string Password = Console.ReadLine();
        if (Password != FileIO.FileSystem.ReadAllText(Path))
            new Error("Incorrect password.");
    }
}
