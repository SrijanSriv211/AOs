using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Diagnostics;

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

            return false;
        }

        public static void GetHelp()
        {
            GetHelp(new string[0]);
        }

        public static void GetHelp(string[] args)
        {
            string[] HelpCenter = {
                "about           ~> About AOs.",
                "admin           ~> Administrator.",
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
                "get             ~> Asks for input.",
                "pause           ~> Suspends processing of a command and displays the message.",
                "wait            ~> Suspends processing of a command for the given number of seconds.",
                "run             ~> Starts a specified program or command.",
                "ls              ~> Displays a list of files and subdirectories in a directory.",
                "cd              ~> Changes the current directory.",
                "touch           ~> Creates a file or folder.",
                "del             ~> Deletes one or more files or folders.",
                "ren             ~> Renames one or more files from one directory to another directory.",
                "copy            ~> Copies one or more files from one directory to another directory.",
                "move            ~> Moves one or more files from one directory to another directory.",
                "alarm           ~> Sets an alarm.",
                "lock            ~> Locks the system temporarily.",
                "pixelate        ~> Starts a website in a web browser.",
                "prompt          ~> Specifies a new command prompt.",
                "apps            ~> Lists all installed apps on your machine.",
                "commit          ~> Edits a text file via command prompt.",
                "read            ~> Reads a text file via command prompt."
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
                            Console.WriteLine(HelpCenter[j]);
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
                if (Password != File.ReadAllText($"{rDir}\\Files.x72\\root\\User.set"))
                {
                    var Color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Incorrect password.");
                    Console.ForegroundColor = Color;
                }

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
                File.AppendAllText($"{rDir}\\Files.x72\\Temp\\log\\Crashreport.log", $"{NoteCurrentTime}, UPDATE DIRECTORY is missing or corrupted.\n");

                var Color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Update failed.");
                Console.WriteLine("UPDATE DIRECTORY is missing or corrupted.");
                Console.ForegroundColor = Color;
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
            if (File.Exists($"{rDir}\\RESTORE.exe"))
            {
                CommandPrompt($"call \"{rDir}\\RESTORE.exe\"");
                Console.WriteLine("AOs restore successful.");
            }

            else
            {
                string NoteCurrentTime = DateTime.Now.ToString("[dd-MM-yyyy], [HH:mm:ss]");
                File.AppendAllText($"{rDir}\\Files.x72\\Temp\\log\\Crashreport.log", $"{NoteCurrentTime}, RECOVERY DIRECTORY is missing or corrupted.\n");

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nCannot restore this PC.");
                Console.WriteLine("RECOVERY DIRECTORY is missing or corrupted.");
                Console.ResetColor();

                Environment.Exit(0);
            }
        }

        public static bool Scan()
        {
            string Errors = "";
            string[] CheckFor = {
                $"{rDir}\\Files.x72\\img\\src", $"{rDir}\\Files.x72\\img\\ain",
                $"{rDir}\\SoftwareDistribution\\UpdatePackages\\UPR\\UPR.exe",
                $"{rDir}\\Files.x72\\Packages\\data\\amd", $"{rDir}\\Files.x72\\Packages\\data\\oza",
                $"{rDir}\\RESTORE.exe", $"{rDir}\\Sysfail\\RECOVERY", $"{rDir}\\Files.x72\\Temp\\set\\Config.set"
            };

            // Scan the system.
            for (int i = 0; i < CheckFor.Length; i++)
            {
                if (Directory.Exists(CheckFor[i]) && !Directory.EnumerateFileSystemEntries(CheckFor[i], "*", SearchOption.AllDirectories).Any())
                    Errors += $"{CheckFor[i]} ";

                else if (!Directory.Exists(CheckFor[i]) && !File.Exists(CheckFor[i]))
                    Errors += $"{CheckFor[i]} ";
            }

            // Check for corrupted files.
            if (!string.IsNullOrEmpty(Errors.Trim()))
            {
                string[] Missing = Errors.Trim().Split();
                string NoteCurrentTime = DateTime.Now.ToString("[dd-MM-yyyy], [HH:mm:ss]");

                File.AppendAllText($"{rDir}\\Files.x72\\Temp\\log\\Crashreport.log", $"{NoteCurrentTime}, [{string.Join(", ", Missing)}]\n");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{Missing.Length} Errors were Found!");
                Console.WriteLine("Your PC ran into a problem :(");
                Console.ResetColor();
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
