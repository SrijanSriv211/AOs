using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.Collections.Generic;

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

    catch (System.Exception err)
    {
        new Error(err.Message);
        return;
    }
}

void shout()
{
    Console.WriteLine("Hello world!");
}

void exit()
{
    Environment.Exit(0);
}

void main(Obsidian AOs, List<(string cmd, string[] args)> input)
{
    //TODO: Optimize this later.
    Dictionary<string, Action> cmdlist = new Dictionary<string, Action>();
    cmdlist["shout"] = shout;
    cmdlist["echo"] = shout;

    cmdlist["quit"] = exit;
    cmdlist["exit"] = exit;

    //TODO: This is just for the GetHelp function. Improve this and make this more robust and scalable.
    var parser = new Argparse("AOs", "A Command-line utility for improved efficiency and productivity.");
    parser.Add(new string[]{ "cls", "clear" }, "Clear the screen", is_flag: true);
    parser.Add(new string[]{ "about", "info" }, "About AOs", is_flag: true);
    parser.Add(new string[]{ "shutdown" }, "Shutdown the host machine", is_flag: true);
    parser.Add(new string[]{ "restart" }, "Restart the host machine", is_flag: true);
    parser.Add(new string[]{ "quit", "exit" }, "Exit AOs", is_flag: true);
    parser.Add(new string[]{ "reload", "refresh" }, "Restart AOs", is_flag: true);
    parser.Add(new string[]{ "shout", "echo" }, "Displays messages", is_flag: false);

    // shout "Hello world!";1+3;"1+2"
    foreach (var i in input)
    {
        string lower_cmd = i.cmd.ToLower();

        if (Collection.String.IsEmpty(lower_cmd)){}
        else if (lower_cmd == "help" || Argparse.IsAskingForHelp(lower_cmd))
            parser.GetHelp(i.args.FirstOrDefault(""));

        else if (cmdlist.ContainsKey(lower_cmd))
            cmdlist[lower_cmd]();

        else
        {
            if (!Shell.SysEnvApps(lower_cmd, i.args))
                Error.Command(lower_cmd);
        }
    }
}
