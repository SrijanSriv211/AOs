partial class EntryPoint
{
    public Parser parser;
    private Features features;

    private readonly Obsidian AOs;
    private readonly string[] args;
    private readonly Action<Obsidian, Parser, List<(string, string[])>> run_method;
    private readonly SystemUtils sys_utils = new();

    public EntryPoint(string[] args, Action<Obsidian, Parser, List<(string, string[])>> run_method)
    {
        this.args = args;
        this.run_method = run_method;
        this.AOs = new Obsidian();

        CreateRootPackages();
        LoadFeatures();
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
}
