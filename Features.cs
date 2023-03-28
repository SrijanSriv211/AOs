using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Net.Http;
using Microsoft.Win32;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;

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
                "vol         -> Set the system master volume to a specific level.",
                "switch      -> Switch to an application by using its window title.",
                "cosine      -> Calculate the cosine similarity between two sentences.",
                "help        -> Displays a list of all overload commands."
            };

        if (Collection.Array.IsEmpty(input_args)) Console.WriteLine(string.Join("\n", PromptHelpCenter));
        else if (Obsidian.Shell.IsAskingForHelp(input_args.FirstOrDefault())) Console.WriteLine(string.Join("\n", PromptHelpCenter));
        else if (input_args.FirstOrDefault().ToLower() == "itsmagic") Obsidian.Shell.StartApp("https://youtu.be/dQw4w9WgXcQ"); // Rickroll!!!
        else if (input_args.FirstOrDefault().ToLower() == "studybyte") Obsidian.Shell.StartApp("https://light-lens.github.io/Studybyte");
        else if (input_args.FirstOrDefault().ToLower() == "deeplock") Obsidian.Shell.StartApp(@"C:\WINDOWS\system32\rundll32.exe", "user32.dll,LockWorkStation");
        else if (input_args.FirstOrDefault().ToLower() == "deepscan") Obsidian.Shell.StartApp($"{Obsidian.rDir}\\Sysfail\\rp\\FixCorruptedSystemFiles.bat", AppAdmin: true);
        else if (input_args.FirstOrDefault().ToLower() == "cosine")
        {
            string[] cosine_args = Collection.Array.Trim(input_args.Skip(1).ToArray());
            if (cosine_args.Length < 2)
                Error.TooFewArgs(cosine_args);

            else if (cosine_args.Length > 2)
                Error.TooManyArgs(cosine_args);

            else
                Console.WriteLine(TextSimilarity.CosineSimilarity(cosine_args[0], cosine_args[1]));
        }

        else if (input_args.FirstOrDefault().ToLower() == "switch")
        {
            string appname = Collection.Array.Trim(input_args.Skip(1).ToArray()).FirstOrDefault();
            Process[] processes = Process.GetProcessesByName(appname);
            if (processes.Length > 0)
            {
                Process process = processes[0];
                WindowManager.SetForegroundWindow(process.MainWindowHandle);
            }
        }

        else if (input_args.FirstOrDefault().ToLower() == "vol")
        {
            string[] volume_args = Collection.Array.Trim(input_args.Skip(1).ToArray());

            var parser = new argparse("@vol -> Set the system master volume to a specific level.");
            parser.AddArgument("-v", "Increase/Decrease the volume by 2 levels.", default_value: "");
            var args = parser.Parse(volume_args);

            if (Collection.String.IsEmpty(args["-v"]) && !(Convert.ToInt32(parser.free_args.FirstOrDefault()) < 0 || Convert.ToInt32(parser.free_args.FirstOrDefault()) > 100))
                VolumeChanger.AudioManager.SetMasterVolume(Convert.ToInt32(parser.free_args.FirstOrDefault()));

            else
            {
                if (Convert.ToInt32(args["-v"]) == 0)
                    VolumeChanger.AudioManager.ToggleMasterVolumeMute();

                else if (!(Convert.ToInt32(args["-v"]) < -100 || Convert.ToInt32(args["-v"]) > 100))
                    VolumeChanger.AudioManager.StepMasterVolume(Convert.ToInt32(args["-v"]));
            }
        }

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
            foreach (string Entry in Entries)
                Console.WriteLine(Entry);
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
                    foreach (string Entry in Entries)
                        Console.WriteLine(Entry);

                    break;
                }

                else if (i.ToLower() == "-f" || i.ToLower() == "--files")
                {
                    string[] Files = Directory.GetFiles(".");
                    foreach (string File in Files)
                        Console.WriteLine(File);
                }

                else if (i.ToLower() == "-d" || i.ToLower() == "--folder" || i.ToLower() == "--directories")
                {
                    string[] Directories = Directory.GetDirectories(".");
                    foreach (string Folder in Directories)
                        Console.WriteLine(Folder);
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
        var parser = new argparse("commit -> Edits the contents of a text file.");
        parser.AddArgument("--line", "Edits a specific line in a text file.", default_value: "-1");
        parser.AddArgument("-l", "Edits a specific line in a text file.", default_value: "-1");
        var args = parser.Parse(input_args);

        if (parser.free_args.Length < 2)
        {
            new Error("Filename/Text to be written not specified.");
            return;
        }

        input_args = Lexer.SimplifyString(input_args);
        string content = Lexer.SimplifyString(parser.free_args[1]);
        string filename = parser.free_args.FirstOrDefault();

        if (File.Exists(filename))
        {
            var Color = Console.ForegroundColor;
            if (Convert.ToInt64(args["-l"]) == 0 || Convert.ToInt64(args["--line"]) == 0)
            {
                new Error("Overwrite the entire file; are you sure? Y/N");
                Console.Write("> ");
                ConsoleKeyInfo KeyInput = Console.ReadKey();
                string GetKey = KeyInput.Key.ToString();
                Console.WriteLine();

                if (GetKey.ToLower() == "y")
                    File.WriteAllText(filename, content);

                else
                    return;
            }

            else if (Convert.ToInt64(args["-l"]) < 0 && Convert.ToInt64(args["--line"]) < 0)
            {
                if (File.ReadLines(filename).FirstOrDefault() == "__{time-date}__")
                {
                    File.AppendAllText(filename, $"{input_args[1]}\n");
                    File.AppendAllText(filename, $"{DateTime.Now.ToString("[dd-MM-yyyy, HH:mm:ss]")}\n");
                }

                else
                    File.AppendAllText(filename, $"{input_args[1]}\n");
            }

            else
            {
                long line_no = args["-l"] != "-1" ? Convert.ToInt64(args["-l"]) : Convert.ToInt64(args["--line"]);
                string[] lines = File.ReadAllLines(filename);

                // Check if the line number is valid
                if (line_no > 0 && line_no <= lines.Length)
                {
                    // Overwrite the specified line with the new text
                    lines[line_no - 1] = content;

                    // Write the modified array back to the file
                    File.WriteAllLines(filename, lines);
                }
            }
        }

        else
        {
            new Error($"{filename}: No such file or directory.");
            Console.WriteLine("Use `touch` command to create a file.");
        }
    }

    public static void read(string[] input_args)
    {
        input_args[0] = Obsidian.Shell.Strings(input_args[0]);
        var parser = new argparse("read -> Displays the contents of a text file.");
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
            new Error($"{filename}: No such file or directory.");
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
            Process[] processes;
            foreach (string i in input_args)
            {
                string appname = Lexer.SimplifyString(i);
                if (Collection.String.IsEmpty(appname))
                {
                    uint process_id;
                    IntPtr handle = WindowManager.GetForegroundWindow();
                    WindowManager.GetWindowThreadProcessId(handle, out process_id);
                    processes = new[] { Process.GetProcessById((int)process_id) };
                }

                else
                    processes = Process.GetProcessesByName(appname);

                // Close the first instance of the process
                if (processes.Length > 0)
                {
                    Process app_process = processes.FirstOrDefault();
                    if (!app_process.CloseMainWindow())
                        app_process.Kill();
                }
            }
        }
    }

    public static void reset(string[] input_args)
    {
        if (Collection.Array.IsEmpty(input_args))
        {
            new Error("Resetting this PC will delete all your files, settings and apps.");

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

        var parser = new argparse("zip -> Compresses or Decompresses files or folders.");
        parser.AddArgument("-u", "Decompresses zip files.");
        var args = parser.Parse(input_args);

        unzip = Convert.ToBoolean(args["-u"]);

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

        string sourcePath = parser.free_args.FirstOrDefault();
        string destinationPath = parser.free_args[1];
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

        var parser = new argparse("ply -> Search for a video on youtube based on a query.");
        parser.AddArgument("-m", "Plays the first video from the YouTube search results list.");
        var args = parser.Parse(input_args);

        doOpen = Convert.ToBoolean(args["-m"]);
        string query = Lexer.SimplifyString(parser.free_args.FirstOrDefault());

        if (doOpen)
            Obsidian.Shell.CommandPrompt($"call \"{Obsidian.rDir}\\Files.x72\\root\\ext\\ply.exe\" --play \"{query}\"");

        else
            Obsidian.Shell.CommandPrompt($"call \"{Obsidian.rDir}\\Files.x72\\root\\ext\\ply.exe\" --search \"{query}\"");
    }

    public static void SearchonWiki(string[] input_args)
    {
        bool doOpen = false;

        var parser = new argparse("wiki -> Search for information on wikipedia.");
        parser.AddArgument("-m", "Get the information directly in the terminal.");
        var args = parser.Parse(input_args);

        doOpen = Convert.ToBoolean(args["-m"]);
        string query = Lexer.SimplifyString(parser.free_args.FirstOrDefault());

        if (doOpen)
            Obsidian.Shell.CommandPrompt($"call \"{Obsidian.rDir}\\Files.x72\\root\\ext\\wiki.exe\" --show \"{query}\"");

        else
            Obsidian.Shell.CommandPrompt($"call \"{Obsidian.rDir}\\Files.x72\\root\\ext\\wiki.exe\" --search \"{query}\"");
    }

    public static void SearchonGoogle(string[] input_args)
    {
        var parser = new argparse("srh -> Search for a specific query on the internet.");
        parser.AddArgument("-m", "Search the query on a specific search engine.", default_value: "google");
        var args = parser.Parse(input_args);

        string engineName = args["-m"];
        var engines = new Dictionary<string, string>()
        {
            { "bing", "https://www.bing.com/search?q=" },
            { "google", "https://www.google.com/search?q=" },
            { "duckduckgo", "https://duckduckgo.com/?q=" }
        };

        string query = Lexer.SimplifyString(parser.free_args.FirstOrDefault()).Replace(" ", "+");
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

    public static void weather(string[] input_args)
    {
        string City = string.Empty;
        if (Collection.Array.IsEmpty(input_args))
            City = "muzaffarpur";

        else
            City = input_args.FirstOrDefault();

        Console.WriteLine($"Fetching today's weather report for: {City}");


        // Fetch weather details.
        string URL = $"https://wttr.in/{City}?format=%C";
        HttpClient client = new HttpClient();
        HttpResponseMessage response = client.GetAsync(URL).Result;
        string res = response.Content.ReadAsStringAsync().Result;

        Console.WriteLine(res);
    }

    public static void temperature(string[] input_args)
    {
        string City = string.Empty;
        if (Collection.Array.IsEmpty(input_args))
            City = "muzaffarpur";

        else
            City = input_args.FirstOrDefault();

        Console.WriteLine($"Fetching today's temperature report in: {City}");

        // Fetch temperature details.
        string URL = $"https://wttr.in/{City}?format=%t";
        HttpClient client = new HttpClient();
        HttpResponseMessage response = client.GetAsync(URL).Result;
        string res = response.Content.ReadAsStringAsync().Result;
        string Temp = (res[0] == '+') ? res.Substring(1) : res;

        Console.WriteLine(Temp);
    }
}

class WindowManager
{
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetForegroundWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    public static extern IntPtr GetForegroundWindow();

    [DllImport("user32.dll")]
    public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
}

// https://gist.github.com/sverrirs/d099b34b7f72bb4fb386
namespace VolumeChanger
{
    /// <summary>
    /// Controls audio using the Windows CoreAudio API
    /// from: http://stackoverflow.com/questions/14306048/controling-volume-mixer
    /// and: http://netcoreaudio.codeplex.com/
    /// </summary>
    public static class AudioManager
    {
        #region Master Volume Manipulation

        /// <summary>
        /// Gets the current master volume in scalar values (percentage)
        /// </summary>
        /// <returns>-1 in case of an error, if successful the value will be between 0 and 100</returns>
        public static float GetMasterVolume()
        {
            IAudioEndpointVolume masterVol = null;
            try
            {
                masterVol = GetMasterVolumeObject();
                if (masterVol == null)
                    return -1;

                float volumeLevel;
                masterVol.GetMasterVolumeLevelScalar(out volumeLevel);
                return volumeLevel*100;
            }
            finally
            {
                if (masterVol != null)
                    Marshal.ReleaseComObject(masterVol);
            }
        }

        /// <summary>
        /// Sets the master volume to a specific level
        /// </summary>
        /// <param name="newLevel">Value between 0 and 100 indicating the desired scalar value of the volume</param>
        public static void SetMasterVolume(float newLevel)
        {
            IAudioEndpointVolume masterVol = null;
            try
            {
                masterVol = GetMasterVolumeObject();
                if (masterVol == null)
                    return;

                masterVol.SetMasterVolumeLevelScalar(newLevel/100, Guid.Empty);
            }
            finally
            {
                if (masterVol != null)
                    Marshal.ReleaseComObject(masterVol);
            }
        }

        /// <summary>
        /// Increments or decrements the current volume level by the <see cref="stepAmount"/>.
        /// </summary>
        /// <param name="stepAmount">Value between -100 and 100 indicating the desired step amount. Use negative numbers to decrease
        /// the volume and positive numbers to increase it.</param>
        /// <returns>the new volume level assigned</returns>
        public static float StepMasterVolume(float stepAmount)
        {
            IAudioEndpointVolume masterVol = null;
            try
            {
                masterVol = GetMasterVolumeObject();
                if (masterVol == null)
                    return -1;

                float stepAmountScaled = stepAmount/100;

                // Get the level
                float volumeLevel;
                masterVol.GetMasterVolumeLevelScalar(out volumeLevel);

                // Calculate the new level
                float newLevel = volumeLevel + stepAmountScaled;
                newLevel = Math.Min(1, newLevel);
                newLevel = Math.Max(0, newLevel);

                masterVol.SetMasterVolumeLevelScalar(newLevel, Guid.Empty);

                // Return the new volume level that was set
                return newLevel*100;
            }
            finally
            {
                if (masterVol != null)
                    Marshal.ReleaseComObject(masterVol);
            }
        }

        /// <summary>
        /// Switches between the master volume mute states depending on the current state
        /// </summary>
        /// <returns>the current mute state, true if the volume was muted, false if unmuted</returns>
        public static bool ToggleMasterVolumeMute()
        {
            IAudioEndpointVolume masterVol = null;
            try
            {
                masterVol = GetMasterVolumeObject();
                if (masterVol == null)
                    return false;

                bool isMuted;
                masterVol.GetMute(out isMuted);
                masterVol.SetMute(!isMuted, Guid.Empty);

                return !isMuted;
            }
            finally
            {
                if (masterVol != null)
                    Marshal.ReleaseComObject(masterVol);
            }
        }

        private static IAudioEndpointVolume GetMasterVolumeObject()
        {
            IMMDeviceEnumerator deviceEnumerator = null;
            IMMDevice speakers = null;
            try
            {
                deviceEnumerator = (IMMDeviceEnumerator) (new MMDeviceEnumerator());
                deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia, out speakers);

                Guid IID_IAudioEndpointVolume = typeof (IAudioEndpointVolume).GUID;
                object o;
                speakers.Activate(ref IID_IAudioEndpointVolume, 0, IntPtr.Zero, out o);
                IAudioEndpointVolume masterVol = (IAudioEndpointVolume) o;

                return masterVol;
            }
            finally
            {
                if (speakers != null) Marshal.ReleaseComObject(speakers);
                if (deviceEnumerator != null) Marshal.ReleaseComObject(deviceEnumerator);
            }
        }

        #endregion
    }

    #region Abstracted COM interfaces from Windows CoreAudio API

    [ComImport]
    [Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
    internal class MMDeviceEnumerator
    {
    }

    internal enum EDataFlow
    {
        eRender,
        eCapture,
        eAll,
        EDataFlow_enum_count
    }

    internal enum ERole
    {
        eConsole,
        eMultimedia,
        eCommunications,
        ERole_enum_count
    }

    [Guid("A95664D2-9614-4F35-A746-DE8DB63617E6"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IMMDeviceEnumerator
    {
        int NotImpl1();

        [PreserveSig]
        int GetDefaultAudioEndpoint(EDataFlow dataFlow, ERole role, out IMMDevice ppDevice);

        // the rest is not implemented
    }

    [Guid("D666063F-1587-4E43-81F1-B948E807363F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IMMDevice
    {
        [PreserveSig]
        int Activate(ref Guid iid, int dwClsCtx, IntPtr pActivationParams, [MarshalAs(UnmanagedType.IUnknown)] out object ppInterface);

        // the rest is not implemented
    }

    [Guid("77AA99A0-1BD6-484F-8BC7-2C654C9A9B6F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IAudioSessionManager2
    {
        int NotImpl1();
        int NotImpl2();

        [PreserveSig]
        int GetSessionEnumerator(out IAudioSessionEnumerator SessionEnum);

        // the rest is not implemented
    }

    [Guid("E2F5BB11-0570-40CA-ACDD-3AA01277DEE8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IAudioSessionEnumerator
    {
        [PreserveSig]
        int GetCount(out int SessionCount);

        [PreserveSig]
        int GetSession(int SessionCount, out IAudioSessionControl2 Session);
    }

    [Guid("87CE5498-68D6-44E5-9215-6DA47EF883D8"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface ISimpleAudioVolume
    {
        [PreserveSig]
        int SetMasterVolume(float fLevel, ref Guid EventContext);

        [PreserveSig]
        int GetMasterVolume(out float pfLevel);

        [PreserveSig]
        int SetMute(bool bMute, ref Guid EventContext);

        [PreserveSig]
        int GetMute(out bool pbMute);
    }

    [Guid("bfb7ff88-7239-4fc9-8fa2-07c950be9c6d"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    internal interface IAudioSessionControl2
    {
        // IAudioSessionControl
        [PreserveSig]
        int NotImpl0();

        [PreserveSig]
        int GetDisplayName([MarshalAs(UnmanagedType.LPWStr)] out string pRetVal);

        [PreserveSig]
        int SetDisplayName([MarshalAs(UnmanagedType.LPWStr)]string Value, [MarshalAs(UnmanagedType.LPStruct)] Guid EventContext);

        [PreserveSig]
        int GetIconPath([MarshalAs(UnmanagedType.LPWStr)] out string pRetVal);

        [PreserveSig]
        int SetIconPath([MarshalAs(UnmanagedType.LPWStr)] string Value, [MarshalAs(UnmanagedType.LPStruct)] Guid EventContext);

        [PreserveSig]
        int GetGroupingParam(out Guid pRetVal);

        [PreserveSig]
        int SetGroupingParam([MarshalAs(UnmanagedType.LPStruct)] Guid Override, [MarshalAs(UnmanagedType.LPStruct)] Guid EventContext);

        [PreserveSig]
        int NotImpl1();

        [PreserveSig]
        int NotImpl2();

        // IAudioSessionControl2
        [PreserveSig]
        int GetSessionIdentifier([MarshalAs(UnmanagedType.LPWStr)] out string pRetVal);

        [PreserveSig]
        int GetSessionInstanceIdentifier([MarshalAs(UnmanagedType.LPWStr)] out string pRetVal);

        [PreserveSig]
        int GetProcessId(out int pRetVal);

        [PreserveSig]
        int IsSystemSoundsSession();

        [PreserveSig]
        int SetDuckingPreference(bool optOut);
    }

    // http://netcoreaudio.codeplex.com/SourceControl/latest#trunk/Code/CoreAudio/Interfaces/IAudioEndpointVolume.cs
    [Guid("5CDF2C82-841E-4546-9722-0CF74078229A"),InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IAudioEndpointVolume
    {
        [PreserveSig]
        int NotImpl1();

        [PreserveSig]
        int NotImpl2();

        /// <summary>
        /// Gets a count of the channels in the audio stream.
        /// </summary>
        /// <param name="channelCount">The number of channels.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        int GetChannelCount(
            [Out] [MarshalAs(UnmanagedType.U4)] out UInt32 channelCount);

        /// <summary>
        /// Sets the master volume level of the audio stream, in decibels.
        /// </summary>
        /// <param name="level">The new master volume level in decibels.</param>
        /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        int SetMasterVolumeLevel(
            [In] [MarshalAs(UnmanagedType.R4)] float level,
            [In] [MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

        /// <summary>
        /// Sets the master volume level, expressed as a normalized, audio-tapered value.
        /// </summary>
        /// <param name="level">The new master volume level expressed as a normalized value between 0.0 and 1.0.</param>
        /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        int SetMasterVolumeLevelScalar(
            [In] [MarshalAs(UnmanagedType.R4)] float level,
            [In] [MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

        /// <summary>
        /// Gets the master volume level of the audio stream, in decibels.
        /// </summary>
        /// <param name="level">The volume level in decibels.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        int GetMasterVolumeLevel(
            [Out] [MarshalAs(UnmanagedType.R4)] out float level);

        /// <summary>
        /// Gets the master volume level, expressed as a normalized, audio-tapered value.
        /// </summary>
        /// <param name="level">The volume level expressed as a normalized value between 0.0 and 1.0.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        int GetMasterVolumeLevelScalar(
            [Out] [MarshalAs(UnmanagedType.R4)] out float level);

        /// <summary>
        /// Sets the volume level, in decibels, of the specified channel of the audio stream.
        /// </summary>
        /// <param name="channelNumber">The channel number.</param>
        /// <param name="level">The new volume level in decibels.</param>
        /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        int SetChannelVolumeLevel(
            [In] [MarshalAs(UnmanagedType.U4)] UInt32 channelNumber,
            [In] [MarshalAs(UnmanagedType.R4)] float level,
            [In] [MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

        /// <summary>
        /// Sets the normalized, audio-tapered volume level of the specified channel in the audio stream.
        /// </summary>
        /// <param name="channelNumber">The channel number.</param>
        /// <param name="level">The new master volume level expressed as a normalized value between 0.0 and 1.0.</param>
        /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        int SetChannelVolumeLevelScalar(
            [In] [MarshalAs(UnmanagedType.U4)] UInt32 channelNumber,
            [In] [MarshalAs(UnmanagedType.R4)] float level,
            [In] [MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

        /// <summary>
        /// Gets the volume level, in decibels, of the specified channel in the audio stream.
        /// </summary>
        /// <param name="channelNumber">The zero-based channel number.</param>
		/// <param name="level">The volume level in decibels.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        int GetChannelVolumeLevel(
            [In] [MarshalAs(UnmanagedType.U4)] UInt32 channelNumber,
            [Out] [MarshalAs(UnmanagedType.R4)] out float level);

        /// <summary>
        /// Gets the normalized, audio-tapered volume level of the specified channel of the audio stream.
        /// </summary>
        /// <param name="channelNumber">The zero-based channel number.</param>
		/// <param name="level">The volume level expressed as a normalized value between 0.0 and 1.0.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        int GetChannelVolumeLevelScalar(
            [In] [MarshalAs(UnmanagedType.U4)] UInt32 channelNumber,
            [Out] [MarshalAs(UnmanagedType.R4)] out float level);

        /// <summary>
        /// Sets the muting state of the audio stream.
        /// </summary>
        /// <param name="isMuted">True to mute the stream, or false to unmute the stream.</param>
        /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        int SetMute(
            [In] [MarshalAs(UnmanagedType.Bool)] Boolean isMuted,
            [In] [MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

        /// <summary>
        /// Gets the muting state of the audio stream.
        /// </summary>
        /// <param name="isMuted">The muting state. True if the stream is muted, false otherwise.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        int GetMute(
            [Out] [MarshalAs(UnmanagedType.Bool)] out Boolean isMuted);

        /// <summary>
        /// Gets information about the current step in the volume range.
        /// </summary>
        /// <param name="step">The current zero-based step index.</param>
        /// <param name="stepCount">The total number of steps in the volume range.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        int GetVolumeStepInfo(
            [Out] [MarshalAs(UnmanagedType.U4)] out UInt32 step,
            [Out] [MarshalAs(UnmanagedType.U4)] out UInt32 stepCount);

        /// <summary>
        /// Increases the volume level by one step.
        /// </summary>
        /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        int VolumeStepUp(
            [In] [MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

        /// <summary>
        /// Decreases the volume level by one step.
        /// </summary>
        /// <param name="eventContext">A user context value that is passed to the notification callback.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        int VolumeStepDown(
            [In] [MarshalAs(UnmanagedType.LPStruct)] Guid eventContext);

        /// <summary>
        /// Queries the audio endpoint device for its hardware-supported functions.
        /// </summary>
        /// <param name="hardwareSupportMask">A hardware support mask that indicates the capabilities of the endpoint.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        int QueryHardwareSupport(
            [Out] [MarshalAs(UnmanagedType.U4)] out UInt32 hardwareSupportMask);

        /// <summary>
        /// Gets the volume range of the audio stream, in decibels.
        /// </summary>
		/// <param name="volumeMin">The minimum volume level in decibels.</param>
		/// <param name="volumeMax">The maximum volume level in decibels.</param>
		/// <param name="volumeStep">The volume increment level in decibels.</param>
        /// <returns>An HRESULT code indicating whether the operation passed of failed.</returns>
        [PreserveSig]
        int GetVolumeRange(
            [Out] [MarshalAs(UnmanagedType.R4)] out float volumeMin,
            [Out] [MarshalAs(UnmanagedType.R4)] out float volumeMax,
            [Out] [MarshalAs(UnmanagedType.R4)] out float volumeStep);
    }

    #endregion
}
