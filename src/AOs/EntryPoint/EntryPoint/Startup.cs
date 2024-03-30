partial class EntryPoint
{
    // Run AOs app, an AOs script or execute based command-line argument.
    private void Startup()
    {
        // Change the title of the window to AOs.
        this.features.ChangeTitle(["AOs"]);

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
            Dictionary<string, string[]> startup_apps = ReadAllStartupApps(LoadStartlist());

            foreach (var contents in startup_apps)
            {
                TerminalColor.Print($"> ", ConsoleColor.DarkGray, false);
                TerminalColor.Print($"{contents.Key}", ConsoleColor.White); // contents.Key is the name of that file
                Execute(contents.Value); // contents.Value contains all the lines that are going to be executed.
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
                this.sys_utils.StartApp(AOsBinaryFilepath, is_admin: true);
                break;
            }

            // Execute an AOs command directly from the command-line without running AOs itself.
            else if (arg.Names.Contains("-c"))
            {
                // If the command is provided to execute then throw an error
                // suggesting that an argument is expected for this particular flag to work.
                if (arg.Value == null || Utils.String.IsEmpty(arg.Value))
                {
                    _ = new Error($"Argument expected for the {string.Join(", ", arg.Names)} option.", "boot error");
                    Environment.Exit(1);
                }

                // Execute the command if provided
                Execute(arg.Value);
            }

            else if (arg.Names.First().ToLower().EndsWith(".aos"))
            {
                // Run an AOs script
                RunAOsScript();

                // Break, it's useless to continue with rest of the arguments.
                // All the arguments were already used by the AOs script as command-line args,
                // so if I don't break here then it will continue to loop through all those args
                // and throw error since those args are not defined in the Argparse obj already.
                break;
            }

            else
            {
                // Throw error if a file with another extention is passed.
                _ = new Error($"{arg.Names.First()}: File format not recognized. File must have '.aos' extension", "filesystem i/o error");
                Environment.Exit(1);
            }
        }
    }

    // Run AOs script
    private void RunAOsScript()
    {
        // The the name of the AOs script that is going to be executed.
        string filename = this.args.First();

        // Throw error and exit the program as a whole if that file does not exist or cannot be located.
        if (!File.Exists(filename))
        {
            _ = new Error($"Can't open file '{filename}': No such file or directory", "filesystem i/o error");
            Environment.Exit(1);
        }

        // Read all lines in that file and execute them one by one.
        foreach (string line in FileIO.FileSystem.ReadAllLines(filename))
        {
            // Tokenize the line
            List<string[]> ListOfToks = new Lexer(line).Tokens;

            foreach (string[] Toks in ListOfToks)
            {
                // Loop through all tokens
                for (int k = 0; k < Toks.Length; k++)
                {
                    // Check if a token has '$' in it's prefix, if yes then,
                    // replace that '${any_number}' with the argument value
                    // which is on that `any_number` index of all command-line arguments
                    if (Toks[k].StartsWith("$"))
                    {
                        Toks[k] = int.TryParse(Toks[k][1..], out int arg_index) && arg_index < this.args.Length ? this.args[arg_index] : "";

                        // If the argument contains a space for example: Toks[k] = "Hello world!",
                        // then wrap that value around with string literals something like this: Toks[k] = "\"Hello world!\"",
                        // this is to make sure that when the line will be executed no argument value will be missed.
                        if (Toks[k].Contains(' '))
                            Toks[k] = $"\"{Toks[k]}\"";
                    }
                }

                // Join and execute the new list of tokens that are modified to account for
                // command-line arguments that were passed for the script.
                Execute(string.Join("", Toks));
            }
        }
    }
}
