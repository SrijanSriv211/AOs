partial class Obsidian
{
    public string version;
    public string[] prompt_preset;
    public ConsoleColor current_foreground_color = original_foreground_color;

    private string prompt;

    public Obsidian()
    {
        this.version = $"AOs 2023 [Version {version_no}]";
        this.prompt = "$ ";
        this.prompt_preset = new string[0];
    }

    public List<(string cmd, string[] args)> TakeInput(string input="")
    {
        List<(string cmd, string[] args)> output = new();
        string CMD = input.Trim();

        Console.ForegroundColor = current_foreground_color;
        if (Utils.String.IsEmpty(CMD))
        {
            SetPrompt(this.prompt_preset);
            new TerminalColor(this.prompt, ConsoleColor.White, false);

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
            if (Utils.String.IsEmpty(Toks.FirstOrDefault()))
                continue;

            // Split the Toks into a cmd and Args variable and array respectively.
            string input_cmd = Utils.String.Strings(Toks.FirstOrDefault());
            string[] input_args = Utils.Array.Trim(Toks.Skip(1).ToArray());

            // Add input_cmd & input_args to output.
            output.Add((input_cmd, input_args));
        }

        return output;
    }
}

// partial class Obsidian
// {
//     public Obsidian(string Title="AOs", string Prompt="$ ")
//     {
//         this.Title = is_admin ? $"{Title} (Administrator)" : Title;
//         this.Prompt = Utils.String.IsEmpty(Prompt) ? SetPrompt(PromptPreset) : Prompt;
//     }

//     public List<(string cmd, string[] args)> TakeInput(string input="")
//     {
//         List<(string cmd, string[] args)> output = new();
//         string CMD = input.Trim();

//         Console.ForegroundColor = Default_color;
//         this.Prompt = SetPrompt(PromptPreset);

//         if (Utils.String.IsEmpty(CMD))
//         {
//             new TerminalColor(this.Prompt, ConsoleColor.White, false);

//             CMD = Console.ReadLine().Trim();

//             if (Utils.String.IsEmpty(CMD))
//                 return new List<(string cmd, string[] args)>(); // (cmd: "", args: new string[0])
//         }

//         if (CMD.First() == '_')
//             CMD = CMD.Substring(1).Trim();

//         // Set history.
//         History.Set(CMD);

//         // Some lexer stuff.
//         List<string[]> ListOfToks = new Lexer(CMD).Tokens;
//         foreach (string[] Toks in ListOfToks)
//         {
//             if (Utils.String.IsEmpty(Toks.FirstOrDefault()))
//                 continue;

//             // Split the Toks into a cmd and Args variable and array respectively.
//             string input_cmd = Utils.String.Strings(Toks.FirstOrDefault());
//             string[] input_args = Utils.Array.Trim(Toks.Skip(1).ToArray());

//             // Add input_cmd & input_args to output.
//             output.Add((input_cmd, input_args));
//         }

//         return output;
//     }

//     public void ClearConsole()
//     {
//         sys_utils.CommandPrompt("cls"); //* This is better solution over the below one.
//         /*https://learn.microsoft.com/en-us/windows/console/clearing-the-screen
//         https://stackoverflow.com/a/75492171/18121288*/
//         // Console.Clear();
//         // Console.Write("\x1b[3J");

//         new TerminalColor(this.Version, ConsoleColor.Yellow, false);
//         new TerminalColor($"\t({Environment.GetEnvironmentVariable("username")})", ConsoleColor.White);
//     }

//     public void PrintVersion()
//     {
//         Console.WriteLine(this.Version);
//     }

//     public void Credits()
//     {
//         string[] CreditCenter = {
//              "_________ Team AOS ________",
//              "Author     -> Srijan Srivastava",
//              "Github     -> github.com/Light-Lens/AOs",
//              "Contact    -> QCoreNest@gmail.com",
//              "",
//              "____________________ Note (For Developers) ____________________",
//             $"|| {AOsDesc}",
//              "|| All code is licensed under an MIT license.",
//              "|| This allows you to re-use the code freely, remixed in both commercial and non-commercial projects.",
//              "|| The only requirement is to include the same license when distributing.",
//              "",
//              "____________________ Note (For All) ____________________",
//              "|| Warning - Do not Delete any File",
//              "|| or it may Cause Corruption",
//              "|| and may lead to instability.",
//              "",
//              "Type 'help' to get information about all supported command."
//         };

//         new TerminalColor(string.Join("\n", CreditCenter), ConsoleColor.White);
//     }

//     public void About()
//     {
//         new TerminalColor(AOsDesc, ConsoleColor.White);
//         new TerminalColor("For more information go to ", ConsoleColor.DarkGray, false);
//         new TerminalColor(AOsRepo, ConsoleColor.Cyan);
//     }

//     public string SetPrompt(string[] flags, string default_prompt="$ ")
//     {
//         if (Utils.Array.IsEmpty(flags))
//             return default_prompt;

//         var parser = new Argparse("prompt", "Specifies a new command prompt.");

//         parser.Add(new string[] {"-h", "--help"}, "Display all supported arguments", is_flag: true);
//         parser.Add(new string[] {"-r", "--reset", "--restore", "--default"}, "$ (dollar sign, reset the prompt)", is_flag: true);

//         parser.Add(new string[] {"-u", "--username"}, "%username%", is_flag: true);
//         parser.Add(new string[] {"-s", "--space"}, "(space)", is_flag: true);
//         parser.Add(new string[] {"-b", "--backspace"}, "(backspace)", is_flag: true);
//         parser.Add(new string[] {"-v", "--version"}, "Current AOs version", is_flag: true);

//         parser.Add(new string[] {"-t", "--time"}, "Current time", is_flag: true);
//         parser.Add(new string[] {"-d", "--date"}, "Current date", is_flag: true);
//         parser.Add(new string[] {"-p", "--path"}, "Current path", is_flag: true);
//         parser.Add(new string[] {"-n", "--drive"}, "Current drive", is_flag: true);

//         var parsed_args = parser.Parse(flags);
//         string new_prompt = "";

//         foreach (var arg in parsed_args)
//         {
//             if (Argparse.IsAskingForHelp(arg.Names))
//             {
//                 parser.PrintHelp();
//                 return default_prompt;
//             }

//             else if (arg.Names.Contains("-r"))
//                 return default_prompt;

//             else if (arg.Names.Contains("-v"))
//                 new_prompt += new Obsidian().Version;

//             else if (arg.Names.Contains("-s"))
//                 new_prompt += " ";

//             else if (arg.Names.Contains("-b"))
//                 new_prompt += "\b \b";

//             else if (arg.Names.Contains("-t"))
//                 new_prompt += DateTime.Now.ToString("HH:mm:ss");

//             else if (arg.Names.Contains("-d"))
//                 new_prompt += DateTime.Now.ToString("dd-MM-yyyy");

//             else if (arg.Names.Contains("-p"))
//                 new_prompt += Directory.GetCurrentDirectory();

//             else if (arg.Names.Contains("-n"))
//                 new_prompt += Path.GetPathRoot(Environment.SystemDirectory);

//             else if (arg.Names.Contains("-u"))
//                 new_prompt += Environment.GetEnvironmentVariable("username");

//             else if (arg.Names.Any(name => name.StartsWith("-")))
//             {
//                 Error.UnrecognizedArgs(arg.Names);
//                 return default_prompt;
//             }

//             else
//                 new_prompt += arg.Names.First();
//         }

//         return new_prompt;
//     }
// }
