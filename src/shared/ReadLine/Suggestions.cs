using System.Text.Json;

partial class ReadLine
{
    private List<string> SuggestCommands(string str)
    {
        // Get the directory name from the current string and if the directory exists then,
        // set it as the search path otherwise set the current dir as search path
        string path = Path.Exists(str) ? str : Directory.GetCurrentDirectory();

        // Find all the matching files, folders and commands if they start with 'str'
        return EntryPoint.Settings.cmd_schema.Where(cmd => cmd.do_index).SelectMany(cmd => cmd.cmd_names).Where(cmd => cmd.StartsWith(str)).ToList();
    }

    // Load all the commands from history and suggest them.
    private List<string> SuggestHistoricalCommands(string str)
    {
        // Read and deserialize history.json
        string HistoryFilepath = Path.Combine(Obsidian.root_dir, "Files.x72\\root\\history.json");
        string JsonData = FileIO.FileSystem.ReadAllText(HistoryFilepath);
        History.HistoryObj history = JsonSerializer.Deserialize<History.HistoryObj>(JsonData);

        // Loop through all sessions in the history.
        List<string> HistoricalCommands = [];
        foreach (var Details in history.history)
            HistoricalCommands.AddRange(Details.Value.Select(x => x.command).Where(x => x.StartsWith(str)));

        return Utils.Array.Filter(HistoricalCommands.ToArray()).ToList();
    }
}
