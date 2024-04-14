partial class Terminal
{
    partial class ReadLine
    {
        private void GetAutocompleteSuggestions(string str)
        {
            // If the string is empty then return an empty array
            if (Utils.String.IsEmpty(str))
                Suggestions = [];

            // Get the directory name from the current string and if the directory exists then,
            // set it as the search path otherwise set the current dir as search path
            string path = Path.Exists(str) ? str : Directory.GetCurrentDirectory();

            // Find all the matching files, folders and commands if they start with 'str'
            List<string> entries = FileIO.FolderSystem.Read(path).Where(dir => dir.StartsWith(str)).Select(Path.GetFileName).ToList();
            List<string> matching_commands = EntryPoint.Settings.cmds.Where(cmd => cmd.do_index).SelectMany(cmd => cmd.cmd_names).Where(cmd => cmd.StartsWith(str)).ToList();

            // Add all of them to suggestions list
            Suggestions = [];
            Suggestions.AddRange(entries);
            Suggestions.AddRange(matching_commands);
        }
    }
}
