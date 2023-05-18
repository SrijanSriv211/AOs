using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

class Obsidian
{
    public string Version = String.Format("AOs 2023 [Version 2.5]");
    public string[] PromptPreset = {"-r"};

    public static dynamic rootDir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\').TrimEnd('/');

    private string Title = "AOs";
    private string Prompt = "";

    public Obsidian(string title="AOs", string prompt="$ ")
    {
        Title = title;
        Prompt = Collection.String.IsEmpty(prompt) ? SetPrompt(PromptPreset) : prompt;
    }

    public Dictionary<string, string[]> TakeInput(string input = "")
    {
        Dictionary<string, string[]> output = new Dictionary<string, string[]>();
        string CMD = input.Trim();
        if (Collection.String.IsEmpty(CMD))
        {
            new TerminalColor(Prompt, ConsoleColor.White, false);
            CMD = Console.ReadLine().Trim();

            if (Collection.String.IsEmpty(CMD))
                return new Dictionary<string, string[]>(); // return (cmd: "", args: new string[0])

            if (CMD[0] == '_')
                CMD = CMD.Substring(1).Trim();
        }

        // Set history.
        History.Set(CMD);

        // Some lexer stuff.
        List<List<string>> ListOfToks = new Lexer(CMD).Tokens;
        foreach (var Toks in ListOfToks)
        {
            // Split the Toks into a cmd and Args variable and array respectively.
            string input_cmd = Collection.Array.Trim(Toks.ToArray()).FirstOrDefault() ?? "";
            string[] input_args = Collection.Array.Trim(Collection.Array.Reduce(Toks.ToArray())) ?? new string[0];
            if (!Collection.Array.IsEmpty(input_args) && input_args.FirstOrDefault() == input_cmd)
                input_args = Collection.Array.Trim(input_args.Skip(1).ToArray());

            output.Add(input_cmd, input_args);
        }

        return output;
    }

    public void ClearConsole()
    {
        Console.Clear();
        new TerminalColor(Version, ConsoleColor.Yellow);
    }

    public void Entrypoint(bool clear=true)
    {
        Console.Title = Title;
        if (clear)
            ClearConsole();

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

    public string SetPrompt(string[] flags, string default_prompt="$ ")
    {
        string PromptMessage = "";
        foreach (string i in flags)
        {
            if (Collection.String.IsString(i)) PromptMessage += Collection.String.Strings(i);
            else if (Argparse.IsAskingForHelp(i.ToLower()))
            {
                string[] PromptHelpCenter = {
                    "Specifies a new command prompt.",
                    "Usage: prompt [Option]",
                    "",
                    "Options:",
                    "-h, --help      -> Displays the help message.",
                    "-r, --reset     -> $ (dollar sign, reset the prompt)",
                    "-s, --space     -> (space)",
                    "-v, --version   -> Current AOs version",
                    "-t, --time      -> Current time",
                    "-d, --date      -> Current date",
                    "-p, --path      -> Current path",
                    "-n, --drive     -> Current drive"
                };

                Console.WriteLine(string.Join("\n", PromptHelpCenter));
                PromptMessage = default_prompt;
                break;
            }

            else if (i.ToLower() == "-r" || i.ToLower() == "--reset" || i.ToLower() == "--restore" || i.ToLower() == "--default")
            {
                PromptMessage = "$ ";
                break;
            }

            else if (i.ToLower() == "-v") PromptMessage += new Obsidian().Version;
            else if (i.ToLower() == "-s") PromptMessage += " ";
            else if (i.ToLower() == "-t") PromptMessage += DateTime.Now.ToString("HH:mm:ss");
            else if (i.ToLower() == "-d") PromptMessage += DateTime.Now.ToString("dd-MM-yyyy");
            else if (i.ToLower() == "-p") PromptMessage += Directory.GetCurrentDirectory();
            else if (i.ToLower() == "-n") PromptMessage += Path.GetPathRoot(Environment.SystemDirectory);
            else if (i.StartsWith("-"))
            {
                PromptMessage = default_prompt;
                Error.UnrecognizedArgs(i);
                break;
            }

            else PromptMessage += i;
        }

        return PromptMessage;
    }
}

class History
{
    public static void Set(string cmd)
    {
        string CurrentTime = DateTime.Now.ToString("[dd-MM-yyyy HH:mm:ss]");
        File.AppendAllText($"{Obsidian.rootDir}\\Files.x72\\root\\.history", $"{CurrentTime}, '{cmd}'");
    }

    public static void Get()
    {
        Console.WriteLine(File.ReadAllText($"{Obsidian.rootDir}\\Files.x72\\root\\.history"));
    }

    public static void Clear()
    {
        File.Delete($"{Obsidian.rootDir}\\Files.x72\\root\\.history");
    }
}

class TerminalColor
{
    public TerminalColor(string details, ConsoleColor Color, bool isNewLine=true)
    {
        var ForegroundColor = Console.ForegroundColor;
        Console.ForegroundColor = Color;

        if (isNewLine)
            Console.WriteLine(details);

        else
            Console.Write(details);

        Console.ForegroundColor = ForegroundColor;
    }
}
