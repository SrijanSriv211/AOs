using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

public class Obsidian
{
    public string Title = "AOs", Prompt = "$ ";
    public string Version = String.Format("AOs 2023 [Version {0}]", vNum);

    public static string vNum = "2.3";
    private bool AOsVerPrompt = true;
    private string CMD;

    public Obsidian(string title = "AOs", string prompt = "$ ")
    {
        Title = title;
        Prompt = prompt;
    }

    public (string cmd, string[] args) TakeInput(string input = "")
    {
        if (!Collection.String.IsEmpty(input.Trim())) CMD = input.Trim();
        else
        {
            if (AOsVerPrompt)
            {
                ClearConsole();
                AOsVerPrompt = false;
                Console.Write(Prompt);
            }

            else
            {
                if (!Collection.String.IsEmpty(CMD)) Console.Write($"\n{Prompt}");
                else Console.Write(Prompt);
            }

            CMD = Console.ReadLine() ?? "";
            CMD = CMD.Trim() ?? "";
        }

        if (!Collection.String.IsEmpty(CMD))
        {
            History.SetHistory(CMD); // Set history.
            if (CMD[0] == '_') CMD = CMD.Substring(1).Trim();
        }

        string[] ListOfToks = new Lexer(CMD.Trim()).Tokens;

        // Split the ListOfToks into a cmd and Args variable and array respectively.
        string input_cmd = Collection.Array.Trim(ListOfToks).FirstOrDefault() ?? "";
        string[] input_Args = Collection.Array.Trim(Collection.Array.Reduce(ListOfToks)) ?? new string[0];
        if (!Collection.Array.IsEmpty(input_Args) && input_Args.FirstOrDefault() == input_cmd) input_Args = Collection.Array.Trim(input_Args.Skip(1).ToArray());

        // Parse input.
        if (!Collection.String.IsEmpty(input_cmd))
        {
            if (input_cmd == "∞" || double.TryParse(input_cmd, out double _)) Console.WriteLine(input_cmd);
            else if (Collection.String.IsString(input_cmd))
            {
                string NewString = "";
                for (int i = 0; i < ListOfToks.Length; i++)
                {
                    if (Collection.String.IsString(ListOfToks[i]))
                        NewString += Shell.Strings(ListOfToks[i]);
                }

                Console.WriteLine(NewString);
            }

            else return (cmd: input_cmd, args: input_Args);
        }

        return (cmd: "", args: new string[0]);
    }

    public void ClearConsole()
    {
        Console.Clear();

        var Color = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(Version);
        Console.ForegroundColor = Color;
        AOsVerPrompt = true;
    }

    public void Entrypoint()
    {
        Console.Title = Title;
        Console.ResetColor();
        Console.Clear();

        Shell.RootPackages();
        Shell.AskPass();
        Shell.Scan();
    }

    public void Credits()
    {
        string[] CreditCenter = {
            "_________ Team AOS ________",
            "Author     -> Srijan Srivastava",
            "Github     -> github.com/Light-Lens/AOs",
            "Contact    -> QCoreNest@gmail.com",
            "",
            "____________________ Note (For Developers) ____________________",
            "|| Command-line utility for improved efficiency and productivity.",
            "|| All code is licensed under an MIT license.",
            "|| This allows you to re-use the code freely, remixed in both commercial and non-commercial projects.",
            "|| The only requirement is to include the same license when distributing.",
            "",
            "____________________ Note (For All) ____________________",
            "|| Warning - Do not Delete any File",
            "|| or it may Cause Corruption",
            "|| and may lead to instability.",
            "",
            "Type 'help' to get information about all supported command."
        };

        Console.WriteLine(string.Join("\n", CreditCenter));
    }

    public static dynamic rDir = AppDomain.CurrentDomain.BaseDirectory; // root dir
    public class Shell
    {
        public static void Track(int time=100, int total=100, string description="Working...")
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            DateTime start = DateTime.Now;
            var Color = Console.ForegroundColor;
            int current = 0;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (current <= total)
            {
                int percentage = (int)(((double)current / total) * 100);
                int barWidth = 40;

                // Calculate the estimated time remaining
                var elapsed = stopwatch.Elapsed;
                TimeSpan timeRemaining = TimeSpan.FromTicks(elapsed.Ticks / (current + 1) * (total - current - 1));

                // Clear the current line
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write(new string(' ', Console.WindowWidth));
                Console.SetCursorPosition(0, Console.CursorTop);

                // Draw the progress bar
                Console.Write(description + " ");
                Console.ForegroundColor = ConsoleColor.Red;

                Console.Write(new string('\u2501', percentage * barWidth / 100));

                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write(new string('\u2501', barWidth - (percentage * barWidth / 100)));

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write(" " + percentage + "%");

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write(" " + timeRemaining.ToString(@"hh\:mm\:ss"));

                Console.ForegroundColor = Color;

                current++;
                Thread.Sleep(time);
            }

