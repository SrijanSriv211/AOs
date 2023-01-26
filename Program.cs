using System;
using System.Linq;
using System.Diagnostics;

Obsidian AOs = new Obsidian();
AOs.Entrypoint();
while (true)
{
    var input = AOs.TakeInput();

    // Parse commands.
    if (Collection.String.IsEmpty(input.cmd)) continue;
    else if (input.cmd.ToLower() == "cls" || input.cmd.ToLower() == "clear")
    {
        if (Collection.Array.IsEmpty(input.args)) AOs.ClearConsole();
        else Error.Args(input.args);
    }

    else if (input.cmd.ToLower() == "about" || input.cmd.ToLower() == "info")
        Console.WriteLine("Command-line utility for improved efficiency and productivity.");

    else if (input.cmd.ToLower() == "shutdown") Obsidian.Shell.CommandPrompt("shutdown /s /t0");
    else if (input.cmd.ToLower() == "quit" || input.cmd.ToLower() == "exit")
    {
        if (Collection.Array.IsEmpty(input.args)) Environment.Exit(0);
        else Error.Args(input.args);
    }

    else if (input.cmd.ToLower() == "restart") Obsidian.Shell.CommandPrompt("shutdown /r /t0");
    else if (input.cmd.ToLower() == "reload" || input.cmd.ToLower() == "refresh")
    {
        if (Collection.Array.IsEmpty(input.args))
        {
            string AOsBinaryFile = Process.GetCurrentProcess().MainModule.FileName;
            Obsidian.Shell.CommandPrompt($"\"{AOsBinaryFile}\"");
            Environment.Exit(0);
        }

        else Error.Args(input.args);
    }

    else if (input.cmd.ToLower() == "help" || Obsidian.Shell.IsAskingForHelp(input.cmd))
    {
        if (Collection.Array.IsEmpty(input.args)) Obsidian.Shell.GetHelp();
        else Obsidian.Shell.GetHelp(input.args);
    }

    else if (input.cmd.ToLower() == "version" || input.cmd.ToLower() == "ver" || input.cmd.ToLower() == "-v")
    {
        if (Collection.Array.IsEmpty(input.args)) Console.WriteLine(AOs.Version);
        else Error.Args(input.args);
    }

    else if (input.cmd.ToLower() == "credits")
    {
        if (Collection.Array.IsEmpty(input.args)) AOs.Credits();
        else Error.Args(input.args);
    }

    else if (input.cmd == "AOs1000")
    {
        if (Collection.Array.IsEmpty(input.args))
        {
            string[] AOs1000 = {
                "AOs1000!",
                "CONGRATULATIONS! For hitting 1000 LINES OF CODE in AOs 1.3!",
                "It was the first program to ever reach these many LINES OF CODE!"
            };

            Console.WriteLine(string.Join("\n", AOs1000));
        }

        else Error.Args(input.args);
    }

    else if (input.cmd.ToLower() == "history")
    {
        if (Collection.Array.IsEmpty(input.args)) Obsidian.History.GetHistory();
        else if ((input.args.FirstOrDefault() == "-c" || input.args.FirstOrDefault() == "--clear") && input.args.Length == 1)
            Obsidian.History.ClearHistory();

        else Error.Args(input.args);
    }

    else if (input.cmd == ">")
    {
        if (Collection.Array.IsEmpty(input.args)) Obsidian.Shell.StartApp("cmd");
        else Obsidian.Shell.CommandPrompt(string.Join(" ", input.args));
    }

    else if (input.cmd == "@") Features.overload(input.args);
    else if (input.cmd.ToLower() == "wait") Features.wait(input.args);
    else
    {
        if (!Obsidian.Shell.SysEnvApps(input.cmd, input.args)) Error.Command(input.cmd);
    }
}
