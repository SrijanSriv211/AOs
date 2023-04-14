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
Dictionary<string, string[]> inp = AOs.TakeInput();
foreach (KeyValuePair<string, string[]> item in inp)
{
    string cmd = item.Key;
    string[] argc = item.Value;
}