            stopwatch.Stop();
        }

        public static bool SysEnvApps(string input_cmd, string[] input_Args)
        {
            // Check for apps in environment variable.
            if (File.Exists(input_cmd.ToLower()))
            {
                if (Collection.Array.IsEmpty(input_Args)) Shell.CommandPrompt(input_cmd.ToLower());
                else Shell.CommandPrompt($"\"{input_cmd.ToLower()}\" {string.Join(" ", input_Args)}");
                return true;
            }

            else if (!Collection.String.IsEmpty(Environment.GetEnvironmentVariable(input_cmd.ToLower()) ?? ""))
            {
                if (Collection.Array.IsEmpty(input_Args)) Console.WriteLine(Environment.GetEnvironmentVariable(input_cmd.ToLower()));
                else Error.Args(input_Args);
                return true;
            }

            else if (!Collection.String.IsEmpty(Shell.LocateEXE(input_cmd.ToLower())))
            {
                if (Collection.Array.IsEmpty(input_Args)) Shell.CommandPrompt($"{input_cmd.ToLower()}");
                else Shell.CommandPrompt($"\"{input_cmd.ToLower()}\" {string.Join(" ", input_Args)}");
                return true;
            }

            else if (!Collection.String.IsEmpty(Shell.LocateEXE($"{input_cmd.ToLower()}.exe")))
            {
                if (Collection.Array.IsEmpty(input_Args)) Shell.CommandPrompt($"{input_cmd.ToLower()}.exe");
                else Shell.CommandPrompt($"\"{input_cmd.ToLower()}.exe\" {string.Join(" ", input_Args)}");
                return true;
            }

            else if (!Collection.String.IsEmpty(Shell.LocateEXE($"{input_cmd.ToLower()}.bat")))
            {
                if (Collection.Array.IsEmpty(input_Args)) Shell.CommandPrompt($"{input_cmd.ToLower()}.bat");
                else Shell.CommandPrompt($"\"{input_cmd.ToLower()}.bat\" {string.Join(" ", input_Args)}");
                return true;
            }

            else if (!Collection.String.IsEmpty(Shell.LocateEXE($"{input_cmd.ToLower()}.cmd")))
            {
                if (Collection.Array.IsEmpty(input_Args)) Shell.CommandPrompt($"{input_cmd.ToLower()}.cmd");
                else Shell.CommandPrompt($"\"{input_cmd.ToLower()}.cmd\" {string.Join(" ", input_Args)}");
                return true;
            }

            else
            {
                try
                {
                    using (Process process = new Process())
                    {
                        process.StartInfo.FileName = "cmd.exe";
                        process.StartInfo.Arguments = "/C " + $"{input_cmd} {string.Join(" ", input_Args)}";
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.RedirectStandardOutput = true;
                        process.Start();

                        string output = process.StandardOutput.ReadToEnd().Trim();
                        Console.WriteLine(output);

                        return true;
                    }
                }

                catch (Exception)
                {
                    return false;
                }
            }
        }

        public static void GetHelp()
        {
            GetHelp(new string[0]);
        }

