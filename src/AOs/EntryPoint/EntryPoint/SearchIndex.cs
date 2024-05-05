partial class EntryPoint
{
    public static void SearchIndex()
    {
        // https://gist.github.com/ppisarczyk/43962d06686722d26d176fad46879d41
        List<string> extensions = EntryPoint.Settings.search_index.extensions.ToList();
        string[] search_paths = EntryPoint.Settings.search_index.search_paths;
        string[] excluded_items = EntryPoint.Settings.search_index.search_paths;

        // https://stackoverflow.com/a/9830116/18121288
        List<string> FileSystemEntries = [];

        var watch = System.Diagnostics.Stopwatch.StartNew();

        foreach (string path in search_paths)
        {
            // Split the path and loop through each path component to find any path which uses any env var.
            // If yes, then replace those %env-var% with the actual value of the environment var.
            string[] split_path = path.Contains("\\") ? path.Split("\\") : path.Split("/");
            for (int i = 0; i < split_path.Length; i++)
            {
                if (split_path[i].StartsWith("%") && split_path[i].EndsWith("%"))
                    split_path[i] = Environment.GetEnvironmentVariable(split_path[i][1..^1]);
            }

            // Index all files
            IEnumerable<string> Entries = Directory.EnumerateFileSystemEntries(string.Join("\\", split_path), "*", new EnumerationOptions {
                IgnoreInaccessible = true,
                RecurseSubdirectories = true
            }).Where(x => extensions.IndexOf(Path.GetExtension(x)) >= 0);

            // Exclude items from the index
            foreach (string excluded_item in excluded_items)
                FileSystemEntries.AddRange(Entries.Where(x => x.Split("\\").ToList().IndexOf(excluded_item) < 0));
        }

        FileSystemEntries = Utils.Array.Filter(FileSystemEntries.ToArray()).ToList();
        FileIO.FileSystem.Overwrite(Path.Combine(Obsidian.root_dir, "Files.x72\\root\\search_index"), string.Join("\n", FileSystemEntries));
    }
}
