class History
{
    public static void Set(string cmd)
    {
        string CurrentTime = DateTime.Now.ToString("[dd-MM-yyyy HH:mm:ss]");
        FileIO.FileSystem.Write($"{Obsidian.root_dir}\\Files.x72\\root\\.history", $"{CurrentTime}\n'{cmd}'\n\n");
    }

    public static void Get()
    {
        string[] history = FileIO.FileSystem.ReadAllLines($"{Obsidian.root_dir}\\Files.x72\\root\\.history");
        string format = "[dd-MM-yyyy HH:mm:ss]";
        int count = 1;

        for (int i = 0; i < history.Length; i++)
        {
            if (Utils.String.IsEmpty(history[i]))
                continue;

            DateTime.TryParseExact(history[i], format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out DateTime datetime);

            int padding = Math.Max(100 - (int)Math.Log10(count), 0);

            Console.Write("{0}. {1," + -padding + "}", count, $"{history[i+1]}");
            new TerminalColor($"[{datetime}]", ConsoleColor.DarkGray);

            count++;
            i++;
        }
    }

    public static void Clear()
    {
        FileIO.FileSystem.Delete($"{Obsidian.root_dir}\\Files.x72\\root\\.history");
    }
}