        public static void GetHelp(string[] args)
        {
            string[] HelpCenter = {
                "- prompt          ~> Changes the command prompt.",
                "backup          ~> Creates a restore point of AOs.",
                "about           ~> About AOs",
                "clear           ~> Clears the screen.",
                "history         ~> Displays the history of Commands.",
                "version         ~> Displays the AOs version.",
                "console         ~> Starts a new instance of the terminal.",
                "reload          ~> Restart AOs",
                "restart         ~> Restart the system.",
                "exit            ~> Exit AOs",
                "shutdown        ~> Shutdown the system.",
                "title           ~> Changes the title for AOs window.",
                "color           ~> Changes the default AOs foreground and background colors.",
                "time            ~> Displays current time and date.",
                "shout           ~> Displays messages.",
                "wait            ~> Suspends processing of a command for the given number of seconds.",
                "pause           ~> Suspends processing of a command and displays the message.",
                "run             ~> Starts a specified program or command, given the full or sysenv path.",
                "cat             ~> Starts an installed program from the system.",
                "allinstapps     ~> Lists all installed apps on your machine.",
                "ls              ~> Displays a list of files and subdirectories in a directory.",
                "cd              ~> Changes the current directory.",
                "touch           ~> Creates a file or folder.",
                "del             ~> Deletes one or more files or folders.",
                "ren             ~> Renames one or more files from one directory to another directory.",
                "copy            ~> Copies one or more files from one directory to another directory.",
                "move            ~> Moves one or more files from one directory to another directory.",
                "pixelate        ~> Starts a website in a web browser.",
                "commit          ~> Edits the contents of a text file.",
                "read            ~> Displays the contents of a text file.",
                "update          ~> Check for Updates.",
                "scan            ~> Scans the integrity of all protected system files.",
                "lock            ~> Locks the System at Start-up.",
                "terminate       ~> Terminates current running process.",
                "generate        ~> Generates a random number between 0 and 1.",
                "ran             ~> Displays operating system configuration information.",
                "tree            ~> Graphically displays the directory structure of a drive or path.",
                "diagxt          ~> Displays machine specific properties and configuration.",
                "restore         ~> Restores system files and folders.",
                "reset           ~> Reset AOs.",
                "assoc           ~> displays or modifies file extension associations.",
                "attrib          ~> displays or changes file attributes.",
                "break           ~> sets or clears extended ctrl+c checking.",
                "bcdedit         ~> sets properties in boot database to control boot loading.",
                "cacls           ~> displays or modifies access control lists (acls) of files.",
                "call            ~> calls one batch program from another.",
                "chcp            ~> displays or sets the active code page number.",
                "chdir           ~> displays the name of or changes the current directory.",
                "chkdsk          ~> checks a disk and displays a status report.",
                "chkntfs         ~> displays or modifies the checking of disk at boot time.",
                "comp            ~> compares the contents of two files or sets of files.",
                "compact         ~> displays or alters the compression of files on ntfs partitions.",
                "convert         ~> converts fat volumes to ntfs.  you cannot convert the current drive.",
                "diskpart        ~> displays or configures disk partition properties.",
                "doskey          ~> edits command lines, recalls windows commands, and creates macros.",
                "driverquery     ~> displays current device driver status and properties.",
                "echo            ~> displays messages, or turns command echoing on or off.",
                "endlocal        ~> ends localization of environment changes in a batch file.",
                "erase           ~> deletes one or more files.",
                "fc              ~> compares two files or sets of files, and displays the differences between them.",
                "find            ~> searches for a text string in a file or files.",
                "findstr         ~> searches for strings in files.",
                "for             ~> runs a specified command for each file in a set of files.",
                "format          ~> formats a disk for use with windows.",
                "fsutil          ~> displays or configures the file system properties.",
                "ftype           ~> displays or modifies file types used in file extension associations.",
                "goto            ~> directs the windows command interpreter to a labeled line in a batch program.",
                "gpresult        ~> displays group policy information for machine or user.",
                "graftabl        ~> enables windows to display an extended character set in graphics mode.",
                "help            ~> provides help information for windows commands.",
                "icacls          ~> display, modify, backup, or restore acls for files and directories.",
                "if              ~> performs conditional processing in batch programs.",
                "label           ~> creates, changes, or deletes the volume label of a disk.",
                "mklink          ~> creates symbolic links and hard links",
                "mode            ~> configures a system device.",
                "more            ~> displays output one screen at a time.",
                "openfiles       ~> displays files opened by remote users for a file share.",
                "path            ~> displays or sets a search path for executable files.",
                "popd            ~> restores the previous value of the current directory saved by pushd.",
                "print           ~> prints a text file.",
                "pushd           ~> saves the current directory then changes it.",
                "recover         ~> recovers readable information from a bad or defective disk.",
                "rem             ~> records comments (remarks) in batch files or config.sys.",
                "replace         ~> replaces files.",
                "robocopy        ~> advanced utility to copy files and directory trees",
                "set             ~> displays, sets, or removes windows environment variables.",
                "setlocal        ~> begins localization of environment changes in a batch file.",
                "sc              ~> displays or configures services (background processes).",
                "schtasks        ~> schedules commands and programs to run on a computer.",
                "shift           ~> shifts the position of replaceable parameters in batch files.",
                "sort            ~> sorts input.",
                "subst           ~> associates a path with a drive letter.",
                "verify          ~> tells windows whether to verify that your files are written correctly to a disk.",
                "vol             ~> displays a disk volume label and serial number.",
                "wmic            ~> displays wmi information inside interactive command shell.",
            };

            if (Collection.Array.IsEmpty(args))
            {
                Console.WriteLine("Type `help <command-name>` for more information on a specific command");
                Array.Sort(HelpCenter);
                Console.WriteLine(string.Join("\n", HelpCenter));
            }

            else
            {
                for (int i = 0; i < args.Length; i++)
                {
                    for (int j = 0; j < HelpCenter.Length; j++)
                    {
                        if (HelpCenter[j].StartsWith(args[i]))
                        {
                            Console.WriteLine(Collection.String.Reduce(HelpCenter[j]));
                            break;
                        }
                    }
                }
            }
        }

