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
            string SYSVersion = "AOs 2021 [Version 1.7.2]";

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
                    "console  - Opens System Terminal using AOs."
                    };

                    Console.WriteLine("Type 'help' to get information of all the commands.");
                    Array.Sort(HelpCenter);
                    for (int i = 0; i < HelpCenter.Length; i++)
                    {
                        Console.WriteLine(HelpCenter[i]);
                    }
                }

                else if (Command.ToLower() == "admin")
                {
                    Console.WriteLine("Administrator carries an advanced of AOs commands.");
                    Console.WriteLine("Use admin <command> to access administrator commands.");
                }

                else if (Command.ToLower().StartsWith("admin "))
                {
                    // code here.
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
                        "|| When you say in your program \"Based on AOs Kernel 1.7\".",
                        "",
                        "____________________ Note(For All) ____________________",
                        "|| Warning - Do not delete any of the files or folders,",
                        "|| or it may cause system corruption",
                        "|| and may lead AOs not to boot.",
                        "",
                        "For more information type \"help\" or checkout README.docx"
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
                    Console.WriteLine("You can use \"-e\" command line at the end to launch the application separately from AOs.");
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
            if (File.Exists("BOOT.log") == false) File.WriteAllText("BOOT.log", "AOs 2021 [Version 1.7.2] - BOOT_LOG\n");

            Console.WriteLine(Loadstatus);
            Console.WriteLine("This may take some time.\nPlease wait!\n");

            if (File.Exists("Config.set") == false) Errors++;
            Timer();

            Console.Clear();
            Console.WriteLine(Loadstatus);
            if (Errors == 0)
            {
                if (File.Exists("BOOT.log")) File.AppendAllText("BOOT.log", $"AOs booted at {DateTime.Now.ToString("[dd-MM-yyyy], [HH:mm:ss]")}\n");
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
