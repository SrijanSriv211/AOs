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
            try
            {
                File.AppendAllText(Filepath, Content);
            }

            catch (Exception e)
            {
                new Error(e.Message);
                EntryPoint.CrashreportLog(e.ToString());
            }
        }

        public static void Overwrite(string Filepath, string[] Content)
        {
            Create(Filepath);
            try
            {
                File.WriteAllLines(Filepath, Content);
            }

            catch (Exception e)
            {
                new Error(e.Message);
                EntryPoint.CrashreportLog(e.ToString());
            }
        }

        public static void Overwrite(string Filepath, string Content)
        {
            Create(Filepath);
            try
            {
                File.WriteAllText(Filepath, Content);
            }

            catch (Exception e)
            {
                new Error(e.Message);
                EntryPoint.CrashreportLog(e.ToString());
            }
        }

        public static void Delete(string Filepath)
        {
            if (File.Exists(Filepath))
            {
                try
                {
                    File.Delete(Filepath);
                }

                catch (Exception e)
                {
                    new Error(e.Message);
                    EntryPoint.CrashreportLog(e.ToString());
                }
            }

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

            try
            {
                File.Move(Source, Destination);
            }

            catch (Exception e)
            {
                new Error(e.Message);
                EntryPoint.CrashreportLog(e.ToString());
            }
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

            try
            {
                File.Copy(Source, Destination, true);
            }

            catch (Exception e)
            {
                new Error(e.Message);
                EntryPoint.CrashreportLog(e.ToString());
            }
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
