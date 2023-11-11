using System.Text.Json;

partial class DeveloperFeatures
{
    private void Tasks()
    {
        string json_data = FileIO.FileSystem.ReadAllText("AOs.dev\\tasks.json");

        if (Utils.String.IsEmpty(json_data))
        {
            Console.WriteLine("null");
            return;
        }

        TasksConfig tasks = JsonSerializer.Deserialize<TasksConfig>(json_data);

        int count = 1;
        foreach (var task in tasks.tasks)
        {
            TerminalColor.Print($"{count}. ", ConsoleColor.DarkGray, false);
            TerminalColor.Print(string.Format("{0," + -Utils.Maths.CalculatePadding(count) + "}", task.Key), ConsoleColor.Gray, false);
            TerminalColor.Print(task.Value.description, ConsoleColor.DarkGray);
            count++;
        }
    }
}
