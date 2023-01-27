using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

public class Features
{
    public static void overload(string[] input_args)
    {
        string[] PromptHelpCenter = {
                "Displays overload commands.",
                "Usage: @[args]",
                "",
                "Arguments:",
                "itsmagic    -> It's magic It's magic.",
                "studybyte   -> Starts studybyte.",
                "deeplock    -> Locks windows itself.",
                "deepscan    -> Scans the host operating system.",
                "todo        -> Create and manages a todo list.",
                "help        -> Displays a list of all overload commands."
            };

        if (Collection.Array.IsEmpty(input_args)) Console.WriteLine(string.Join("\n", PromptHelpCenter));
        else if (Obsidian.Shell.IsAskingForHelp(input_args.FirstOrDefault())) Console.WriteLine(string.Join("\n", PromptHelpCenter));
        else if (input_args.FirstOrDefault().ToLower() == "itsmagic") Obsidian.Shell.StartApp("https://youtu.be/dQw4w9WgXcQ"); // Rickroll!!!
        else if (input_args.FirstOrDefault().ToLower() == "studybyte") Obsidian.Shell.StartApp("https://light-lens.github.io/Studybyte");
        else if (input_args.FirstOrDefault().ToLower() == "deeplock") Obsidian.Shell.StartApp(@"C:\WINDOWS\system32\rundll32.exe", "user32.dll,LockWorkStation");
        else if (input_args.FirstOrDefault().ToLower() == "deepscan") Obsidian.Shell.StartApp($"{Obsidian.rDir}\\Sysfail\\rp\\FixCorruptedSystemFiles.bat", AppAdmin: true);
        else if (input_args.FirstOrDefault().ToLower() == "todo")
        {
            string[] TODOHelp = {
                                "Displays a list of all todo arguments",
                                "Usage: @todo [args] [taskname]",
                                "",
                                "Arguments:",
                                "add      -> Adds a task.",
                                "delete   -> Deletes a task.",
                                "cut      -> Marks the task as completed.",
                                "list     -> Shows all pending tasks.",
                                "done     -> Shows all completed tasks.",
                                "help     -> Displays a list of all todo arguments."
                            };

            string[] TODOArgs = Collection.Array.Trim(input_args.Skip(1).ToArray());
            if (TODOArgs.Length == 0 || TODOArgs == null) Console.WriteLine(string.Join("\n", TODOHelp));
            else if (Obsidian.Shell.IsAskingForHelp(TODOArgs.FirstOrDefault()) && TODOArgs.Length == 1) Console.WriteLine(string.Join("\n", TODOHelp));
            else if (TODOArgs.FirstOrDefault() == "list" && TODOArgs.Length == 1)
            {
                string[] Tasks = File.ReadLines($"{Obsidian.rDir}\\Files.x72\\etc\\TODO.txt").ToArray();
                for (int i = 0; i < Tasks.Length; i++) Console.WriteLine($"{i}. {Tasks[i]}");
            }

            else if (TODOArgs.FirstOrDefault() == "done" && TODOArgs.Length == 1)
            {
                string[] Tasks = File.ReadLines($"{Obsidian.rDir}\\Files.x72\\etc\\DONE.txt").ToArray();
                for (int i = 0; i < Tasks.Length; i++) Console.WriteLine($"{i}. {Tasks[i]}");
            }

            else if (TODOArgs.Length > 2) Error.TooManyArgs(TODOArgs);
            else if (TODOArgs.Length < 2) Error.TooFewArgs(TODOArgs);
            else if (TODOArgs.FirstOrDefault() == "add")
            {
                if (!File.Exists($"{Obsidian.rDir}\\Files.x72\\etc\\TODO.txt")) File.Create($"{Obsidian.rDir}\\Files.x72\\etc\\TODO.txt").Dispose();
                string TaskName = Obsidian.Shell.Strings(TODOArgs[1]);
                string[] Tasks = File.ReadLines($"{Obsidian.rDir}\\Files.x72\\etc\\TODO.txt").ToArray();

                if (!Tasks.Any(TaskName.Contains)) File.AppendAllText($"{Obsidian.rDir}\\Files.x72\\etc\\TODO.txt", $"{TaskName}\n");
                else Console.WriteLine("This task already exists.");
            }

            else if (TODOArgs.FirstOrDefault() == "delete")
            {
                string TaskName = Obsidian.Shell.Strings(TODOArgs[1]);
                if (File.Exists($"{Obsidian.rDir}\\Files.x72\\etc\\TODO.txt"))
                {
                    string[] Tasks = File.ReadLines($"{Obsidian.rDir}\\Files.x72\\etc\\TODO.txt").ToArray();
                    if (Tasks.Any(TaskName.Contains))
                    {
                        List<string> TaskList = new List<string>(Tasks);
                        TaskList.Remove(TaskName);
                        Tasks = TaskList.ToArray();
                        File.WriteAllLines($"{Obsidian.rDir}\\Files.x72\\etc\\TODO.txt", Tasks);
                    }

                    else Console.WriteLine("No such task found.");
                }

                else Console.WriteLine("No todo list found.");
            }

            else if (TODOArgs.FirstOrDefault() == "cut")
            {
                string TaskName = Obsidian.Shell.Strings(TODOArgs[1]);
                if (File.Exists($"{Obsidian.rDir}\\Files.x72\\etc\\TODO.txt"))
                {
                    string[] Tasks = File.ReadLines($"{Obsidian.rDir}\\Files.x72\\etc\\TODO.txt").ToArray();
                    if (Tasks.Any(TaskName.Contains))
                    {
                        List<string> TaskList = new List<string>(Tasks);
                        TaskList.Remove(TaskName);
                        Tasks = TaskList.ToArray();
                        File.WriteAllLines($"{Obsidian.rDir}\\Files.x72\\etc\\TODO.txt", Tasks);
                        File.AppendAllText($"{Obsidian.rDir}\\Files.x72\\etc\\DONE.txt", $"{TaskName}\n");
                    }

                    else Console.WriteLine("No such task found.");
                }

                else Console.WriteLine("No todo list found.");
            }

            else Error.Args(input_args);
        }

        else Error.Args(input_args);
    }

    public static void wait(string[] input_args)
    {
        if (Collection.Array.IsEmpty(input_args))
        {
            Console.Write("No. of Seconds$ ");
            int Sec = Convert.ToInt32(Console.ReadLine());

            Obsidian.Shell.Track(Sec * 1000, description: "Waiting...");
        }

        else if (int.TryParse(input_args.FirstOrDefault(), out int Sec) && input_args.Length == 1) Obsidian.Shell.Track(Sec * 1000, description: "Waiting...");
        else Error.Args(input_args);
    }

    public static void cat(string[] applist)
    {
        foreach (string appname in applist)
        {
            var proc = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "powershell",
                    Arguments = $"Get-StartApps {appname} | Select-Object -ExpandProperty AppID",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            proc.Start();
            string AppID = proc.StandardOutput.ReadToEnd();
            proc.Close();

            if (Collection.String.IsEmpty(AppID)) new Error($"App {appname} not found");
            else Obsidian.Shell.CommandPrompt($"start explorer shell:appsfolder\\{AppID}");
        }
    }
}
