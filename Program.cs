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
                    // code here.
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
                    ShutDown();
                }

                else if (Check.ToLower() == "quit")
                {
                    Environment.Exit(0);
                }

                else
                {
                    Console.WriteLine("Command does not exist.");
                }
            }
        }

        static void BIOS(string data)
        {
            Random Ran = new Random();
            int Generic = Ran.Next(0, 8);
            if (Generic == 0)
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

        static void ShutDown()
        {
            Timer();
            Console.Write("ShutDown.");
            Console.ReadKey();
            Environment.Exit(0);
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
