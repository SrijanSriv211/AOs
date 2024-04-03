partial class EntryPoint
{
    // Read all the files that are meant to run at startup.
    private static Dictionary<string, string> ReadAllStartupApps()
    {
        // Dictionary<string, string>:
        // string -> filename, string -> absolute path of the file
        Dictionary<string, string> StartupAppsContent = [];

        foreach (string filename in Settings.startlist)
        {
            // If '.' is there in-place of the filename then stop because
            // '.' states that the following startup files are disabled, so don't run them.
            if (filename == ".")
                break;

            // Only if a file endswith '.aos' suffix then run that file at startup,
            // this is to ensure that startup files are not imposters.
            else if (filename.EndsWith(".aos"))
            {
                string startup_file_data = Path.Combine(Obsidian.root_dir, "Files.x72\\etc\\Startup\\", filename);
                StartupAppsContent.Add(filename, startup_file_data);
            }

            // If someone is trying to run a Non-AOs script (scripts that does not contain '.aos' suffix) then, throw error that this is not allowed.
            // IDK. Maybe change this in future and allow all kinds of files to run at startup but for now it is what it is.
            else
                _ = new Error($"Can't open file '{filename}': File format not recognized. File must have '.aos' extension.", "filesystem i/o error");
        }

        return StartupAppsContent;
    }
}
