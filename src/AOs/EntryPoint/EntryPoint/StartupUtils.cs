partial class EntryPoint
{
    // Read the '.startlist' file and get all the files
    // that are meant to be run at AOs' startup.
    private static string[] LoadStartlist()
    {
        string startlist_path = Path.Combine(Obsidian.root_dir, "Files.x72\\etc\\Startup\\.startlist");
        return FileIO.FileSystem.ReadAllLines(startlist_path);
    }

    // Read all the files that are meant to run at startup.
    private static Dictionary<string, string[]> ReadAllStartupApps(string[] filenames)
    {
        // Dictionary<string, string[]>:
        // string -> filename, string[] -> all it's lines that are meant to be executed.
        Dictionary<string, string[]> StartupAppsContent = new();

        foreach (string filename in filenames)
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
                StartupAppsContent.Add(filename, FileIO.FileSystem.ReadAllLines(startup_file_data));
            }

            // If someone is trying to run a Non-AOs script (scripts that does not contain '.aos' suffix) then, throw error that this is not allowed.
            // IDK. Maybe change this in future and allow all kinds of files to run at startup but for now it is what it is.
            else
                _ = new Error($"Can't open file '{filename}': Non-AOs scripts are not allowed to run at startup.", "filesystem i/o error");
        }

        return StartupAppsContent;
    }
}
