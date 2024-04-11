using System.Text.Json;

class History
{
    public static void Set(string cmd)
    {
        string CurrentTime = DateTime.Now.ToString("[dd-MM-yyyy HH:mm:ss]");

        // Read and Deserialize the history json text object from the file.
        string HistoryFilepath = Path.Combine(Obsidian.root_dir, "Files.x72\\root\\history.json");
        string JsonTextData = FileIO.FileSystem.ReadAllText(HistoryFilepath);
        HistoryObj history;

        // If the 'history.json' file is empty then save a new file.
        if (Utils.String.IsEmpty(JsonTextData))
        {
            history = new()
            {
                history = new()
                {
                    {
                        Obsidian.SessionTime, new List<HistoryTemplate>()
                        {
                            new()
                            {
                                command = cmd,
                                time = CurrentTime
                            }
                        }
                    }
                }
            };
        }

        // Save the command into history inside the current session of AOs
        else
        {
            history = JsonSerializer.Deserialize<HistoryObj>(JsonTextData);

            if (!history.history.ContainsKey(Obsidian.SessionTime))
                history.history[Obsidian.SessionTime] = [];

            history.history[Obsidian.SessionTime].Add(new HistoryTemplate
            {
                command = cmd,
                time = CurrentTime
            });
        }

        string JsonData = JsonSerializer.Serialize(history, new JsonSerializerOptions { WriteIndented = true });
        FileIO.FileSystem.Overwrite(HistoryFilepath, JsonData);
    }

    // Load the history and print it.
    public static void Get()
    {
        // Read and Deserialize the history json text object from the file.
        string HistoryFilepath = Path.Combine(Obsidian.root_dir, "Files.x72\\root\\history.json");
        string JsonData = FileIO.FileSystem.ReadAllText(HistoryFilepath);
        HistoryObj history = JsonSerializer.Deserialize<HistoryObj>(JsonData);

        string DateTimeFormat = "[dd-MM-yyyy HH:mm:ss]"; // time format

        // Loop through all sessions in the history.
        foreach (var Details in history.history)
        {
            // Get the session time.
            string Session = Details.Key;
            List<HistoryTemplate> HistoryDetails = Details.Value;

            // Convert the session string into DateTime format.
            DateTime.TryParseExact(
                Session, DateTimeFormat,
                System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None,
                out DateTime SessionDateTime
            );

            // Print history of every session.
            Terminal.Print($"[{SessionDateTime}]", ConsoleColor.White);
            for (int i = 0; i < HistoryDetails.Count; i++)
            {
                string Command = HistoryDetails[i].command;
                string Time = HistoryDetails[i].time;

                // Convert the time of that command to DateTime format.
                DateTime.TryParseExact(
                    Time, DateTimeFormat,
                    System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None,
                    out DateTime Datetime
                );

                // Calculate the padding between the command and the it's time.
                int padding = Utils.Maths.CalculatePadding(i+1, 100);

                // Print history.
                Terminal.Print($"{i+1}. ", ConsoleColor.DarkGray, false);
                Console.Write("{0," + -padding + "}", Command);
                Terminal.Print($"[{Datetime}]", ConsoleColor.DarkGray);
            }
        }
    }

    public static void Clear()
    {
        FileIO.FileSystem.Delete($"{Obsidian.root_dir}\\Files.x72\\root\\history.json");
    }

    private class HistoryObj
    {
        public Dictionary<string, List<HistoryTemplate>> history { get; set; }
    }

    private class HistoryTemplate
    {
        public string command { get; set; }
        public string time { get; set; }
    }
}
