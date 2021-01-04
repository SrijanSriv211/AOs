using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Globalization;
using System.Collections.Generic;

namespace AOs
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "AOs";
            BIOS("Done.");
        }

        static void Sys()
        {
            Console.Clear();
            bool Loop = true;
            while (Loop)
            {
                Console.Write("$");
                string Check = Console.ReadLine();
                string Global = Check.ToLower();
                if (Check == "" || Check == " " || Check.Contains("  "))
                {
                    continue;
                }

                else if (Check.ToLower() == "help")
                {
                    string[] HelpCenter = {
                    "about    - Tells you about AOs.",
                    "shutdowm - Shuts your PC down.",
                    "restart  - Restarts your PC.",
                    "shout    - Prints to the console.",
                    "get      - Takes input from the User.",
                    "generate - Generates a random number.",
                    "clear    - Clears the console.",
                    "scan     - Scans the system to check for viruses, malwares, spywares and etc.",
                    "color    - Changes the user-interface theme.",
                    "update   - Updates your pc to the latest version.",
                    "refresh  - Optimizes the pc for better performance.",
                    "math     - Calculate integers given by user.",
                    "calander - Displays current time and date.",
                    "credits  - Provides Credit to Developers.",
                    "AOs1000  - AOs is made on 1000 lines of code.",
                    "timer    - Creats a stop-watch for users.",
                    "lock     - Locks your pc so others can't access your system.",
                    "cmd      - Opens Command Prompt window.",
                    "admin    - An adminstrator tool for more advanced AOs commands.",
                    "pixstore - Helps you to install any applications easily.",
                    "builder  - Provides user a notepad to store multi-line data.",
                    "version  - Shows you the current version of AOs.",
                    "leaf     - Leaf is a webbrowser you can use to open webpages."
                    };
                    Console.WriteLine("Type 'help' to get information of all the commands.");
                    Array.Sort(HelpCenter);
                    for (int i = 0; i < HelpCenter.Length; i++)
                    {
                        Console.WriteLine(HelpCenter[i]);
                    }
                }

                else if (Global.StartsWith("leaf ") || Check.ToLower() == "leaf")
                {
                    string Leaf = Check.Replace("leaf ", "start ");
                    CommandPrompt(Leaf);
                }

                else if (Check.ToLower() == "version" || Check.ToLower() == "-v")
                {
                    Console.WriteLine("AOs 2020 [Version 1.5.4]");
                }

                else if (Global.StartsWith("builder ") || Check.ToLower() == "builder")
                {
                    if (Global.StartsWith("builder "))
                    {
                        string Builder = Check.Replace("builder ", "");
                        string Text = Builder.ToLower();
                        if (Text.StartsWith("open "))
                        {
                            string Open_Text = Text.Replace("open ", "");
                            string op = Open_Text.Replace(" ", "");
                            if (File.Exists(op))
                            {
                                string READ_FILE = File.ReadAllText(op);
                                Console.WriteLine(READ_FILE);
                            }

                            else
                            {
                                Console.WriteLine($"Cannot open {op}");
                            }
                        }

                        else if (Text.StartsWith("delete "))
                        {
                            string Build = Text.Replace("delete ", "");
                            string Txt = Build.Replace(" ", "");
                            if (File.Exists(Txt))
                            {
                                File.Delete(Txt);
                                Console.WriteLine($"\"{Txt}\" deleted successfully");
                            }

                            else
                            {
                                Console.WriteLine($"Cannot delete {Txt}");
                            }
                        }

                        else if (!Text.StartsWith("delete ") && !Text.StartsWith("open "))
                        {
                            string Txt = Text.Replace(" ", "");
                            if (File.Exists(Txt))
                            {
                                string CD = File.ReadAllText(Txt);
                                Console.WriteLine($"{CD}Add more text.");
                                Builder_Txt(Txt);
                            }

                            else
                            {
                                File.WriteAllText(Txt, "");
                                Builder_Txt(Txt);
                            }
                        }
                    }

                    else if (Check.ToLower() == "builder")
                    {
                        Console.WriteLine("You need to give some file name, such as \"Test.txt\".");
                        Console.WriteLine("You need to add some arguments, such as, \"open\", \"delete\".");
                        Console.WriteLine("Use \"--builder<quit>\" to exit.");
                    }
                }

                else if (Global.StartsWith("pixstore ") || Check.ToLower() == "pixstore")
                {
                    if (Global.StartsWith("pixstore "))
                    {
                        string App = Check.Replace("pixstore ", "");
                        string application = App.ToLower();
                        if (application.StartsWith("open "))
                        {
                            string Open = application.Replace("open ", "");
                            string apps = Open.Replace(" ", "");
                            if (File.Exists($"{apps}.co"))
                            {
                                Console.WriteLine($"Opening {apps}");                         
                            }

                            else
                            {
                                Console.WriteLine($"Cannot open {apps}");
                            }                       
                        }

                        else if (application.StartsWith("install "))
                        {
                            string Install = application.Replace("install ", "");
                            string apps = Install.Replace(" ", "");
                            if (File.Exists($"{apps}.co"))
                            {
                                Console.WriteLine($"Collecting {apps}");
                                Thread.Sleep(1000);

                                Console.WriteLine($"Updating {apps}");
                                Console.WriteLine($"Finalizing {apps}");
                                Thread.Sleep(3000);

                                File.WriteAllText($"{apps}.co", $"\"{apps}\" updated successfully.");
                                Console.WriteLine($"{apps} updated\n");                          
                            }

                            else
                            {
                                Console.WriteLine($"Collecting {apps}");
                                Thread.Sleep(1000);

                                Console.WriteLine($"Installing {apps}");
                                Console.WriteLine($"Finalizing {apps}");
                                Thread.Sleep(3000);

                                File.WriteAllText($"{apps}.co", $"\"{apps}\" installed successfully.");
                                Console.WriteLine($"{apps} installed\n");

                                File.AppendAllText("Pixstore_installed.log", $"{apps}\n");
                            }
                        }

                        else if (application.StartsWith("update "))
                        {
                            string Uninstall = application.Replace("update ", "");
                            string apps = Uninstall.Replace(" ", "");
                            if (File.Exists($"{apps}.co"))
                            {
                                Console.WriteLine($"Collecting {apps}");
                                Thread.Sleep(1000);

                                Console.WriteLine($"Updating {apps}");
                                Console.WriteLine($"Finalizing {apps}");
                                Thread.Sleep(3000);

                                File.WriteAllText($"{apps}.co", $"\"{apps}\" updated successfully.");
                                Console.WriteLine($"{apps} updated\n");
                            }

                            else
                            {
                                Console.WriteLine($"Cannot update {Uninstall}");
                            }
                        }

                        else if (application.StartsWith("uninstall "))
                        {
                            string Uninstall = application.Replace("uninstall ", "");
                            string apps = Uninstall.Replace(" ", "");
                            if (File.Exists($"{apps}.co"))
                            {
                                Console.WriteLine($"Collecting {apps}");
                                Thread.Sleep(1000);

                                Console.WriteLine($"Uninstalling {apps}");
                                Console.WriteLine($"Finalizing {apps}");
                                Thread.Sleep(3000);

                                File.Delete($"{apps}.co");

                                Console.WriteLine($"{apps} uninstalled\n");

                                string Deline = File.ReadAllText("Pixstore_installed.log");
                                string[] Strlist = Deline.Split($"{apps}\n");
                                string NewLine = string.Join("", Strlist);
                                string repl = NewLine.Replace("\n", "");
                                File.WriteAllText("Pixstore_installed.log", $"{repl}\n");
                            }

                            else
                            {
                                Console.WriteLine($"Cannot uninstall {Uninstall}");
                            }
                        }

                        else if (App.ToLower() == "list")
                        {
                            if (File.Exists("Pixstore_installed.log"))
                            {
                                string Cd = File.ReadAllText("Pixstore_installed.log");
                                if (Cd == "")
                                {
                                    Console.WriteLine("No software listed.");
                                }

                                else
                                {
                                    Console.WriteLine(Cd);
                                }
                            }

                            else
                            {
                                Console.WriteLine("You haven't installed any programs yet.");
                            }
                        }
                    }

                    else if (Check.ToLower() == "pixstore")
                    {
                        Console.WriteLine("You need to add some arguments, such as, \"install\", \"update\", \"uninstall\", \"list\"");
                    }
                }

                else if (Check.ToLower() == "admin")
                {
                    Adminstrator();
                }

                else if (Check.ToLower() == "cmd")
                {
                    CommandPrompt("start cmd");
                }

                else if (Check.ToLower() == "lock")
                {
                    LockPC();
                }

                else if (Check.ToLower() == "timer")
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

                else if (Check == "AOs1000")
                {
                    Console.Clear();
                    Console.WriteLine("Congratulations for hitting 1000 lines of code in AOs.");
                    Console.WriteLine("It is your first program to hit 1000 lines of code.");
                }

                else if (Check.ToLower() == "credits")
                {
                    string[] CreditCenter = {
                        "_________ AOS - Team ________",
                        "Developer - Srijan Srivastava",
                        "Found on  - 15 June 2020",
                        "",
                        "____________________ Note(For Developers Only) ____________________",
                        "|| AOs - Terminal based Operating System",
                        "|| Contact: Srivastavavsrijan321@gmail.com",
                        "",
                        "|| From now onwards AOs is not an Open-Source software.",
                        "|| If you want to modify or contribute on this project.",
                        "|| Then you must Contact me on my gmail address."
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

                else if (Check.ToLower() == "calander")
                {
                    Console.WriteLine(DateTime.Now.DayOfWeek);
                    Console.WriteLine(DateTime.Now.TimeOfDay);
                    Console.WriteLine(DateTime.Now.Date);
                }

                else if (Check.ToLower() == "math")
                {
                    Console.Write("Enter your First number: ");
                    int n1 = Convert.ToInt32(Console.ReadLine());

                    Console.Write("Enter your Second number: ");
                    int n2 = Convert.ToInt32(Console.ReadLine());

                    Console.Write("Enter you Operator: ");
                    string ope = Console.ReadLine();
                    Calculate(n1, n2, ope);
                }

                else if (Check.ToLower() == "scan")
                {
                    Scan();
                }

                else if (Check.ToLower() == "refresh")
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("Refresh.");
                }

                else if (Check.ToLower() == "color")
                {
                    string[] Colors = {
                    "1 >>      Default",
                    "2 >>      Black - White",
                    "3 >>      White - Red",
                    "4 >>      Red - White"
                    };
                    for (int i = 0; i < Colors.Length; i++)
                    {
                        Console.WriteLine(Colors[i]);
                    }
                    Console.Write("\nContinue.");
                    ConsoleKeyInfo Control = Console.ReadKey();
                    string GetKey = Control.Key.ToString();
                    Console.WriteLine("");
                    if (GetKey == "D1")
                    {
                        Console.ResetColor();
                        Console.WriteLine("Done.");
                        Thread.Sleep(1000);
                        Console.Clear();
                    }

                    else if (GetKey == "D2")
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine("Done.");
                        Thread.Sleep(1000);
                        Console.Clear();
                    }

                    else if (GetKey == "D3")
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Done.");
                        Thread.Sleep(1000);
                        Console.Clear();
                    }

                    else if (GetKey == "D4")
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Done.");
                        Thread.Sleep(1000);
                        Console.Clear();
                    }

                    else
                    {
                        Console.WriteLine("Given theme does not exist.");
                    }
                }

                else if (Check.ToLower() == "update")
                {
                    Load("Update.");
                    Update();
                }

                else if (Check.ToLower() == "about")
                {
                    Console.WriteLine("AOs is a terminal based Operating System inspired by MS-DOS and BatchUnited.");
                }

                else if (Global.StartsWith("shout "))
                {
                    string Repl = Check.Replace("shout ", "");
                    Console.WriteLine(Repl);
                }

                else if (Global.StartsWith("get "))
                {
                    string Repl = Check.Replace("get ", "");
                    Console.Write(Repl);
                    Console.ReadLine();
                }

                else if (Check.ToLower() == "shout")
                {
                    Console.WriteLine("");
                }

                else if (Check.ToLower() == "get")
                {
                    Console.ReadLine();
                }

                else if (Check.ToLower() == "clear" || Check.ToLower() == "cls")
                {
                    Console.Clear();
                }

                else if (Check.ToLower() == "generate")
                {
                    Console.WriteLine("Generate a random number");
                    Console.Write("Enter you first number: ");
                    int n1 = Convert.ToInt32(Console.ReadLine());

                    Console.Write("Enter you second number: ");
                    int n2 = Convert.ToInt32(Console.ReadLine());

                    Random rand = new Random();
                    int Generic = rand.Next(n1, n2+1);
                    Console.WriteLine(Generic);
                }

                else if (Check.ToLower() == "restart")
                {
                    BIOS("Restart.");
                }

                else if (Check.ToLower() == "shutdown")
                {
                    Load("Shutdown.");
                    Environment.Exit(0);
                }

                else if (Check.ToLower() == "quit" || Check.ToLower() == "exit")
                {
                    Environment.Exit(0);
                }

                else
                {
                    Console.WriteLine("Command does not exist.");
                }
            }
        }

        static void Adminstrator()
        {
            Console.ResetColor();
            Console.Clear();

            bool Loop = true;
            while(Loop)
            {
                Console.Write("admin> ");
                string Check = Console.ReadLine();
                string Global = Check.ToLower();
                if (Check == "" || Check == " " || Check.Contains("  "))
                {
                    continue;
                }

                else if (Check.ToLower() == "help")
                {
                    string[] HelpCenter = {
                    "process   - Starts all the processes on your PC. This may cause some performance issues.",
                    "debug     - Debug the code which are not working properly.",
                    "diagxt    - Diagnose your PC's major or minor issues.",
                    "generate  - Generates a random number of your desired integer values.",
                    "terminate - Terminates all your PC process, Sometimes this may crash your PC.",
                    "quit      - Quits Admin system.",
                    "lock      - Asks for password at PC start-up.",
                    "format    - Formats your PC for better performance.",
                    "ran       - Displays machine specific properties and configuration."
                    };
                    Console.WriteLine("Type 'help' to get information of all the commands.");
                    Array.Sort(HelpCenter);
                    for (int i = 0; i < HelpCenter.Length; i++)
                    {
                        Console.WriteLine(HelpCenter[i]);
                    }
                }

                else if (Check.ToLower() == "ran")
                {
                    CommandPrompt("systeminfo");
                }

                else if (Check.ToLower() == "format")
                {
                    Console.WriteLine("Formatting.");
                    Thread.Sleep(5000);
                    Console.WriteLine("Formatted successfully.");
                }

                else if (Check.ToLower() == "lock")
                {
                    LockSYS();
                }

                else if (Check.ToLower() == "process")
                {
                    Thread.Sleep(5000);
                    Console.WriteLine("Processed.");
                }

                else if (Check.ToLower() == "debug")
                {
                    Console.WriteLine("Debugging.");
                    Thread.Sleep(20000);
                    Console.WriteLine("Debugging successfully completed!");
                }

                else if (Check.ToLower() == "diagxt")
                {
                    Console.WriteLine("Diagnosting your PC.");
                    Thread.Sleep(3000);
                    Console.WriteLine("This may take few seconds.");
                    Thread.Sleep(70000);
                    Console.WriteLine("Your PC was Diagnosed successfully.");
                }

                else if (Global.StartsWith("generate"))
                {
                    if (Global.Contains("uniform"))
                    {
                        Random Ran = new Random();
                        double Generic = Ran.NextDouble();
                        Console.WriteLine(Generic);
                    }

                    else if (Global.Contains("random"))
                    {
                        Console.WriteLine("Generate a random number");
                        Console.Write("Enter you first number: ");
                        int n1 = Convert.ToInt32(Console.ReadLine());

                        Console.Write("Enter you second number: ");
                        int n2 = Convert.ToInt32(Console.ReadLine());

                        Random rand = new Random();
                        int Generic = rand.Next(n1, n2+1);
                        Console.WriteLine(Generic);
                    }

                    else
                    {
                        Console.WriteLine("Command does not exist.");
                    }
                }

                else if (Check.ToLower() == "terminate")
                {
                    Random Ran = new Random();
                    int Generic = Ran.Next(0, 2);
                    if (Generic == 0)
                    {
                        CrashSys();
                    }

                    else
                    {
                        Console.WriteLine("Terminating all your PC processes.");
                        Thread.Sleep(10000);
                        Console.WriteLine("All PC processes was terminated successfully.");
                    }
                }

                else if (Check.ToLower() == "quit" || Check.ToLower() == "exit")
                {
                    break;
                }

                else
                {
                    Console.WriteLine("Command does not exist.");
                }
            }
        }

        static void Builder_Txt(string txt)
        {
            while (true)
            {
                string Data = Console.ReadLine();
                if (Data == "--builder<quit>")
                {
                    break;
                }
                File.AppendAllText(txt, $"{Data}\n");
            }
        }

        static void LockSYS()
        {
            Console.Clear();
            Console.Write("Set Password: ");
            string pinLock = Console.ReadLine();
            Console.Clear();
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

        static void CrashSys()
        {
            Console.WriteLine("Terminating all your PC processes.");
            Thread.Sleep(10000);
            Environment.Exit(0);
        }

        static void CommandPrompt(string Commands)
        {
            ProcessStartInfo start = new ProcessStartInfo("cmd.exe", $"/c {Commands}");
            Process.Start(start);
        }

        static void LockPC()
        {
            Console.Clear();
            Console.Write("Set Password: ");
            string pinLock = Console.ReadLine();
            Console.Clear();

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

        static void Update()
        {
            Random Ran = new Random();
            int Generic = Ran.Next(0, 18);
            if (Generic == 17)
            {
                Console.WriteLine("");
                Console.WriteLine("Updates are available!");
                Console.Write("Continue.");
                Console.ReadKey();
                Console.WriteLine("");

                Console.Clear();

                Console.WriteLine("Connecting.");
                Thread.Sleep(7000);
                Console.WriteLine("Connected.");
                Console.WriteLine("[]\n");

                Console.WriteLine("Downloading.");
                Thread.Sleep(7000);
                Console.WriteLine("72500 files Downloaded.");
                Console.WriteLine("[]\n");

                Console.WriteLine("Verifying.");
                Thread.Sleep(7000);
                Console.WriteLine("72500 files Verified.");
                Console.WriteLine("[]\n");

                Console.WriteLine("Extracting.");
                Thread.Sleep(7000);
                Console.WriteLine("107005 files Extracted.");
                Console.WriteLine("[]\n");

                Console.WriteLine("Installing.");
                Thread.Sleep(7000);
                Console.WriteLine("10500 files Installed.");
                Console.WriteLine("[]\n");

                Console.WriteLine("Updating.");
                Thread.Sleep(7000);

                if (File.Exists("updateinfo.log"))
                {
                    File.AppendAllText("updateinfo.log", $"This PC was updated at {DateTime.Now.Date}, {DateTime.Now.TimeOfDay}\n");
                }

                else
                {
                    File.WriteAllText("updateinfo.log", $"This PC was updated at {DateTime.Now.Date}, {DateTime.Now.TimeOfDay}\n");
                }

                Console.WriteLine("Your PC is Updated.");
                Console.WriteLine("[]\n");
                Console.Write("Restart.");
                Console.ReadKey();

                BIOS("Restart.");
            }

            else
            {
                Console.WriteLine("");
                Console.WriteLine("You are Up-To-Date.");
                Console.Write("Continue.");
                Console.ReadKey();
                Console.WriteLine("");
            }
        }

        static void Scan()
        {
            Load("Scaning.");
            Console.WriteLine("Scaning completed successfully!");
            Thread.Sleep(3000);
            Console.WriteLine("Generating report.");
            Thread.Sleep(1000);
            Random Ran = new Random();
            int Generic = Ran.Next(0, 18);
            if (Generic == 17)
            {
                Console.WriteLine("Your PC ran into problem =(");
            }

            else
            {
                Console.WriteLine("Your pc is working fine.");
                Console.Write("Continue.");
                Console.ReadKey();
                Console.WriteLine("");
            }
        }

        static void BIOS(string data)
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

            if (File.Exists("BOOT.log"))
            {
                File.AppendAllText("BOOT.log", "");
            }

            else
            {
                File.WriteAllText("BOOT.log", "BOOT_LOG\n");
            }

            Random Ran = new Random();
            int Generic = Ran.Next(0, 18);
            if (Generic == 17)
            {
                Timer();

                File.AppendAllText("BOOT.log", $"AOs crashed with a BSOD at {DateTime.Now.Date}, {DateTime.Now.TimeOfDay}\n");
                Console.Title = "Error 0x001.";
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.Write("Error 0x001.");
                Console.ReadKey();
                Environment.Exit(0);
            }

            else
            {
                File.AppendAllText("BOOT.log", $"AOs booted at {DateTime.Now.Date}, {DateTime.Now.TimeOfDay}\n");
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
                    Load(data);
                    Sys();
                }

                else
                {
                    Load(data);
                    Sys();
                }
            }
        }

        static void Load(string loading)
        {
            Timer();
            Console.Write(loading);
            Console.ReadKey();            
        }

        static void Timer()
        {
            for (int i = 0; i <= 100; i++)
            {
                Console.WriteLine(i);
                Thread.Sleep(10); // As miliseconds are counted.
                Console.Clear();
            }
        }
    }
}
