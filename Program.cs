using System;
using System.Linq;
using System.Security.Principal;
using System.Collections.Generic;

Obsidian AOs = IsAdmin() ? new Obsidian("AOs (Administrator)") : new Obsidian();

bool IsAdmin()
{
    var identity = WindowsIdentity.GetCurrent();
    var principal = new WindowsPrincipal(identity);
    return principal.IsInRole(WindowsBuiltInRole.Administrator);
}

// shout "Hello world!";1+3;"1+2"
foreach (KeyValuePair<string, string[]> item in AOs.TakeInput())
{
    string cmd = item.Key;
    string[] argc = item.Value;

    Console.WriteLine(cmd);
    for (int i = 0; i < argc.Length; i++)
        Console.WriteLine(argc[i]);
}
