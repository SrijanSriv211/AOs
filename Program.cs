using System;
using System.IO;
using System.Linq;
using System.Security.Principal;
using System.Collections.Generic;

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

    if (Collection.Array.IsEmpty(argv))
        AOs.Entrypoint();

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
                AOs.Entrypoint(false);
                run(AOs, AOs.TakeInput(arg.value));
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

void main(Obsidian AOs, List<(string cmd, string[] args)> input)
{
    // shout "Hello world!";1+3;"1+2"
    foreach (var i in input)
    {
        Console.WriteLine(i.cmd);
        foreach (string arg in i.args)
            Console.WriteLine(arg);

        Console.WriteLine();
    }
}
