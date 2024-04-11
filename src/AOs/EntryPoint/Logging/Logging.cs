partial class EntryPoint
{
    // Log every error into 'Crashreport.log' file.
    // This function is really helpful because it saves all the errors and crash bugs into a text file,
    // which can later be used to fix several problems in AOs.
    public static void CrashreportLog(string content)
    {
        string current_time = $"[{DateTime.Now:h:mm:ss tt} {DateTime.Now:dddd, dd MMMM yyyy}]";
        string crashreport_path = Path.Combine(Obsidian.root_dir, "Files.x72\\root\\log\\Crashreport.log");

        Terminal.Print($"An error occured, for more information see the crash report at '{crashreport_path}'", ConsoleColor.Red);
        FileIO.FileSystem.Write(crashreport_path, $"{current_time}\t{content} \n");
    }
}
