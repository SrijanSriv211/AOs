using System;
using System.IO;
using System.Threading;
using System.Diagnostics;

namespace AOs
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "AOs";
            if (BootLoader("Starting up.")) System();
            else Environment.Exit(0);
        }

        static void System()
        {
            Console.Title = "AOs";
            Console.ResetColor();
            Console.Clear();

            string Command;
            string Prompt = "$ ";
            string SYSVersion = "AOs 2021 [Version 1.7.3]";

            bool Fresh = true;

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(SYSVersion);
            Console.ResetColor();
            while (true)
            {
                if (Fresh == true)
                {
                    Fresh = false;
                    Console.Write(Prompt);
                }

                else Console.Write($"\n{Prompt}");
                Command = Console.ReadLine().Trim();

                if (Command == "") continue;
                else if (Command.ToLower() == "quit" || Command.ToLower() == "exit") Environment.Exit(0);
                else if (Command.ToLower() == "clear" || Command.ToLower() == "cls")
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(SYSVersion);
                    Console.ResetColor();
                }

                else if (Command.ToLower() == "restart")
                {
                    Console.ResetColor();
                    Console.Clear();

                    Console.Title = "AOs";
                    if (BootLoader("Restart.")) System();
                    else Environment.Exit(0);
                }

                else if (Command.ToLower() == "shutdown")
                {
                    Console.ResetColor();
                    Console.Clear();

                    if (BootLoader("Shutdown.")) Environment.Exit(0);
                    else Environment.Exit(0);
                }

                else if (Command.ToLower() == "help")
                {
                    string[] HelpCenter = {
                    "about    - Tells you about AOs.",
                    "shutdown - Shuts your PC down.",
                    "restart  - Restarts your PC.",
                    "clear    - Clears the console.",
                    "color    - Changes the user-interface theme.",
                    "title    - Changes the title of the AOs window.",
                    "calendar - Displays current time and date.",
                    "credits  - Provides Credit to Developers.",
                    "admin    - An administrator tool for more advanced AOs commands.",
                    "version  - Shows you the current version of AOs.",
                    "run      - Allows you to applications that exists in your system.",
                    "console  - Opens System Terminal using AOs.",
                    "notebook - Notebook is a basic Terminal based Text Editor.",
                    "timer    - Creats a stop-watch for users.",
                    "scan     - Scans the system to check for viruses, malwares, spywares and etc.",
                    "math     - Calculate integers given by user."
                    };

                    Console.WriteLine("Type 'help' to get information of all the commands.");
                    Array.Sort(HelpCenter);
                    for (int i = 0; i < HelpCenter.Length; i++) Console.WriteLine(HelpCenter[i]);
                }

                else if (Command.EndsWith("$Prompt")) Prompt = Command.Replace("Prompt", " ");
                else if (Command.StartsWith(">"))
                {
                    string SysCmd = Command.Substring(1);
                    CommandPrompt(SysCmd);
                }

                else if (Command == "AOs1000")
                {
                    Console.Clear();
                    Console.WriteLine("Congratulations for hitting 1000 Lines Of Code in AOs!");
                    Console.WriteLine("It was the first program to ever reach these many Lines Of Code.");
                }

                else if (Command.ToLower() == "math")
                {
                    Console.Write("Enter your First number: ");
                    int n1 = Convert.ToInt32(Console.ReadLine());

                    Console.Write("Enter your Second number: ");
                    int n2 = Convert.ToInt32(Console.ReadLine());

                    Console.Write("Enter you Operator: ");
                    string ope = Console.ReadLine();
                    Calculate(n1, n2, ope);
                }

                else if (Command.ToLower() == "scan")
                {
                    Timer();
                    Console.Write("Scanning.");
                    Console.WriteLine("Scanning completed successfully!");

                    Thread.Sleep(3000);
                    Console.WriteLine("Generating report.");

                    Thread.Sleep(1000);
                    Random Ran = new Random();
                    if (File.Exists("Config.set") == false) Console.WriteLine("Your PC ran into a problem :(");
                    else
                    {
                        Console.WriteLine("Your PC is working fine.");
                        Console.Write("Continue.");
                        Console.ReadKey();
                        Console.WriteLine("");
                    }
                }

                else if (Command.ToLower() == "timer")
                {
                    Console.WriteLine("Set time.");
                    Console.Write("Seconds: ");
                    long sec = Convert.ToInt64(Console.ReadLine());
                    for (int defaultTime = 1; defaultTime <= sec; defaultTime++)
                    {
                        Thread.Sleep(1000);
                        Console.Clear();
                        Console.WriteLine(defaultTime);
                        if (defaultTime == sec)
                        {
                            Console.Write("Times up...\nContinue.");
                            Console.ReadKey();
                            Console.WriteLine("");
                        }
                    }
                }

                else if (Command.ToLower() == "notebook")
                {
                    Console.WriteLine("notebook [your file name].");
                    Console.WriteLine("Press the Escape key to quit Notebook.");
                }

                else if (Command.ToLower().StartsWith("notebook "))
                {
                    string[] SplitCommand = Command.Split(new[] {' '}, 2);
                    Notebook(SplitCommand[1]);
                }

                else if (Command.ToLower() == "admin")
                {
                    Console.WriteLine("Administrator carries an advanced of AOs commands.");
                    Console.WriteLine("Type 'admin help' to get a list all administrator commands.");
                }

                else if (Command.ToLower().StartsWith("admin "))
                {
                    string[] SplitCommand = Command.Split(new[] {' '}, 2);

                    Directory.SetCurrentDirectory("..");
                    if (SplitCommand[1].ToLower() == "help")
                    {
                        string[] HelpCenter = {
                        "diagxt    - Displays machine specific properties and configuration.",
                        "lock      - Lock your System at Start-up.",
                        "reset     - Resets your PC for Better Performance."
                        };

                        Console.WriteLine("Type 'admin help' to get information of all administrator commands.");
                        Array.Sort(HelpCenter);
                        for (int i = 0; i < HelpCenter.Length; i++) Console.WriteLine(HelpCenter[i]);
                    }

                    else if (SplitCommand[1].ToLower() == "lock")
                    {
                        LockSYS();
                        Console.WriteLine("Use 'admin rm-lock' command to remove password.");
                    }

                    else if (SplitCommand[1].ToLower() == "rm-lock")
                    {
                        if (File.Exists("user.set"))
                        {
                            File.Delete("user.set");
                            Console.WriteLine("Password removed successfully.");
                        }

                        else Console.WriteLine("Your System has no Security.");
                    }

                    else if (SplitCommand[1].ToLower() == "diagxt")
                    {
                        string Cfg = File.ReadAllText("Config.set");
                        Console.WriteLine(Cfg);
                    }

                    else if (SplitCommand[1].ToLower() == "reset")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("WARNING: Formatting your PC will delete all your system and personal data including AOs update backup and reset AOs to default settings.");
                        Console.WriteLine("Are you sure? Y/N");
                        Console.ResetColor();

                        Console.Write("> ");
                        ConsoleKeyInfo FormatKey = Console.ReadKey();
                        string GetKey = FormatKey.Key.ToString();
                        Console.WriteLine("");
                        if (GetKey == "Y")
                        {
                            try
                            {
                                Directory.Delete("Files.x72");
                                File.Delete("BOOT.log");
                                if (File.Exists("Crashreport.log")) File.Delete("Crashreport.log");
                                if (Directory.Exists("UpdatePackages/AOs.old")) Directory.Delete("UpdatePackages/AOs.old");

                                Console.WriteLine("System Format Completed!");
                                Console.Write("A system restart is required to apply all changes.");
                                Console.ReadKey();
                                BootLoader("Restart.");
                            }

                            catch (Exception)
                            {
                                Console.WriteLine("Cannot perform a System Format.");
                            }
                        }

                        else if (GetKey == "N") Console.WriteLine("System Format Cancelled!");
                        else Console.WriteLine("Invalid Key Input.");
                    }

                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"{SplitCommand[1]}, Administrator Command does not exist.");
                        Console.ResetColor();
                    }

                    Directory.SetCurrentDirectory("Files.x72");
                }

                else if (Command.ToLower() == "calendar") Console.WriteLine(DateTime.Now.ToString("[dd-MM-yyyy], [HH:mm:ss]"));
                else if (Command.ToLower().StartsWith("color ")) CommandPrompt(Command);
                else if (Command.ToLower().StartsWith("title ")) CommandPrompt(Command);
                else if (Command.ToLower() == "version" || Command.ToLower() == "-v") Console.WriteLine(SYSVersion);
                else if (Command.ToLower() == "credits")
                {
                    string[] CreditCenter = {
                        "_________ AOS - Team ________",
                        "Developer - Srijan Srivastava",
                        "Found on  - 15 June 2020",
                        "Github    - github.com/Light-Lens",
                        "",
                        "____________________ Note(For Developers Only) ____________________",
                        "|| AOs - Terminal based Operating System",
                        "|| Contact: Srivastavavsrijan321@gmail.com",
                        "",
                        "|| AOs is an Open-Source Software.",
                        "|| If you want to modify or contribute on this project.",
                        "|| Then you can do anything you want.",
                        "|| But only at a condition,",
                        "|| When you say in your program 'Based on AOs Kernel 1.7'.",
                        "",
                        "____________________ Note(For All) ____________________",
                        "|| Warning - Do not delete any of the files or folders,",
                        "|| or it may cause system corruption",
                        "|| and may lead AOs not to boot.",
                        "",
                        "For more information type 'help' or checkout README.docx"
                    };

                    Console.Clear();
                    for (int i = 0; i < CreditCenter.Length; i++)
                    {
                        Console.WriteLine(CreditCenter[i]);
                    }

                    Console.Write("Continue.");
                    Console.ReadKey();
                    Console.WriteLine("");
                    Console.Clear();
                }

                else if (Command.ToLower() == "about") Console.WriteLine("AOs is an Open-source Terminal based Operating System written in C#. It is inspired by MS-DOS. It is designed to improve User's Efficiency and Productivity while working.");
                else if (Command.ToLower() == "run")
                {
                    Console.WriteLine("You need to give some file name that you want to run.");
                    Console.WriteLine("You can use '-e' command line at the end to launch the application separately from AOs.");
                }

                else if (Command.ToLower().StartsWith("run "))
                {
                    string[] RUN = Command.Split(new[] {' '}, 2);

                    if (RUN[1].EndsWith(" -e"))
                    {
                        string EXTERNALRUN = RUN[1].Replace(" -e", "");
                        CommandPrompt($"start {EXTERNALRUN}");
                    }

                    else CommandPrompt($"call {RUN[1]}");
                }

                else if (Command.ToLower() == "console" || Command.ToLower() == "terminal" || Command.ToLower() == "cmd") CommandPrompt("start cmd");
                else if (Command.ToLower().StartsWith("console ") || Command.ToLower().StartsWith("terminal ") || Command.ToLower().StartsWith("cmd "))
                {
                    string[] SplitCommand = Command.Split(new[] {' '}, 2);
                    CommandPrompt(SplitCommand[1]);
                }

                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\'{Command}\', Command does not exist.");
                    Console.ResetColor();
                }
            }
        }

        static void Notebook(string Filename)
        {
            Console.WriteLine("Press the Escape key to quit Notebook.");
            while (true)
            {
                ConsoleKeyInfo PressedKey = Console.ReadKey();
                string Data = Console.ReadLine();
                if (PressedKey.Key == ConsoleKey.Escape) break;

                File.AppendAllText(Filename, $"{Data}\n");
            }
        }

        static void LockSYS()
        {
            Console.Clear();
            if (File.Exists("user.set"))
            {
                Console.ResetColor();
                string Cd = File.ReadAllText("user.set");
                while (true)
                {
                    Console.Clear();
                    Console.Write("Enter password: ");
                    string ByPassCode = Console.ReadLine();
                    if (ByPassCode == Cd)
                    {
                        Console.Write("Correct password.\nContinue.");
                        Console.ReadKey();
                        Console.WriteLine("");
                        break;
                    }

                    else
                    {
                        Console.Write("Incorrect password.\nContinue.");
                        Console.ReadKey();
                        Console.WriteLine("");
                    }
                }

                Console.Clear();
            }

            else if (File.Exists("user.set") == false)
            {
                Console.Write("Set Password: ");
                string pinLock = Console.ReadLine();

                Console.Clear();
                Directory.SetCurrentDirectory("..");
                File.WriteAllText("user.set", $"{pinLock}");
                while(true)
                {
                    Console.Write("Enter Password: ");
                    string askPin = Console.ReadLine();
                    if (askPin == pinLock)
                    {
                        Console.Write("Correct password.\nContinue.");
                        Console.ReadKey();
                        Console.WriteLine("");
                        Console.Clear();
                        break;
                    }

                    else
                    {
                        Console.Write("Incorrect password.\nContinue.");
                        Console.ReadKey();
                        Console.WriteLine("");
                        Console.Clear();
                    }
                }
            }

            Directory.SetCurrentDirectory("Files.x72");
        }

        static void Calculate(int num1, int num2, string ope)
        {
            if (ope == "+")
            {
                Console.WriteLine(num1 + num2);
            }

            else if (ope == "-")
            {
                Console.WriteLine(num1 - num2);
            }

            else if (ope == "*" || ope == "x")
            {
                Console.WriteLine(num1 * num2);
            }

            else if (ope == "/")
            {
                Console.WriteLine(num1 / num2);
            }
        }

        static void CommandPrompt(string GetProcess)
        {
            ProcessStartInfo StartProcess = new ProcessStartInfo("cmd.exe", $"/c {GetProcess}");
            var Execute = Process.Start(StartProcess);
            Execute.WaitForExit();
        }

        static void Timer()
        {
            for (int i = 0; i <= 100; i++)
            {
                Thread.Sleep(25);
                Console.Write("-");
                if (i >= 100)
                {
                    Console.WriteLine(">");
                    break;
                }
            }
        }

        static void RootPackages()
        {
            if (Directory.Exists("Files.x72"))
            {
                Directory.SetCurrentDirectory("Files.x72");
            }

            else
            {
                Directory.CreateDirectory("Files.x72");
                Directory.SetCurrentDirectory("Files.x72");
            }
        }

        static bool BootLoader(string Loadstatus)
        {
            int Errors = 0;
            string RootDir = Directory.GetCurrentDirectory();
            if (RootDir.Contains("AOs\\Files.x72")) Directory.SetCurrentDirectory("..");
            if (File.Exists("BOOT.log") == false) File.WriteAllText("BOOT.log", "AOs 2021 [Version 1.7.3] - BOOT_LOG\n");

            Console.WriteLine(Loadstatus);
            Console.WriteLine("This may take some time.\nPlease wait!\n");

            if (File.Exists("Config.set") == false) Errors++;
            Timer();

            Console.Clear();
            Console.WriteLine(Loadstatus);
            if (Errors == 0)
            {
                if (File.Exists("BOOT.log")) File.AppendAllText("BOOT.log", $"AOs booted at {DateTime.Now.ToString("[dd-MM-yyyy], [HH:mm:ss]")}\n");
                if (File.Exists("user.set"))
                {
                    Console.ResetColor();
                    string Cd = File.ReadAllText("user.set");
                    while (true)
                    {
                        Console.Clear();
                        Console.Write("Enter password: ");
                        string ByPassCode = Console.ReadLine();
                        if (ByPassCode == Cd)
                        {
                            Console.Write("Correct password.\nContinue.");
                            Console.ReadKey();
                            Console.WriteLine("");
                            break;
                        }

                        else
                        {
                            Console.Write("Incorrect password.\nContinue.");
                            Console.ReadKey();
                            Console.WriteLine("");
                        }
                    }
                }

                RootPackages();
                Console.Write("Done.");
                Console.ReadKey();
                return true;
            }

            else
            {
                if (File.Exists("BOOT.log")) File.AppendAllText("BOOT.log", $"AOs crashed at {DateTime.Now.ToString("[dd-MM-yyyy], [HH:mm:ss]")}\n");

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{Errors} Startup Errors were Found!");
                Console.ResetColor();
                Console.Write("Done.");
                Console.ReadKey();
                return false;
            }
        }
    }
}