        public static bool IsAskingForHelp(string input)
        {
            if (input == "/?" || input == "-h" || input == "--help" || input == "??") return true;
            return false;
        }

        public static void RootPackages()
        {
            Directory.CreateDirectory($"{rDir}\\Files.x72\\etc");
            Directory.CreateDirectory($"{rDir}\\Files.x72\\root\\tmp");
            Directory.CreateDirectory($"{rDir}\\SoftwareDistribution\\RestorePoint");

            if (!File.Exists($"{rDir}\\Files.x72\\root\\HISTORY")) File.Create($"{rDir}\\Files.x72\\root\\HISTORY").Dispose();
            if (!File.Exists($"{rDir}\\Files.x72\\root\\tmp\\BOOT.log")) File.Create($"{rDir}\\Files.x72\\root\\tmp\\BOOT.log").Dispose();
            if (!File.Exists($"{rDir}\\Files.x72\\root\\tmp\\Crashreport.log")) File.Create($"{rDir}\\Files.x72\\root\\tmp\\Crashreport.log").Dispose();
        }

        public static void AskPass()
        {
            if (!File.Exists($"{rDir}\\Files.x72\\root\\User.set")) return;
            while (true)
            {
                Console.Write("Enter password: ");
                string Password = Console.ReadLine();
                if (Password != File.ReadAllText($"{rDir}\\Files.x72\\root\\User.set"))new Error("Incorrect password.");
                else break;
            }
        }

        public static void CommandPrompt(string GetProcess = "")
        {
            ProcessStartInfo StartProcess = new ProcessStartInfo("cmd.exe", $"/c {GetProcess}");
            var Execute = Process.Start(StartProcess);
            Execute?.WaitForExit();
        }

        public static void CheckForUpdates()
        {
            if (!File.Exists($"{rDir}\\SoftwareDistribution\\UpdatePackages\\UPR.exe"))
            {
                string NoteCurrentTime = DateTime.Now.ToString("[dd-MM-yyyy], [HH:mm:ss]");
                File.AppendAllText($"{rDir}\\Files.x72\\root\\tmp\\Crashreport.log", $"{NoteCurrentTime}, UPDATE DIRECTORY is missing or corrupted.\n");

                new Error("Update failed." + "\n" + "UPDATE DIRECTORY is missing or corrupted.");
            }

            else CommandPrompt($"call \"{rDir}\\SoftwareDistribution\\UpdatePackages\\UPR.exe\" -v \"{vNum}\"");
        }

        public static void CreatePass()
        {
            Console.Write("Enter password: ");
            string Password = Console.ReadLine();

            File.WriteAllText($"{rDir}\\Files.x72\\root\\User.set", Password);
            Console.WriteLine("Your password was set successfully.");
        }

        public static string LocateEXE(string Filename)
        {
            string[] Path = Environment.GetEnvironmentVariable("path")?.Split(';') ?? new string[0];
            foreach (string Folder in Path)
            {
                if (File.Exists($"{Folder}{Filename}")) return $"{Folder}{Filename}";
                else if (File.Exists($"{Folder}\\{Filename}")) return $"{Folder}\\{Filename}";
            }

            return string.Empty;
        }

        public static void StartApp(string AppName, string AppArgumens = "", bool AppAdmin = false)
        {
            Process AppProcess = new Process();
            AppProcess.StartInfo.UseShellExecute = true;
            AppProcess.StartInfo.FileName = AppName;
            AppProcess.StartInfo.Arguments = AppArgumens;
            AppProcess.StartInfo.CreateNoWindow = false;
            if (AppAdmin) AppProcess.StartInfo.Verb = "runas";
            try
            {
                AppProcess.Start();
            }

            catch (Exception)
            {
                Console.WriteLine("Cannot open the app");
            }
        }

