using System.IO;

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
        }

        public static string Read(string Filepath)
        {
            if (File.Exists(Filepath))
                return File.ReadAllText(Filepath);

            return null;
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
                Directory.Delete(Directoryname);
        }

        public static string[] Read(string Directoryname)
        {
            //TODO: Read all the Filepaths and other dirnames in this folder. it can later be used for the 'ls' command.
            return new string[0];
        }
    }
}
