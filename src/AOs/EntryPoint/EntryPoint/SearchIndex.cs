partial class EntryPoint
{
    //TODO: Improve this function. Move all extentions, search paths and excluded items to json
    public static void SearchIndex()
    {
        // // https://gist.github.com/ppisarczyk/43962d06686722d26d176fad46879d41
        // List<string> extensions = [
        //     ".exe", ".msi", ".bat", ".cmd", ".ps", ".txt", ".pdf",
        //     ".c", ".h", ".cs", ".cpp", ".c++", ".h++", ".hpp",
        //     ".cmake", ".cmake.in",
        //     ".css", ".js", ".html", ".htm", ".xhtml",
        //     ".csv", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx",
        //     ".py", ".ipynb",
        //     ".dockerfile",
        //     ".json", ".lock", "jsonl",
        //     ".java",
        //     ".kt",
        //     ".md", ".markdown"
        // ];

        // string[] search_paths = [
        //     "C:\\Program Files",
        //     "C:\\Program Files (x86)",
        //     "C:\\Users\\SrijanSrivastava\\Desktop",
        //     "C:\\Users\\SrijanSrivastava\\Downloads",
        //     "D:\\"
        // ];

        // string[] excluded_items = [
        //     ".venv"
        // ];

        // // https://stackoverflow.com/a/9830116/18121288
        // List<string> FileSystemEntries = [];

        // foreach (string path in search_paths)
        // {
        //     IEnumerable<string> Entries = Directory.EnumerateFileSystemEntries(path, "*", new EnumerationOptions {
        //         IgnoreInaccessible = true,
        //         RecurseSubdirectories = true
        //     }).Where(x => extensions.IndexOf(Path.GetExtension(x)) >= 0);

        //     foreach (string excluded_item in excluded_items)
        //         FileSystemEntries.AddRange(Entries.Where(x => x.Split("\\").ToList().IndexOf(excluded_item) < 0));
        // }

        // FileIO.FileSystem.Overwrite("Files.x72\\root\\search_index", string.Join("\n", FileSystemEntries));
    }
}
