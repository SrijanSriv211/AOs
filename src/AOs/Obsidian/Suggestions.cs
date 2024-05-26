partial class Obsidian
{
    // Find all the matching files, folders and commands if they start with 'str'
    private List<string> GetAllSuggestions()
    {
        List<string> Suggestions = EntryPoint.Settings.cmd_schema.Where(cmd => cmd.do_index).SelectMany(cmd => cmd.cmd_names).ToList();

        // Read and deserialize history.json
        string HistoryFilepath = Path.Combine(root_dir, "Files.x72\\root\\history.json");
        if (!File.Exists(HistoryFilepath) || Utils.String.IsEmpty(FileIO.FileSystem.ReadAllText(HistoryFilepath)))
            return Utils.Array.Filter(Suggestions.ToArray()).ToList();

        string JsonData = FileIO.FileSystem.ReadAllText(HistoryFilepath);
        History.HistoryObj history = System.Text.Json.JsonSerializer.Deserialize<History.HistoryObj>(JsonData);

        // Loop through all sessions in the history.
        foreach (var Details in history.history)
            Suggestions.AddRange(Details.Value.Select(x => x.command));

        return Utils.Array.Filter(Suggestions.ToArray()).ToList();
    }
}
