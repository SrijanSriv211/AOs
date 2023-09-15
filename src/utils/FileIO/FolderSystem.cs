partial class FileIO
{
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

        public static void Move(string Source, string Destination)
        {
            if (!Directory.Exists(Source))
            {
                new Error($"'{Source}' does not exist");
                return;
            }

            if (Utils.String.IsEmpty(Destination))
            {
                new Error($"'{Destination}' cannot be null or blank.");
                return;
            }

            Directory.Move(Source, Destination);
        }

        public static void Copy(string Source, string Destination)
        {
            if (!Directory.Exists(Source))
            {
                new Error($"'{Source}' does not exist");
                return;
            }

            if (Utils.String.IsEmpty(Destination))
            {
                new Error($"'{Destination}' cannot be null or blank.");
                return;
            }

            // https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories
            // Get information about the source directory
            var dir = new DirectoryInfo(Source);

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory
            Create(Destination);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                string Target_Filepath = Path.Combine(Destination, file.Name);
                file.CopyTo(Target_Filepath);
            }

            // copy subdirectories recursively
            foreach (DirectoryInfo subDir in dirs)
            {
                string newDestinationDir = Path.Combine(Destination, subDir.Name);
                Copy(subDir.FullName, newDestinationDir);
            }
        }

        public static string[] Read(string Directoryname)
        {
            return Directory.GetFileSystemEntries(Directoryname, "*");
        }
    }
}
