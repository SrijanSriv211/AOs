class FileIO
{
    public class FileSystem
    {
        public static void Create(string Filepath)
        {
            if (!File.Exists(Filepath))
            {
                // Create the folder if it doesn't exist.
                string FolderPath = Path.GetDirectoryName(Filepath);
                if (!Utils.String.IsEmpty(FolderPath))
                    FolderSystem.Create(FolderPath);

                // Create the file in that folder.
                File.Create(Filepath).Dispose();
            }
        }

        public static void Write(string Filepath, string Content)
        {
            if (!File.Exists(Filepath))
            {
                // Create the folder if it doesn't exist.
                string FolderPath = Path.GetDirectoryName(Filepath);
                if (!Utils.String.IsEmpty(FolderPath))
                    FolderSystem.Create(FolderPath);

                // Create the file in that folder.
                File.Create(Filepath).Dispose();
            }

            File.AppendAllText(Filepath, Content);
        }

        public static void Delete(string Filepath)
        {
            if (File.Exists(Filepath))
                File.Delete(Filepath);

            else
                new Error($"'{Filepath}' does not exist");
        }

        public static string ReadAllText(string Filepath)
        {
            if (File.Exists(Filepath))
                return File.ReadAllText(Filepath);

            return string.Empty;
        }

        public static string[] ReadAllLines(string Filepath)
        {
            if (File.Exists(Filepath))
                return File.ReadAllLines(Filepath);

            return new string[0];
        }
    }

    public class FolderSystem
    {
        public static void Create(string Directoryname)
        {
            if (!Directory.Exists(Directoryname))
                Directory.CreateDirectory(Directoryname);
        }

        public static void Delete(string Directoryname)
        {
            if (Directory.Exists(Directoryname))
                Directory.Delete(Directoryname, true);

            else
                new Error($"'{Directoryname}' does not exist");
        }

        public static string[] Read(string Directoryname)
        {
            return Directory.GetFileSystemEntries(Directoryname, "*");
        }
    }
}
