partial class EntryPoint
{
    // Run AOs app, an AOs script or execute based command-line argument.
    private void Startup()
    {
        // Change the title of the window to AOs.
        Features.ChangeTitle(["AOs"]);

        // Init argument parser
        var parser = new Argparse("AOs", Obsidian.about_AOs, Error.UnrecognizedArgs);
        parser.Add(["-h", "--help"], "Display all supported arguments", is_flag: true);
        parser.Add(["-a", "--admin"], "Run as administrator", is_flag: true);
        parser.Add(["-c", "--cmd"], "Program passed in as string");
        var parsed_args = parser.Parse(args);

        /*
        ------------------------------------------------------
        ------------------ Run AOs normally ------------------
        ------------------------------------------------------
        */

        // Run normally AOs if no arguments are passed.
        if (parsed_args.Count == 0)
        {
            // Clear the console to give a fresh look.
            this.AOs.ClearConsole();
            // Get all the scripts that are going to run at the start of AOs.
            Dictionary<string, string> startup_apps = ReadAllStartupApps();

            foreach (var contents in startup_apps)
            {
                TerminalColor.Print($"> ", ConsoleColor.DarkGray, false);
                TerminalColor.Print($"{contents.Key}", ConsoleColor.White); // contents.Key contains the name of that file
                RunAOsScript(contents.Value); // contents.Value contains the full absolute path of the file.
            }

            // After running all the startup apps, run AOs normally.
            Execute();
        }

        /*
        ------------------------------------------------------
        ---------------- Run command-line args ---------------
        ------------------------------------------------------
        */

        // If arguments are passed, in that case run do according to the options provided in Argparse.
        foreach (var arg in parsed_args)
        {
            // Help if the user wants it.
            if (Argparse.IsAskingForHelp(arg.Names))
                parser.PrintHelp();

            else if (arg.Names.Contains("-a"))
            {
                // Run AOs as administrator. Useful for some cases such as 'sfc /scannow' and more..
                string AOsBinaryFilepath = Obsidian.AOs_binary_path;
                SystemUtils.StartApp(AOsBinaryFilepath, is_admin: true);
                break;
            }

            // Execute an AOs command directly from the command-line without running AOs itself.
            else if (arg.Names.Contains("-c"))
            {
                // If the command is provided to execute then throw an error
                // suggesting that an argument is expected for this particular flag to work.
                if (arg.Value == null || Utils.String.IsEmpty(arg.Value))
                {
                    new Error($"Argument expected for the {string.Join(", ", arg.Names)} option.", "boot error");
                    Environment.Exit(1);
                }

                // Execute the command if provided
                Execute(arg.Value);
            }

            else if (arg.Names.First().ToLower().EndsWith(".aos"))
            {
                // Run an AOs script
                RunAOsScript(arg.Names.First());

                // Break, it's useless to continue with rest of the arguments.
                // All the arguments were already used by the AOs script as command-line args,
                // so if I don't break here then it will continue to loop through all those args
                // and throw error since those args are not defined in the Argparse obj already.
                break;
            }

            else
            {
                // Throw error if a file with another extention is passed.
                new Error($"{arg.Names.First()}: File format not recognized. File must have '.aos' extension", "filesystem i/o error");
                Environment.Exit(1);
            }
        }
    }

    // Run AOs script
    // The the name of the AOs script that is going to be executed.
    private void RunAOsScript(string filename)
    {
        // Throw error and exit the program as a whole if that file does not exist or cannot be located.
        if (!File.Exists(filename))
        {
            new Error($"Can't open file '{filename}': No such file or directory", "filesystem i/o error");
            Environment.Exit(1);
        }

        // Read all lines in that file and execute them one by one.
        string[] lines = FileIO.FileSystem.ReadAllLines(filename, FileStream: true);
        DateTime LastModified = File.GetLastWriteTimeUtc(filename);
        int pause_idx = ExecuteLines(lines, 0);

        while(pause_idx != -1)
        {
            Thread.Sleep(250); // Wait for 250ms before checking for file changes
            DateTime NewLastModified = File.GetLastWriteTimeUtc(filename);

            if (NewLastModified != LastModified)
            {
                LastModified = NewLastModified;
                lines = FileIO.FileSystem.ReadAllLines(filename, FileStream: true);
                pause_idx = ExecuteLines(lines, pause_idx);
            }
        }
    }

    private int ExecuteLines(string[] lines, int pause_idx)
    {
        // Loop through the entire file line-by-line.
        for (int i = pause_idx; i < lines.Length; i++)
        {
            // Get the current line.
            string line = lines[i];

            // If a line is "." then pause and wait until the file is updated and "." is moved or removed. AKA hot reload the file.
            //* I'm implementing this feature for WINTER (https://github.com/Light-Lens/WINTER)
            if (line.Trim() == ".")
                return i;

            ExecuteLine(line);
        }

        return -1;
    }

    private void ExecuteLine(string line)
    {
        // If a line is not a fullstop, then tokenize the line and execute it one-by-one.
        List<Lexer.Tokenizer.Token> Tokens = new Lexer.Tokenizer(line).tokens;
        List<string> NewTokens = [];

        foreach (Lexer.Tokenizer.Token Tok in Tokens)
        {
            // Check if a token has '$' prefix, if yes then replace that '${any_number}' with the argument value
            // which is on that `any_number` index of all command-line arguments
            string Name = Tok.Name;
            if (Name.StartsWith("$") && Tok.Type == Lexer.Tokenizer.TokenType.IDENTIFIER)
            {
                Name = int.TryParse(Name[1..], out int arg_index) && arg_index < this.args.Length ? this.args[arg_index] : "";

                // If the argument contains a space for example: Name = "Hello world!",
                // then wrap that value around with string literals something like this: Name = "\"Hello world!\"",
                // this is to make sure that when the line will be executed no argument value will be missed.
                if (Name.Contains(' '))
                    Name = $"\"{Name}\"";
            }

            else if (Tok.Type == Lexer.Tokenizer.TokenType.STRING)
                Name = $"\"{Name}\"";

            NewTokens.Add(Name);
        }

        // Join and execute the new list of tokens that are modified to account for command-line arguments that were passed for the script.
        Execute(string.Join("", NewTokens));
    }
}
