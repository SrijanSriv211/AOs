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
                _ = new Error(e.Message, "filesystem i/o error");
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
                _ = new Error(e.Message, "filesystem i/o error");
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
                _ = new Error(e.Message, "filesystem i/o error");
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
                    _ = new Error(e.Message, "filesystem i/o error");
                    EntryPoint.CrashreportLog(e.ToString());
                }
            }

            else
                _ = new Error($"'{Filepath}' does not exist", "filesystem i/o error");
        }

        public static void Move(string Source, string Destination)
        {
            if (!File.Exists(Source))
            {
                _ = new Error($"'{Source}' does not exist", "filesystem i/o error");
                return;
            }

            if (Utils.String.IsEmpty(Destination))
            {
                _ = new Error($"'{Destination}' cannot be null or blank.", "filesystem i/o error");
                return;
            }

            try
            {
                File.Move(Source, Destination);
            }

            catch (Exception e)
            {
                _ = new Error(e.Message, "filesystem i/o error");
                EntryPoint.CrashreportLog(e.ToString());
            }
        }

        public static void Copy(string Source, string Destination)
        {
            if (!File.Exists(Source))
            {
                _ = new Error($"'{Source}' does not exist", "filesystem i/o error");
                return;
            }

            if (Utils.String.IsEmpty(Destination))
            {
                _ = new Error($"'{Destination}' cannot be null or blank.", "filesystem i/o error");
                return;
            }

            try
            {
                File.Copy(Source, Destination, true);
            }

            catch (Exception e)
            {
                _ = new Error(e.Message, "filesystem i/o error");
                EntryPoint.CrashreportLog(e.ToString());
            }
        }

        public static string ReadAllText(string Filepath, bool FileStream=false)
        {
            if (File.Exists(Filepath))
            {
                if (!FileStream)
                    return File.ReadAllText(Filepath);

                using var StreamReader = new StreamReader(new FileStream(Filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
                return StreamReader.ReadToEnd();
            }

            return "";
        }

        public static string[] ReadAllLines(string Filepath, bool FileStream=false)
        {
            List<string> Lines = [];

            if (File.Exists(Filepath))
            {
                if (!FileStream)
                    return File.ReadAllLines(Filepath);

                string line;
                using var StreamReader = new StreamReader(new FileStream(Filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
                while ((line = StreamReader.ReadLine()) != null)
                    Lines.Add(line);
            }

            return [.. Lines];
        }
    }
}
