using System;
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

    if (Argparse.IsAskingForHelp(argv))
        parser.PrintHelp();

    else if (Collection.Array.IsEmpty(argv))
        AOs.Entrypoint();

    else
    {
        foreach (var arg in parsed_args)
        {
            if (arg.names[0] == "-c")
            {
                Console.WriteLine(arg.value);
                // AOs.Entrypoint(false);
            }

            else
            {
                // Console.WriteLine(arg.names[0]);
            }
        }
    }
}

// shout "Hello world!";1+3;"1+2"
foreach (KeyValuePair<string, string[]> item in AOs.TakeInput())
{
    string cmd = item.Key;
    string[] arg = item.Value;

    Console.WriteLine(cmd);
    for (int i = 0; i < arg.Length; i++)
        Console.WriteLine(arg[i]);
}
