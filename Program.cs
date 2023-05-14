using System;
using System.Security.Principal;
using System.Collections.Generic;

bool isAdmin = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
Obsidian AOs = isAdmin ? new Obsidian("AOs (Administrator)") : new Obsidian();
Startup();

void Startup()
{
    string[] argv = Collection.Array.Filter(args);
    if (Argparse.IsAskingForHelp(argv))
    {
        string[] SYSHelpCenter = {
            "A Command-line utility for improved efficiency and productivity.",
            "Usage: AOs [file]",
            "",
            "Options:",
            "-h, --help ~> Displays all supported arguments.",
        };

        Console.WriteLine(string.Join("\n", SYSHelpCenter));
    }

    else if (Collection.Array.IsEmpty(argv))
    {
        AOs.Entrypoint();
    }

    else
    {
        AOs.Entrypoint(false);
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
