using System;
using System.IO;
using System.Threading;

namespace AOs
{
    class Program
    {
        static void Main(string[] args)
        {
            if (BIOS("Starting up.")) System();
            else Environment.Exit(0);
        }

        static void System()
        {
            Console.Title = "AOs";
            Console.ResetColor();
            Console.Clear();

            string Prompt = "$ ";
            string SYSVersion = "AOs 2021 [Version 1.7.1]";

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(SYSVersion);
            Console.ResetColor();
            while (true)
            {
                Console.Write(Prompt);
                string Command = Console.ReadLine();
                if (Command == "" || Command == " " || Command.Contains("  ")) continue;
                else if (Command.ToLower() == "quit" || Command.ToLower() == "exit") Environment.Exit(0);
                else if (Command.ToLower() == "clear" || Command.ToLower() == "cls")
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(SYSVersion);
                    Console.ResetColor();
                }

                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\'{Command}\', Command does not exist.\n");
                    Console.ResetColor();
                }
            }
        }

        static bool BIOS(string Loadstatus)
        {
            int Errors = 0;
            string RootDir = Directory.GetCurrentDirectory();
            if (RootDir.Contains("AOs\\Files.x72")) Directory.SetCurrentDirectory("..");
            if (File.Exists("BOOT.log") == false) File.WriteAllText("BOOT.log", "BOOT_LOG\n");

            Console.WriteLine(Loadstatus);
            Console.WriteLine("This may take some time.\nPlease wait!\n");

            if (File.Exists("Config.set") == false) Errors++;
            for (int i = 0; i <= 100; i++)
            {
                Thread.Sleep(25);
                Console.Write("-");
                if (i >= 50)
                {
                    Console.WriteLine(">");
                    break;
                }
            }

            Console.Clear();
            Console.WriteLine($"{Loadstatus}\n");
            if (Errors == 0)
            {
                if (File.Exists("BOOT.log")) File.AppendAllText("BOOT.log", $"AOs booted at {DateTime.Now.ToString("[dd-MM-yyyy], [HH:mm:ss]")}\n");

                Console.WriteLine("No Startup Errors were Found!");
                Console.Write("Continue.");
                Console.ReadKey();
                return true;
            }

            else
            {
                if (File.Exists("BOOT.log")) File.AppendAllText("BOOT.log", $"AOs crashed at {DateTime.Now.ToString("[dd-MM-yyyy], [HH:mm:ss]")}\n");

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{Errors} Startup Errors were Found!");
                Console.ResetColor();
                Console.Write("Continue.");
                Console.ReadKey();
                return false;
            }
        }
    }
}
