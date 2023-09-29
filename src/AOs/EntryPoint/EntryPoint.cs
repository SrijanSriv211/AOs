partial class EntryPoint
{
    public Parser parser;
    private Features features;

    private readonly Obsidian AOs;
    private readonly string[] args;
    private readonly Action<Obsidian, Parser, List<(string, string[])>> run_method;
    private readonly SystemUtils sys_utils = Obsidian.sys_utils;

    public EntryPoint(string[] args, Action<Obsidian, Parser, List<(string, string[])>> run_method)
    {
        this.args = args;
        this.run_method = run_method;
        this.AOs = new Obsidian();

        CreateRootPackages();
        LoadFeatures();
        Startup();
    }

    public static void CrashreportLogging(string content)
    {
        string current_time = $"[{DateTime.Now:h:mm:ss tt} {DateTime.Now:dddd, dd MMMM yyyy}]";
        string boot_log_path = Path.Combine(Obsidian.root_dir, "Files.x72\\root\\log\\Crashreport.log");

        FileIO.FileSystem.Write(boot_log_path, $"{current_time}\t{content} \n");
    }

    private static void PreStartupLogging(string content)
    {
        string current_time = $"[{DateTime.Now:h:mm:ss tt} {DateTime.Now:dddd, dd MMMM yyyy}]";
        string boot_log_path = Path.Combine(Obsidian.root_dir, "Files.x72\\root\\log\\BOOT.log");

        FileIO.FileSystem.Write(boot_log_path, $"{current_time}\t{content} \n");
    }

    private static void CreateRootPackages()
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

        static void LogRootPackages(string full_path, bool is_file)
        {
            PreStartupLogging($"Checking {full_path}");

            if ((is_file && File.Exists(full_path)) || (!is_file && Directory.Exists(full_path)))
            {
                PreStartupLogging($"Found {full_path}");
                return;
            }

            CrashreportLogging($"Not found {full_path}");
            PreStartupLogging($"Not found {full_path}");
            PreStartupLogging($"Creating {full_path}");
        }

        foreach (string path in DirectoryList)
        {
            string full_path = Path.Combine(Obsidian.root_dir, path);

            LogRootPackages(full_path, false);
            FileIO.FolderSystem.Create(full_path);
        }

        foreach (string path in FileList)
        {
            string full_path = Path.Combine(Obsidian.root_dir, path);

            LogRootPackages(full_path, true);
            FileIO.FileSystem.Create(full_path);
        }
    }
}
