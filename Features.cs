using System;
using System.IO;
using System.Text;
using System.Linq;
using Microsoft.Win32;
using System.Diagnostics;
using System.Collections.Generic;

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

            Obsidian.Shell.Track(1000, Sec, "Waiting...");
        }

        else if (int.TryParse(input_args.FirstOrDefault(), out int Sec) && input_args.Length == 1) Obsidian.Shell.Track(1000, Sec, "Waiting...");
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

    public static void ls(string[] input_args)
    {
        if (Collection.Array.IsEmpty(input_args))
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
            string[] Entries = Directory.GetFileSystemEntries(".", "*");
            foreach (string Entry in Entries) Console.WriteLine(Entry);
        }

        else
        {
            Console.WriteLine(Directory.GetCurrentDirectory());
            foreach (string i in input_args)
            {
                if (i.ToLower() == "-h" || i.ToLower() == "/?" || i.ToLower() == "--help")
                {
                    string[] LSHelpCenter = {
                                    "Displays a list of files and subdirectories in a directory.",
                                    "Usage: ls [Option]",
                                    "",
                                    "Options:",
                                    "-f   -> Display only files.",
                                    "-d   -> Display only folders.",
                                    "-a   -> Display all files and folders.",
                                };

                    Console.WriteLine(string.Join("\n", LSHelpCenter));
                    break;
                }

                else if (i.ToLower() == "-a" || i.ToLower() == "--all")
                {
                    string[] Entries = Directory.GetFileSystemEntries(".", "*");
                    foreach (string Entry in Entries) Console.WriteLine(Entry);
                    break;
                }

                else if (i.ToLower() == "-f" || i.ToLower() == "--files")
                {
                    string[] Files = Directory.GetFiles(".");
                    foreach (string File in Files) Console.WriteLine(File);
                }

                else if (i.ToLower() == "-d" || i.ToLower() == "--folder" || i.ToLower() == "--directories")
                {
                    string[] Directories = Directory.GetDirectories(".");
                    foreach (string Folder in Directories) Console.WriteLine(Folder);
                }

                else
                {
                    Error.Args(i);
                    break;
                }
            }
        }
    }

    public static void touch(string[] input_args)
    {
        string FileOrFolderName = Obsidian.Shell.Strings(string.Join(" ", input_args));
        if (FileOrFolderName.ToString().ToLower() == "con") Console.WriteLine("Hello CON!");
        else if ((FileOrFolderName.EndsWith("\\") || FileOrFolderName.EndsWith("/")) && !Directory.Exists(FileOrFolderName))
            Directory.CreateDirectory(FileOrFolderName.Substring(0, FileOrFolderName.Length - 1));

        else if (!File.Exists(FileOrFolderName) && !Directory.Exists(FileOrFolderName)) File.Create(FileOrFolderName).Dispose();
        else Console.WriteLine("File or directory already exist.");
    }

    public static void del(string[] input_args)
    {
        string FileOrFolderName = Obsidian.Shell.Strings(string.Join(" ", input_args));
        if (FileOrFolderName.EndsWith("\\") || FileOrFolderName.EndsWith("/"))
            FileOrFolderName = FileOrFolderName.Substring(0, FileOrFolderName.Length - 1);

        if (FileOrFolderName.ToString().ToLower() == "con") Console.WriteLine("Don't Delete CON.");
        else if (Directory.Exists(FileOrFolderName))
        {
            try
            {
                Directory.Delete(FileOrFolderName, true);
            }

            catch (System.UnauthorizedAccessException)
            {
                Obsidian.Shell.CommandPrompt($"rmdir {FileOrFolderName} /s /q");
            }
        }

        else if (File.Exists(FileOrFolderName)) File.Delete(FileOrFolderName);
        else new Error("No such file or directory.");
    }

    public static void ren(string[] input_args)
    {
        string FileOrFolderName = Obsidian.Shell.Strings(string.Join(" ", input_args));
        if (FileOrFolderName.EndsWith("\\") || FileOrFolderName.EndsWith("/"))
            FileOrFolderName = FileOrFolderName.Substring(0, FileOrFolderName.Length - 1);

        if (FileOrFolderName.ToString().ToLower() == "con") Console.WriteLine("Hello CON!");
        else if (Directory.Exists(FileOrFolderName) || File.Exists(FileOrFolderName)) Obsidian.Shell.CommandPrompt($"ren {FileOrFolderName}");
        else new Error("No such file or directory.");
    }

    public static void copy(string input_cmd, string[] input_args)
    {
        string FileOrFolderName = Obsidian.Shell.Strings(string.Join(" ", input_args));
        if (FileOrFolderName.EndsWith("\\") || FileOrFolderName.EndsWith("/"))
            FileOrFolderName = FileOrFolderName.Substring(0, FileOrFolderName.Length - 1);

        if (Directory.Exists(FileOrFolderName) || File.Exists(FileOrFolderName)) Obsidian.Shell.CommandPrompt($"{input_cmd} {FileOrFolderName}");
        else new Error("No such file or directory.");
    }

    public static void move(string[] input_args)
    {
        string FileOrFolderName = Obsidian.Shell.Strings(string.Join(" ", input_args));
        if (FileOrFolderName.EndsWith("\\") || FileOrFolderName.EndsWith("/"))
            FileOrFolderName = FileOrFolderName.Substring(0, FileOrFolderName.Length - 1);

        if (Directory.Exists(FileOrFolderName) || File.Exists(FileOrFolderName)) Obsidian.Shell.CommandPrompt($"move {FileOrFolderName}");
        else new Error("No such file or directory.");
    }

    public static void commit(string[] input_args)
    {
        for (int i = 0; i < input_args.Length; i++)
        {
            if (Collection.String.IsString(input_args[i])) input_args[i] = Obsidian.Shell.Strings(input_args[i]);
        }

        if (File.Exists(input_args[0]))
        {
            if (File.ReadLines(input_args[0]).FirstOrDefault() == "{4c4f4747494e4720544849532046494c45}")
            {
                File.AppendAllText(input_args[0], $"{input_args[1]}\n");
                File.AppendAllText(input_args[0], $"{DateTime.Now.ToString("[dd-MM-yyyy, HH:mm:ss]")}\n");
            }

            else File.AppendAllText(input_args[0], $"{input_args[1]}\n");
        }

        else
        {
            new Error($"{input_args[0]}: No such file or directory.");
            Console.WriteLine("Use `touch` command to create a file.");
        }
    }

    public static void read(string[] input_args)
    {
        input_args[0] = Obsidian.Shell.Strings(input_args[0]);
        var parser = new argparse("read ~> Displays the contents of a text file.");
        parser.AddArgument("--info", "Shows information about a specific line.", default_value: "-1");
        parser.AddArgument("-i", "Shows information about a specific line.", default_value: "-1");
        var args = parser.Parse(input_args);

        string filename = parser.free_args.FirstOrDefault();
        if (File.Exists(filename))
        {
            Console.OutputEncoding = Encoding.UTF8;
            if (Convert.ToInt64(args["-i"]) <= 0 && Convert.ToInt64(args["--info"]) <= 0)
            {
                var Color = Console.ForegroundColor;
                string[] lines = File.ReadAllLines(filename);
                Encoding encoding = Encoding.Unicode; // Get the Unicode encoding of the line
                FileInfo fileInfo = new FileInfo(filename); // Create a FileInfo object for a text file

                for (int i = 0; i < lines.Length; i++)
                {
                    encoding = Encoding.Unicode; // Get the Unicode encoding of the line
                    fileInfo = new FileInfo(filename); // Create a FileInfo object for a text file

                    // Print the line.
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"{i+1}. ");
                    Console.ForegroundColor = Color;

                    Console.WriteLine($"{lines[i]}");
                }

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(String.Concat(new String('\u2501', 50)));
                Console.ForegroundColor = Color;

                // Print the properties of the file
                long char_count = fileInfo.Length;
                DateTime creation_time = fileInfo.CreationTime;

                Console.WriteLine($"File: {fileInfo.Name}");
                Console.WriteLine($"Characters: {char_count}");
                Console.WriteLine($"Creation Time: {creation_time}");
                Console.WriteLine($"Encoding: {encoding}");
            }

            else
            {
                var Color = Console.ForegroundColor;
                Encoding encoding = Encoding.Unicode; // Get the Unicode encoding of the line
                FileInfo fileInfo = new FileInfo(filename); // Create a FileInfo object for a text file

                long line_no = args["-i"] != "-1" ? Convert.ToInt64(args["-i"]) : Convert.ToInt64(args["--info"]);
                string[] lines = File.ReadAllLines(filename);

                // Print the line.
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"{line_no}. ");
                Console.ForegroundColor = Color;

                Console.WriteLine(lines[line_no-1]);

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine(String.Concat(new String('\u2501', 50)));
                Console.ForegroundColor = Color;

                // Print the properties of the file
                long char_count = fileInfo.Length;

                Console.WriteLine($"File: {fileInfo.Name}");
                Console.WriteLine($"Characters: {char_count}");
                Console.WriteLine($"Encoding: {encoding}");
            }
        }

        else
        {
            new Error($"{filename}: No such file or directory.");
            Console.WriteLine("Please make sure the file exists or the given file is not actually a folder.");
        }
    }

    public static void backup()
    {
        Console.WriteLine("Restoring.");
        Console.Write("Using 'SoftwareDistribution\\RestorePoint' to restore.");
        if (Directory.Exists($"{Obsidian.rDir}\\SoftwareDistribution") && File.Exists($"{Obsidian.rDir}\\Sysfail\\rp\\safe.exe"))
        {
            string NoteCurrentTime = DateTime.Now.ToString("[dd-MM-yyyy], [HH:mm:ss]");

            Obsidian.Shell.CommandPrompt($"call \"{Obsidian.rDir}\\Sysfail\\rp\\safe.exe\" -p \"{Obsidian.rDir}\"");
            Console.WriteLine("Created a restore successful.");
        }

        else
        {
            string NoteCurrentTime = DateTime.Now.ToString("[dd-MM-yyyy], [HH:mm:ss]");
            File.AppendAllText($"{Obsidian.rDir}\\Files.x72\\root\\tmp\\Crashreport.log", $"{NoteCurrentTime}, RESTORE POINT DIRECTORY is missing or corrupted.\n");

            new Error("\n" + "Cannot create a restore point." + "\n" + "RESTORE POINT DIRECTORY is missing or corrupted.");
        }
    }

    public static void lockPC(string[] input_args)
    {
        string[] LOCKHelp = {
            "Locks the System at Start-up.",
            "Usage: lock [args][password]",
            "",
            "Arguments:",
            "-rm   -> Remove current password."
        };

        if (Collection.Array.IsEmpty(input_args))
        {
            Console.Write("Set Password: ");
            string Password = Console.ReadLine();

            File.WriteAllText($"{Obsidian.rDir}\\Files.x72\\root\\User.set", Password);
            Console.WriteLine("Your password was set successfully.");
        }

        else if (Obsidian.Shell.IsAskingForHelp(input_args.FirstOrDefault()) && input_args.Length == 1) Console.WriteLine(string.Join("\n", LOCKHelp));
        else if ((input_args.FirstOrDefault() == "remove" || input_args.FirstOrDefault() == "-rm") && input_args.Length == 1)
        {
            if (File.Exists($"{Obsidian.rDir}\\Files.x72\\root\\User.set"))
            {
                File.Delete($"{Obsidian.rDir}\\Files.x72\\root\\User.set");
                Console.WriteLine("Password removed successfully.");
            }

            Console.WriteLine("Your system isn't password protected.");
        }

        else if (input_args.Length > 1) Error.TooManyArgs(input_args);
        else if (input_args.Length < 1) Error.TooFewArgs(input_args);
        else
        {
            input_args[0] = Obsidian.Shell.Strings(input_args[0]);
            File.WriteAllText($"{Obsidian.rDir}\\Files.x72\\root\\User.set", input_args[0]);
            Console.WriteLine("Your password was set successfully.");
        }
    }

    public static void terminate(string[] input_args)
    {
        if (Collection.Array.IsEmpty(input_args)) Obsidian.Shell.CommandPrompt("tasklist");
        else
        {
            for (int i = 0; i < input_args.Length; i++) input_args[i] = Obsidian.Shell.Strings(input_args[i]);
            Obsidian.Shell.CommandPrompt($"taskkill /f /im {string.Join(" ", input_args)}");
        }
    }

    public static void reset(string[] input_args)
    {
        if (Collection.Array.IsEmpty(input_args))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Resetting this PC will delete all your files, settings and apps.");
            Console.ResetColor();

            Console.WriteLine("Are you sure? Y/N");
            Console.Write("> ");
            ConsoleKeyInfo KeyInput = Console.ReadKey();
            string GetKey = KeyInput.Key.ToString();
            Console.WriteLine();
            if (GetKey.ToLower() == "y")
            {
                try
                {
                    string[] EntrySourcePath = { $"{Obsidian.rDir}\\Files.x72", $"{Obsidian.rDir}\\SoftwareDistribution" };

                    Console.WriteLine("Formatting.");
                    foreach (string Source in EntrySourcePath)
                    {
                        string[] Entries = Directory.GetFileSystemEntries(Source, "*", SearchOption.AllDirectories);
                        foreach (string Entry in Entries)
                        {
                            Console.WriteLine($"Locating {Entry}");
                            if (Directory.Exists(Entry))
                            {
                                Console.WriteLine($"Deleting {Entry}");
                                Directory.Delete(Entry, true);

                                Console.WriteLine("Verifying.");
                                if (!Directory.Exists(Entry)) Console.WriteLine($"Deleted {Entry}");
                            }

                            else if (File.Exists(Entry))
                            {
                                Console.WriteLine($"Deleting {Entry}");
                                File.Delete(Entry);

                                Console.WriteLine("Verifying.");
                                if (!File.Exists(Entry)) Console.WriteLine($"Deleted {Entry}");
                            }
                        }
                    }

                    Console.WriteLine();
                    Obsidian.Shell.SYSRestore();
                    Console.WriteLine("System Reset Completed.");
                    Console.WriteLine("Restarting in 3 seconds.");

                    Obsidian.Shell.Track(1000, 3);

                    Obsidian.Shell.CommandPrompt($"\"{Process.GetCurrentProcess().MainModule.FileName}\"");
                    Environment.Exit(0);
                }

                catch (Exception)
                {
                    Console.WriteLine("Cannot perform a System Reset.");
                }
            }

            else if (GetKey.ToLower() == "n") Console.WriteLine("System Reset Cancelled!");
            else
            {
                Console.WriteLine("Invalid Key Input.");
                Console.WriteLine("Cannot perform a System Reset.");
            }
        }

        else Error.Args(input_args);
    }

    public static void winrar(string[] input_args)
    {
        string winrarPath = string.Empty;
        bool unzip = false;

        // Find the index of the item to remove,
        // if the item was found, remove it from the list
        List<string> list = input_args.ToList();
        int index = list.IndexOf("-u");
        if (index != -1)
        {
            unzip = true;
            list.RemoveAt(index);
            input_args = list.ToArray();
        }

        RegistryKey winrarKey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe");
        if (winrarKey != null)
        {
            winrarPath = winrarKey.GetValue(string.Empty).ToString();
            winrarKey.Close();
        }

        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        startInfo.FileName = winrarPath;

        string sourcePath = input_args[0];
        string destinationPath = input_args[1];
        if (unzip)
        {
            startInfo.Arguments = $"x -y \"{sourcePath}\" \"{destinationPath}\"";
            if (!Directory.Exists(destinationPath))
                Directory.CreateDirectory(destinationPath);
        }

        else
            startInfo.Arguments = $"a -ep \"{destinationPath}\" \"{sourcePath}\"";

        process.StartInfo = startInfo;
        process.Start();
    }

    public static void PlayonYT(string[] input_args)
    {
        bool doOpen = false;

        // Find the index of the item to remove,
        // if the item was found, remove it from the list
        List<string> list = input_args.ToList();
        int index = list.IndexOf("-m");
        if (index != -1)
        {
            doOpen = true;
            list.RemoveAt(index);
            input_args = list.ToArray();
        }

        string query = Lexer.SimplifyString(input_args[0]);

        if (doOpen)
            Obsidian.Shell.CommandPrompt($"call \"{Obsidian.rDir}\\Files.x72\\root\\ext\\ply.exe\" --play \"{query}\"");

        else
            Obsidian.Shell.CommandPrompt($"call \"{Obsidian.rDir}\\Files.x72\\root\\ext\\ply.exe\" --search \"{query}\"");
    }

    public static void SearchonWiki(string[] input_args)
    {
        bool doOpen = false;

        // Find the index of the item to remove,
        // if the item was found, remove it from the list
        List<string> list = input_args.ToList();
        int index = list.IndexOf("-m");
        if (index != -1)
        {
            doOpen = true;
            list.RemoveAt(index);
            input_args = list.ToArray();
        }

        string query = Lexer.SimplifyString(input_args[0]);

        if (doOpen)
            Obsidian.Shell.CommandPrompt($"call \"{Obsidian.rDir}\\Files.x72\\root\\ext\\wiki.exe\" --show \"{query}\"");

        else
            Obsidian.Shell.CommandPrompt($"call \"{Obsidian.rDir}\\Files.x72\\root\\ext\\wiki.exe\" --search \"{query}\"");
    }

    public static void SearchonGoogle(string[] input_args)
    {
        // Find the index of the item to remove,
        // if the item was found, remove it from the list
        List<string> list = input_args.ToList();
        string engineName = string.Empty;
        int index = list.IndexOf("-m");
        var engines = new Dictionary<string, string>()
        {
            { "bing", "https://www.bing.com/search?q=" },
            { "google", "https://www.google.com/search?q=" },
            { "duckduckgo", "https://duckduckgo.com/?q=" }
        };

        if (index != -1)
        {
            engineName = index < input_args.Length - 1 ? input_args[index + 1].ToString() : "google";

            list.RemoveAt(index + 1);
            list.RemoveAt(index);
            input_args = list.ToArray();
        }

        string query = Lexer.SimplifyString(input_args.FirstOrDefault()).Replace(" ", "+");
        if (Collection.String.IsEmpty(query))
        {
            new Error("Please provide a search query.");
            return;
        }

        else
        {
            string engine = string.Empty;
            if (engines.ContainsKey(engineName))
                engine = engines[engineName];

            else
            {
                Console.WriteLine($"Search engine '{engineName}' not supported.");
                return;
            }

            Obsidian.Shell.CommandPrompt($"start {engine}{query}");
        }
    }
}
