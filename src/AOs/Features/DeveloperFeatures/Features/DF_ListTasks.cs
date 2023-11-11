using System.Text.Json;

partial class DeveloperFeatures
{
    private void Tasks(string[] task_args)
    {
        string json_data = FileIO.FileSystem.ReadAllText("AOs.dev\\tasks.json");

        if (Utils.String.IsEmpty(json_data))
        {
            Console.WriteLine("null");
            return;
        }

        TasksConfig tasks = JsonSerializer.Deserialize<TasksConfig>(json_data);

        if (Utils.Array.IsEmpty(task_args))
            ListTasks(tasks);

        else
            ExecTask(task_args, tasks);
    }

    private static void ListTasks(TasksConfig tasks)
    {
        int count = 1;
        foreach (var task in tasks.tasks)
        {
            TerminalColor.Print($"{count}. ", ConsoleColor.DarkGray, false);
            TerminalColor.Print(string.Format("{0," + -Utils.Maths.CalculatePadding(count) + "}", task.Key), ConsoleColor.Gray, false);
            TerminalColor.Print(task.Value.description, ConsoleColor.DarkGray);
            count++;
        }
    }

    private void ExecTask(string[] args, TasksConfig tasks)
    {
        string task_name = Utils.String.Strings(args.FirstOrDefault());
        string[] task_args = Utils.Array.Trim(args.Skip(1).ToArray());

        foreach (var task in tasks.tasks)
        {
            if (task_name == task.Key)
            {
                sys_utils.CommandPrompt(task.Value.command + " " + string.Join(" ", task_args));

                if (task.Value.update_build_number)
                {
                    string json_data = FileIO.FileSystem.ReadAllText("AOs.dev\\project.json");
                    ProjectTemplate project_data = JsonSerializer.Deserialize<ProjectTemplate>(json_data);
                    project_data.build++;

                    var options = new JsonSerializerOptions { WriteIndented = true };
                    string project_json_obj = JsonSerializer.Serialize(project_data, options);
                    FileIO.FileSystem.Overwrite("AOs.dev\\project.json", project_json_obj);
                }
            }
        }
    }
}
