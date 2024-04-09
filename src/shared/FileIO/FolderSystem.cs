using System.IO.Compression;

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
            {
                try
                {
                    Directory.Delete(Directoryname, true);
                }

                catch (Exception e)
                {
                    new Error(e.Message, "filesystem i/o error");
                    EntryPoint.CrashreportLog(e.ToString());
                }
            }

            else
                new Error($"'{Directoryname}' does not exist", "filesystem i/o error");
        }

        public static void Move(string Source, string Destination)
        {
            if (!Directory.Exists(Source))
            {
                new Error($"'{Source}' does not exist", "filesystem i/o error");
                return;
            }

            if (Utils.String.IsEmpty(Destination))
            {
                new Error($"'{Destination}' cannot be null or blank.", "filesystem i/o error");
                return;
            }

            try
            {
                Directory.Move(Source, Destination);
            }

            catch (Exception e)
            {
                new Error(e.Message, "filesystem i/o error");
                EntryPoint.CrashreportLog(e.ToString());
            }
        }

        public static void Copy(string Source, string Destination)
        {
            if (!Directory.Exists(Source))
            {
                new Error($"'{Source}' does not exist", "filesystem i/o error");
                return;
            }

            if (Utils.String.IsEmpty(Destination))
            {
                new Error($"'{Destination}' cannot be null or blank.", "filesystem i/o error");
                return;
            }

            try
            {
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

            catch (Exception e)
            {
                new Error(e.Message, "filesystem i/o error");
                EntryPoint.CrashreportLog(e.ToString());
            }
        }

        public static string[] Read(string Directoryname)
        {
            if (!Directory.Exists(Directoryname))
                return [];

            // Get file and folder names without full paths
            string[] FileSystemEntries = Directory.GetFileSystemEntries(Directoryname, "*");

            // Separate directories and files
            string[] Directories = FileSystemEntries.Where(Directory.Exists).ToArray();
            string[] Files = FileSystemEntries.Where(File.Exists).ToArray();

            // Concatenate directories and files in the desired order
            return Directories.Concat(Files).Select(Path.GetFileName).ToArray();
        }

        public static void Compress(string Source_dirname, string zip_path)
        {
            if (!Directory.Exists(Source_dirname))
            {
                new Error($"{Source_dirname}: No such directory.", "filesystem i/o error");
                return;
            }

            try
            {
                ZipFile.CreateFromDirectory(Source_dirname, zip_path);
            }

            catch (Exception e)
            {
                new Error(e.Message, "filesystem i/o error");
                EntryPoint.CrashreportLog(e.ToString());
            }
        }

        public static void Decompress(string zip_path, string Destination_dirname)
        {
            if (!File.Exists(zip_path))
            {
                new Error($"{zip_path}: No such file.", "filesystem i/o error");
                return;
            }

            try
            {
                Create(Destination_dirname);
                ZipFile.ExtractToDirectory(zip_path, Destination_dirname, true);
            }

            catch (Exception e)
            {
                new Error(e.Message, "filesystem i/o error");
                EntryPoint.CrashreportLog(e.ToString());
            }
        }
    }
}
