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
                    "calander - Displays current time and date."
                    };
                    Console.WriteLine("Type 'help' to get information of all the commands.");
                    Array.Sort(HelpCenter);
                    for (int i = 0; i < HelpCenter.Length; i++)
                    {
                        Console.WriteLine(HelpCenter[i]);
                    }
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

                else if (Check.StartsWith("shout "))
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
                Console.WriteLine("Updates are available!");
                Console.Write("Continue.");
                Console.ReadKey();
                Console.WriteLine("");

                Console.Clear();

                Console.WriteLine("Downloading.");
                Thread.Sleep(7000);

                Console.WriteLine("Verifying.");
                Thread.Sleep(7000);

                Console.WriteLine("Installing.");
                Thread.Sleep(7000);

                Console.WriteLine("Updating.");
                Thread.Sleep(7000);

                Console.WriteLine("You are Up-To-Date.");
                Console.Write("Restart.");
                Console.ReadKey();

                BIOS("Restart.");
            }

            else
            {
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
            Random Ran = new Random();
            int Generic = Ran.Next(0, 18);
            if (Generic == 17)
            {
                Timer();
                Console.Title = "Error 0x001.";
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.Write("Error 0x001.");
                Console.ReadKey();
                Environment.Exit(0);
            }

            else
            {
                Load(data);
                Sys();
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
