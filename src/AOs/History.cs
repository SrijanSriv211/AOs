class History
{
    public static void Set(string cmd)
    {
        string CurrentTime = DateTime.Now.ToString("[dd-MM-yyyy HH:mm:ss]");
        FileIO.FileSystem.Write($"{Obsidian.rootDir}\\Files.x72\\root\\.history", $"{CurrentTime}\n'{cmd}'\n\n");
    }

    public static void Get()
    {
        string format = "[dd-MM-yyyy HH:mm:ss]";

        string[] history = FileIO.FileSystem.ReadAllLines($"{Obsidian.rootDir}\\Files.x72\\root\\.history");

        for (int i = 0; i < history.Length; i++)
        {
            if (Utils.String.IsEmpty(history[i]))
                continue;

            DateTime.TryParseExact(history[i], format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime datetime);

            Console.Write(history[i+1] + " ");
            new TerminalColor($"[{datetime}]", ConsoleColor.DarkGray);
            i++;
        }
    }

    public static void Clear()
    {
        FileIO.FileSystem.Delete($"{Obsidian.rootDir}\\Files.x72\\root\\.history");
    }
}