        public static string Strings(string Line)
        {
            if (Line.StartsWith("\"") && Line.EndsWith("\"")) return Line.Substring(1, Line.Length - 2);
            else if (Line.StartsWith("'") && Line.EndsWith("'")) return Line.Substring(1, Line.Length - 2);
            return Line;
        }

        public static void SYSRestore()
        {
            Console.WriteLine("Restoring.");
            Console.Write("Using 'Sysfail\\RECOVERY' to restore.");
            if (File.Exists($"{rDir}\\safe.exe"))
            {
                CommandPrompt($"call \"{rDir}\\safe.exe\" recover");
                Console.WriteLine("Restore successful.");
            }

            else
            {
                string NoteCurrentTime = DateTime.Now.ToString("[dd-MM-yyyy], [HH:mm:ss]");
                File.AppendAllText($"{rDir}\\Files.x72\\root\\tmp\\Crashreport.log", $"{NoteCurrentTime}, RECOVERY DIRECTORY is missing or corrupted.\n");

                new Error("\n" + "Cannot restore this PC." + "\n" + "RECOVERY DIRECTORY is missing or corrupted.");
                Environment.Exit(0);
            }
        }

        public static bool Scan()
        {
            List<string> Errors = new List<string>();
            string[] CheckFor = {
                $"{rDir}\\SoftwareDistribution\\UpdatePackages\\UPR.exe",
                $"{rDir}\\safe.exe", $"{rDir}\\Sysfail\\RECOVERY",
                $"{rDir}\\Files.x72\\root\\Config.set",
                $"{rDir}\\Sysfail\\rp",
            };

            // Scan the system.
            for (int i = 0; i < CheckFor.Length; i++)
            {
                if ((Directory.Exists(CheckFor[i]) && !Directory.EnumerateFileSystemEntries(CheckFor[i], "*", SearchOption.AllDirectories).Any()) || (!Directory.Exists(CheckFor[i]) && !File.Exists(CheckFor[i])))
                    Errors.Add(CheckFor[i]);
            }

            string[] MissingFiles = Errors.ToArray();

            // Check for corrupted files.
            if (!Collection.Array.IsEmpty(MissingFiles))
            {
                string NoteCurrentTime = DateTime.Now.ToString("[dd-MM-yyyy], [HH:mm:ss]");

                File.AppendAllText($"{rDir}\\Files.x72\\root\\tmp\\Crashreport.log", $"{NoteCurrentTime}, [{string.Join(", ", MissingFiles)}] file(s) were missing.\n");

                new Error($"{MissingFiles.Length} Errors were Found! {string.Join(" ", MissingFiles)}" + "\n" + "Your PC ran into a problem :(");
                Console.Write("Press any Key to Continue.");
                Console.ReadKey();
                Console.WriteLine();
                SYSRestore();

                Console.WriteLine("A restart is recommended.");
                return false;
            }

            return true;
        }

        public static string SetTerminalPrompt(string[] Flags)
        {
            string PromptMessage = "";
            foreach (string i in Flags)
            {
                if (i.ToLower() == "-v") PromptMessage += new Obsidian().Version;
                else if (i.ToLower() == "-s") PromptMessage += " ";
                else if (i.ToLower() == "-t") PromptMessage += DateTime.Now.ToString("HH:mm:ss");
                else if (i.ToLower() == "-d") PromptMessage += DateTime.Now.ToString("dd-MM-yyyy");
                else if (i.ToLower() == "-p") PromptMessage += Directory.GetCurrentDirectory();
                else if (i.ToLower() == "-n") PromptMessage += Path.GetPathRoot(Environment.SystemDirectory);
                else PromptMessage += i;
            }

            return PromptMessage;
        }
    }

    public class History
    {
        public static void SetHistory(string cmd)
        {
            string CurrentTime = DateTime.Now.ToString("[dd-MM-yyyy HH:mm:ss]");
            File.AppendAllText($"{rDir}\\Files.x72\\root\\HISTORY", $"{CurrentTime}, '{cmd}'\n");
        }

        public static void GetHistory()
        {
            Console.WriteLine(File.ReadAllText($"{rDir}\\Files.x72\\root\\HISTORY"));
        }

        public static void ClearHistory()
        {
            File.Delete($"{rDir}\\Files.x72\\root\\HISTORY");
        }
    }
}
