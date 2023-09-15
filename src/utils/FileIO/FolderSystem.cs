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

            // Directory.Copy(Source, Destination, true);
        }

        public static string[] Read(string Directoryname)
        {
            return Directory.GetFileSystemEntries(Directoryname, "*");
        }
    }
}
