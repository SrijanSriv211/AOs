using System.Text;
using System.Security.Principal;

Console.OutputEncoding = Encoding.UTF8;
bool isAdmin = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
Obsidian AOs = isAdmin ? new Obsidian("AOs (Administrator)") : new Obsidian();
Startup();

void Startup()
{
    string[] argv = Collection.Array.Filter(args);
    var parser = new Argparse("AOs", "A Command-line utility for improved efficiency and productivity.");
    parser.Add(new string[] {"-h", "--help"}, "Display all supported arguments.", is_flag: true);
    parser.Add(new string[] {"-c", "--cmd"}, "Program passed in as string.");

    var parsed_args = parser.Parse(argv);

    // If no arguments are passed.
    if (Collection.Array.IsEmpty(argv))
    {
        string startlist_path = Path.Combine(Obsidian.rootDir, "Files.x72\\root\\StartUp\\.startlist");
        bool isEmpty = Collection.String.IsEmpty( FileIO.FileSystem.ReadAllText(startlist_path) );

        if (File.Exists(startlist_path) && !isEmpty)
        {
            AOs.Entrypoint(false);
            foreach (string app in File.ReadLines(startlist_path))
            {
                // Break the loop if "." is detected, as all apps after the dot are marked as disabled.
                if (app == ".")
                    break;

                else if (app.EndsWith(".aos"))
                {
                    foreach (string current_line in FileIO.FileSystem.ReadAllLines( Path.Combine(Path.GetDirectoryName(startlist_path), app) ))
                        run(AOs, AOs.TakeInput(current_line));
                }
            }
        }

        else
            AOs.Entrypoint();
    }

    // Execute AOs on the basis of the command-line arguments passed.
    else
    {
        foreach (var arg in parsed_args)
        {
            if (Argparse.IsAskingForHelp(arg.names))
            {
                parser.PrintHelp();
                return;
            }

            else if (arg.names.Contains("-c"))
            {
                if (!Collection.String.IsEmpty(arg.value))
                {
                    AOs.Entrypoint(false);
                    run(AOs, AOs.TakeInput(arg.value));
                }

                return;
            }

            else
            {
                foreach (string filename in arg.names)
                {
                    if (!filename.EndsWith(".aos"))
                    {
                        new Error($"{filename}: File format not recognized.");
                        return;
                    }

                    else if (!File.Exists(filename))
                    {
                        new Error($"{filename}: No such file or directory.");
                        return;
                    }

                    else
                    {
                        foreach (string current_line in FileIO.FileSystem.ReadAllLines(filename))
                            run(AOs, AOs.TakeInput(current_line));
                    }
                }
            }
        }
    }

    while (true)
        run(AOs, AOs.TakeInput());
}

void run(Obsidian AOs, List<(string cmd, string[] args)> input)
{
    try
    {
        main(AOs, input);
    }

    catch (Exception err)
    {
        new Error(err.Message);
    }
}

void main(Obsidian AOs, List<(string cmd, string[] args)> input)
{
    var parser = new Argparse("AOs", "A Command-line utility for improved efficiency and productivity.");
    parser.Add(new string[]{ "_cls", "_clear" }, "Clear the screen", is_flag: true, method: AOs.ClearConsole);
    parser.Add(new string[]{ "_version", "_ver", "-v" }, "Displays the AOs version.", is_flag: true, method: AOs.PrintVersion);
    parser.Add(new string[]{ "_about", "_info" }, "About AOs", is_flag: true, method: parser.PrintHelp);
    parser.Add(new string[]{ "_shutdown" }, "Shutdown the host machine", is_flag: true, method: Features.Shutdown);
    parser.Add(new string[]{ "_restart" }, "Restart the host machine", is_flag: true, method: Features.Restart);
    parser.Add(new string[]{ "_quit", "_exit" }, "Exit AOs", is_flag: true, method: Features.Exit);
    parser.Add(new string[]{ "_reload", "_refresh" }, "Restart AOs", is_flag: true, method: Features.Refresh);
    parser.Add(new string[]{ "_credits" }, "Credits for AOs", is_flag: true, method: AOs.Credits);

    parser.Add(new string[]{ "_shout", "_echo" }, "Displays messages", default_value: "", is_flag: false, method: Features.Shout);
    parser.Add(new string[]{ "_history" }, "Displays the history of Commands.", default_value: "", is_flag: false, method: Features.GetSetHistory);

    foreach (var i in input)
    {
        if (Collection.String.IsEmpty(i.cmd))
            continue;

        if (i.cmd.ToLower() == "help" || Argparse.IsAskingForHelp(i.cmd))
            parser.GetHelp(i.args.FirstOrDefault(""));

        else if (i.cmd == "AOs1000")
            Console.WriteLine("AOs1000!\nCONGRATULATIONS! For hitting 1000 LINES OF CODE in AOs 1.3!\nIt was the first program to ever reach these many LINES OF CODE!");

        else if (i.cmd == "∞" || double.TryParse(i.cmd, out double _) || Collection.String.IsString(i.cmd))
            Console.WriteLine(Collection.String.Strings(i.cmd));

        else
        {
            string cmd = i.cmd.ToLower();
            string[] cmd_to_be_parsed = new string[]{ $"_{cmd}" }.Concat(i.args).ToArray();
            var parsed_args = parser.Parse(cmd_to_be_parsed, error_func: (arg) => Error.Command(arg));

            foreach (var arg in parsed_args)
            {
                Console.WriteLine($"[({string.Join(", ", arg.names)}), {arg.value}, is_flag: {arg.is_flag}]");
                if (arg.is_flag)
                {
                    var action = arg.method as Action; // Cast the stored delegate to Action
                    action?.Invoke(); // Invoke the delegate with no arguments
                }

                else
                {
                    var action = arg.method as Action<string[]>; // Cast the stored delegate to Action<string[]>
                    action?.Invoke(i.args); // Invoke the delegate with the provided arguments (i.args)
                }
            }
        }
    }

    // shout "Hello world!";1+3;"1+2";
}
