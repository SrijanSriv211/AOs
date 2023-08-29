class Obsidian
{
    public string Version = String.Format("AOs 2023 [Version 2.5]");

    public static string default_else_shell = "cmd.exe";
    public static dynamic rootDir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\').TrimEnd('/');

    private string Title = "AOs";
    private string Prompt = "";

    public Obsidian(string title="AOs", string prompt="$ ")
    {
        Title = title;
        Prompt = Collection.String.IsEmpty(prompt) ? SetPrompt(new string[]{"-r"}) : prompt;
    }

    public List<(string cmd, string[] args)> TakeInput(string input = "")
    {
        List<(string cmd, string[] args)> output = new List<(string cmd, string[] args)>();
        string CMD = input.Trim();

        if (Collection.String.IsEmpty(CMD))
        {
            new TerminalColor(Prompt, ConsoleColor.White, false);
            CMD = Console.ReadLine().Trim();

            if (Collection.String.IsEmpty(CMD))
                return new List<(string cmd, string[] args)>(); // (cmd: "", args: new string[0])

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
            string[] preprocess_toks = Collection.Array.Trim(Collection.Array.Reduce(Toks.ToArray()));
            string input_cmd = preprocess_toks.FirstOrDefault();
            string[] input_args = preprocess_toks.Skip(1).ToArray();

            // Parse input.
            output.Add((input_cmd, input_args));
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

        new TerminalColor(string.Join("\n", CreditCenter), ConsoleColor.White);
    }

    public string SetPrompt(string[] flags, string default_prompt="$ ")
    {
        var parser = new Argparse("prompt", "Specifies a new command prompt.");

        parser.Add(new string[] {"-h", "--help"}, "Display all supported arguments", is_flag: true);
        parser.Add(new string[] {"-r", "--reset", "--restore", "--default"}, "$ (dollar sign, reset the prompt)", is_flag: true);

        parser.Add(new string[] {"-s", "--space"}, "(space)", is_flag: true);
        parser.Add(new string[] {"-v", "--version"}, "Current AOs version", is_flag: true);

        parser.Add(new string[] {"-t", "--time"}, "Current time", is_flag: true);
        parser.Add(new string[] {"-d", "--date"}, "Current date", is_flag: true);
        parser.Add(new string[] {"-p", "--path"}, "Current path", is_flag: true);
        parser.Add(new string[] {"-n", "--drive"}, "Current drive", is_flag: true);

        var parsed_args = parser.Parse(flags);
        string new_prompt = string.Empty;

        if (Collection.Array.IsEmpty(flags))
            return default_prompt;

        else
        {
            foreach (var arg in parsed_args)
            {
                if (Argparse.IsAskingForHelp(arg.names))
                {
                    parser.PrintHelp();
                    return default_prompt;
                }

                else if (arg.names.Contains("-r"))
                    return default_prompt;

                else if (arg.names.Contains("-v"))
                    new_prompt += new Obsidian().Version;

                else if (arg.names.Contains("-s"))
                    new_prompt += " ";

                else if (arg.names.Contains("-t"))
                    new_prompt += DateTime.Now.ToString("HH:mm:ss");

                else if (arg.names.Contains("-d"))
                    new_prompt += DateTime.Now.ToString("dd-MM-yyyy");

                else if (arg.names.Contains("-p"))
                    new_prompt += Directory.GetCurrentDirectory();

                else if (arg.names.Contains("-n"))
                    new_prompt += Path.GetPathRoot(Environment.SystemDirectory);

                else if (arg.names.Any(name => name.StartsWith("-")))
                {
                    Error.UnrecognizedArgs(arg.names);
                    return default_prompt;
                }

                else
                    new_prompt += arg;
            }
        }

        return new_prompt;
    }
}

class History
{
    public static void Set(string cmd)
    {
        string CurrentTime = DateTime.Now.ToString("[dd-MM-yyyy HH:mm:ss]");
        FileIO.FileSystem.Write($"{Obsidian.rootDir}\\Files.x72\\root\\.history", $"{CurrentTime}\n'{cmd}'\n\n");
    }

    public static void Get()
    {
        Console.WriteLine(FileIO.FileSystem.ReadAllText($"{Obsidian.rootDir}\\Files.x72\\root\\.history"));
    }

    public static void Clear()
    {
        FileIO.FileSystem.Delete($"{Obsidian.rootDir}\\Files.x72\\root\\.history");
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
