partial class EntryPoint
{
    public static void CrashreportLog(string content)
    {
        string current_time = $"[{DateTime.Now:h:mm:ss tt} {DateTime.Now:dddd, dd MMMM yyyy}]";
        string boot_log_path = Path.Combine(Obsidian.root_dir, "Files.x72\\root\\log\\Crashreport.log");

        FileIO.FileSystem.Write(boot_log_path, $"{current_time}\t{content} \n");
    }

    private static void StartupLog(string content)
    {
        string current_time = $"[{DateTime.Now:h:mm:ss tt} {DateTime.Now:dddd, dd MMMM yyyy}]";
        string boot_log_path = Path.Combine(Obsidian.root_dir, "Files.x72\\root\\log\\BOOT.log");

        FileIO.FileSystem.Write(boot_log_path, $"{current_time}\t{content} \n");
    }
}
