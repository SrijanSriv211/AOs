partial class FileIO
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
            Create(Filepath);
            File.AppendAllText(Filepath, Content);
        }

        public static void Overwrite(string Filepath, string[] Content)
        {
            Create(Filepath);
            File.WriteAllLines(Filepath, Content);
        }

        public static void Delete(string Filepath)
        {
            if (File.Exists(Filepath))
                File.Delete(Filepath);

            else
                new Error($"'{Filepath}' does not exist");
        }

        public static void Move(string Source, string Destination)
        {
            if (!File.Exists(Source))
            {
                new Error($"'{Source}' does not exist");
                return;
            }

            if (Utils.String.IsEmpty(Destination))
            {
                new Error($"'{Destination}' cannot be null or blank.");
                return;
            }

            File.Move(Source, Destination);
        }

        public static void Copy(string Source, string Destination)
        {
            if (!File.Exists(Source))
            {
                new Error($"'{Source}' does not exist");
                return;
            }

            if (Utils.String.IsEmpty(Destination))
            {
                new Error($"'{Destination}' cannot be null or blank.");
                return;
            }

            File.Copy(Source, Destination, true);
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
}
