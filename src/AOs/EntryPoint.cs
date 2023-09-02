using System.Security.Principal;

class EntryPoint
{
    private readonly Obsidian AOs;
    private readonly string[] args;
    private readonly Action<Obsidian, List<(string, string[])>> run_method;

    public EntryPoint(string[] args, Action<Obsidian, List<(string, string[])>> run_method)
    {
        this.args = args;
        this.run_method = run_method;

        bool isAdmin = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        this.AOs = isAdmin ? new Obsidian("AOs (Administrator)") : new Obsidian();

        Startup();
    }

    private void Run(List<(string cmd, string[] args)> input)
    {
        try
        {
            run_method(AOs, input);
        }

        catch (Exception err)
        {
            new Error(err.Message);
        }
    }

    private void Startup()
    {
        string[] argv = Utils.Array.Filter(args);

        var parser = new Argparse("AOs", "A Command-line utility for improved efficiency and productivity.");
        parser.Add(new string[] {"-h", "--help"}, "Display all supported arguments.", is_flag: true);
        parser.Add(new string[] {"-c", "--cmd"}, "Program passed in as string.");

        var parsed_args = parser.Parse(argv);

        // if (parsed_args.Count() == 0)
        // {
        //     Console.WriteLine("Ters");
        //     Environment.Exit(0);
        //     string startlist_path = Path.Combine(Obsidian.rootDir, "Files.x72\\root\\StartUp\\.startlist");
        //     bool isEmpty = Utils.String.IsEmpty(
        //         FileIO.FileSystem.ReadAllText(startlist_path)
        //     );

        //     if (File.Exists(startlist_path) && !isEmpty)
        //     {
        //         AOs.Entrypoint(false);
        //         foreach (string appname in File.ReadLines(startlist_path))
        //         {
        //             // break if "." is in place of appname
        //             // all apps after the dot will be marked as disabled.
        //             if (appname == ".")
        //                 break;

        //             else if (appname.EndsWith(".aos"))
        //             {
        //                 foreach (string current_line in FileIO.FileSystem.ReadAllLines( Path.Combine(Path.GetDirectoryName(startlist_path), appname) ))
        //                     Run(AOs.TakeInput(current_line));
        //             }
        //         }
        //     }

        //     else
        //     {
        //         AOs.Entrypoint();
        //         while (true)
        //             Run(AOs.TakeInput());
        //     }
        // }

        // else
        // {
        //     foreach (var arg in parsed_args)
        //     {
        //         Console.WriteLine($"[({string.Join(", ", arg.Names)}), {arg.Value}, is_flag: {arg.Is_flag}]");

        //         if (Argparse.IsAskingForHelp(arg.Names))
        //             parser.PrintHelp();

        //         else if (arg.Names.Contains("-c"))
        //         {
        //             string out_value = arg.Value;
        //             if (out_value == null || Utils.String.IsEmpty(out_value))
        //                 return;

        //             AOs.Entrypoint(false);
        //             Run(AOs.TakeInput(out_value));
        //         }

        //         else
        //         {
        //             AOs.Entrypoint(false);
        //             foreach (string filename in arg.Names)
        //             {
        //                 if (!filename.EndsWith(".aos"))
        //                 {
        //                     new Error($"{filename}: File format not recognized.");
        //                     return;
        //                 }

        //                 else if (!File.Exists(filename))
        //                 {
        //                     new Error($"{filename}: No such file or directory.");
        //                     return;
        //                 }

        //                 else
        //                 {
        //                     foreach (string current_line in FileIO.FileSystem.ReadAllLines(filename))
        //                         Run(AOs.TakeInput(current_line));
        //                 }
        //             }
        //         }
        //     }
        // }
    }
}
