using System.Security.Principal;

class Obsidian
{
    public static string root_dir = AppDomain.CurrentDomain.BaseDirectory;
    public static string default_else_shell = "cmd.exe";
    public static bool is_admin = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
    public static ConsoleColor original_color_of_terminal = Console.ForegroundColor;

    public string Version = "AOs 2023 [Version 2.5]";
    public ConsoleColor Default_color { get; set; } = Console.ForegroundColor;

    private readonly string Title = "AOs";
    private readonly string Prompt = "$ ";

    public Obsidian(string Title="AOs", string Prompt="$ ")
    {
        this.Title = is_admin ? $"{Title} (Administrator)" : Title;
        this.Prompt = Utils.String.IsEmpty(Prompt) ? SetPrompt(new string[]{"-r"}) : Prompt;
    }

    public List<(string cmd, string[] args)> TakeInput(string input="")
    {
        List<(string cmd, string[] args)> output = new();
        string CMD = input.Trim();

        Console.ForegroundColor = Default_color;

        if (Utils.String.IsEmpty(CMD))
        {
            new TerminalColor(this.Prompt, ConsoleColor.White, false);

            CMD = Console.ReadLine().Trim();

            if (Utils.String.IsEmpty(CMD))
                return new List<(string cmd, string[] args)>(); // (cmd: "", args: new string[0])
        }

        if (CMD.First() == '_')
            CMD = CMD.Substring(1).Trim();

        // Set history.
        History.Set(CMD);

        // Some lexer stuff.
        List<string[]> ListOfToks = new Lexer(CMD).Tokens;
        foreach (string[] Toks in ListOfToks)
        {
            // Split the Toks into a cmd and Args variable and array respectively.
            string input_cmd = Utils.String.Strings(Toks.FirstOrDefault());
            string[] input_args = Utils.Array.Trim(Toks.Skip(1).ToArray());

            // Add input_cmd & input_args to output.
            if (!Utils.String.IsEmpty(input_cmd))
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

        EntryPoint.RootPackages();
        EntryPoint.AskPass();
    }

    public void PrintVersion()
    {
        Console.WriteLine(Version);
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
            "|| A Developer Command-line Tool Built for Developers by a Developer.",
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

    public void About()
    {
        new TerminalColor("A Developer Command-line Tool Built for Developers by a Developer.", ConsoleColor.White);
        new TerminalColor("For more information go to ", ConsoleColor.DarkGray, false);
        new TerminalColor("https://github.com/Light-Lens/AOs.", ConsoleColor.Cyan);
    }

    public string SetPrompt(string[] flags, string default_prompt="$ ")
    {
        if (Utils.Array.IsEmpty(flags))
            return default_prompt;

        var parser = new Argparse("prompt", "Specifies a new command prompt.");

        parser.Add(new string[] {"-h", "--help"}, "Display all supported arguments", is_flag: true);
        parser.Add(new string[] {"-r", "--reset", "--restore", "--default"}, "$ (dollar sign, reset the prompt)", is_flag: true);

        parser.Add(new string[] {"-s", "--space"}, "(space)", is_flag: true);
        parser.Add(new string[] {"-b", "--backspace"}, "(backspace)", is_flag: true);
        parser.Add(new string[] {"-v", "--version"}, "Current AOs version", is_flag: true);

        parser.Add(new string[] {"-t", "--time"}, "Current time", is_flag: true);
        parser.Add(new string[] {"-d", "--date"}, "Current date", is_flag: true);
        parser.Add(new string[] {"-p", "--path"}, "Current path", is_flag: true);
        parser.Add(new string[] {"-n", "--drive"}, "Current drive", is_flag: true);

        var parsed_args = parser.Parse(flags);
        string new_prompt = "";

        foreach (var arg in parsed_args)
        {
            if (Argparse.IsAskingForHelp(arg.Names))
            {
                parser.PrintHelp();
                return default_prompt;
            }

            else if (arg.Names.Contains("-r"))
                return default_prompt;

            else if (arg.Names.Contains("-v"))
                new_prompt += new Obsidian().Version;

            else if (arg.Names.Contains("-s"))
                new_prompt += " ";

            else if (arg.Names.Contains("-b"))
                new_prompt += "\b \b";

            else if (arg.Names.Contains("-t"))
                new_prompt += DateTime.Now.ToString("HH:mm:ss");

            else if (arg.Names.Contains("-d"))
                new_prompt += DateTime.Now.ToString("dd-MM-yyyy");

            else if (arg.Names.Contains("-p"))
                new_prompt += Directory.GetCurrentDirectory();

            else if (arg.Names.Contains("-n"))
                new_prompt += Path.GetPathRoot(Environment.SystemDirectory);

            else if (arg.Names.Any(name => name.StartsWith("-")))
            {
                Error.UnrecognizedArgs(arg.Names);
                return default_prompt;
            }

            else
                new_prompt += arg.Names.First();
        }

        return new_prompt;
    }
}
