using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Security.Principal;

Obsidian AOs = IsAdmin() ? new Obsidian("AOs (Administrator)") : new Obsidian();
argc();

bool IsAdmin()
{
    var identity = WindowsIdentity.GetCurrent();
    var principal = new WindowsPrincipal(identity);
    return principal.IsInRole(WindowsBuiltInRole.Administrator);
}

void argc()
{
    string[] argv = Collection.Array.Filter(args);
    if (Collection.Array.IsEmpty(argv)) Startup();
    else if (Obsidian.Shell.IsAskingForHelp(argv))
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

    else
    {
        AOs.Entrypoint(false);
        foreach (string i in argv)
        {
            if (!i.EndsWith(".aos")) new Error($"{i}: File format not recognized.");
            else if (!File.Exists(i)) new Error($"{i}: No such file or directory.");
            else
            {
                foreach (string j in File.ReadLines(i))
                    run(AOs, AOs.TakeInput(j));
            }
        }
    }
}

void Startup()
{
    AOs.Entrypoint();
    AOs.ClearConsole();

    // Log current time in boot file.
    string NoteCurrentTime = DateTime.Now.ToString("[dd-MM-yyyy], [HH:mm:ss]");
    File.AppendAllText($"{Obsidian.rDir}\\Files.x72\\root\\tmp\\BOOT.log", $"{NoteCurrentTime}\n");

    // Run all startup apps.
    string start_path = $"{Obsidian.rDir}\\Files.x72\\root\\StartUp\\";
    if (Directory.Exists(start_path))
    {
        if (File.Exists(start_path + ".startlist") && !Collection.String.IsEmpty(File.ReadAllText(start_path + ".startlist")))
        {
            foreach (string App in File.ReadLines(start_path + ".startlist"))
            {
                if (App == ".")
                    break;

                foreach (string Command in File.ReadLines(start_path + App))
                    run(AOs, AOs.TakeInput(Command));
            }
        }

        else
        {
            foreach (string App in Directory.GetFiles(start_path, "*.aos"))
            {
                foreach (string Command in File.ReadLines(App))
                    run(AOs, AOs.TakeInput(Command));
            }
        }
    }

    // Start AOs shell
    while (true)
        run(AOs, AOs.TakeInput());
}

void run(Obsidian AOs, (string cmd, string[] args) input)
{
    try
    {
        main(AOs, input);
    }

    catch (System.Exception err)
    {
        new Error(err.Message);
    }
}

void main(Obsidian AOs, (string cmd, string[] args) input)
{
    // Parse commands.
    if (Collection.String.IsEmpty(input.cmd)) { }
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
            Obsidian.Shell.StartApp(AOsBinaryFile);
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

    else if (input.cmd == "!")
    {
        if (Collection.Array.IsEmpty(input.args)) Obsidian.default_else_shell = "powershell.exe";
        else if (input.args.FirstOrDefault() == "ps" || input.args.FirstOrDefault() == "powershell") Obsidian.default_else_shell = "powershell.exe";
        else if (input.args.FirstOrDefault() == "cmd") Obsidian.default_else_shell = "cmd.exe";
        else Error.Args(input.args);
    }

    else if (input.cmd == ">")
    {
        if (Collection.Array.IsEmpty(input.args)) Obsidian.Shell.StartApp("cmd");
        else Obsidian.Shell.CommandPrompt(string.Join(" ", input.args));
    }

    else if (input.cmd == "@") Features.overload(input.args);
    else if (input.cmd.ToLower() == "wait") Features.wait(input.args);
    else if (input.cmd.ToLower() == "calendar" || input.cmd.ToLower() == "time" || input.cmd.ToLower() == "date" || input.cmd.ToLower() == "clock")
    {
        if (Collection.Array.IsEmpty(input.args))
        {
            Console.WriteLine(DateTime.Now.ToString("h:mm:ss tt"));
            Console.WriteLine(DateTime.Now.ToString("dddd, dd MMMM yyyy"));
        }

        else Error.Args(input.args);
    }

    else if (input.cmd.ToLower() == "admin" || input.cmd.ToLower() == "power")
    {
        if (Collection.Array.IsEmpty(input.args))
        {
            string AOsBinaryFile = Process.GetCurrentProcess().MainModule.FileName;
            Obsidian.Shell.StartApp(AOsBinaryFile, AppAdmin: true);
        }

        else Error.Args(input.args);
    }

    else if (input.cmd.ToLower() == "srh")
    {
        if (Collection.Array.IsEmpty(input.args)) Error.NoArgs();
        else if (input.args.Length > 3) Error.TooManyArgs(input.args);
        else Features.SearchonGoogle(input.args);
    }

    else if (input.cmd.ToLower() == "ply")
    {
        if (Collection.Array.IsEmpty(input.args)) Error.NoArgs();
        else if (input.args.Length > 2) Error.TooManyArgs(input.args);
        else Features.PlayonYT(input.args);
    }

    else if (input.cmd.ToLower() == "wiki")
    {
        if (Collection.Array.IsEmpty(input.args)) Error.NoArgs();
        else if (input.args.Length > 2) Error.TooManyArgs(input.args);
        else Features.SearchonWiki(input.args);
    }

    else if (input.cmd.ToLower() == "zip" || input.cmd.ToLower() == "rar" || input.cmd.ToLower() == "winrar")
    {
        if (Collection.Array.IsEmpty(input.args)) Error.NoArgs();
        else if (input.args.Length < 2) Error.TooFewArgs(input.args);
        else if (input.args.Length > 3) Error.TooManyArgs(input.args);
        else Features.winrar(input.args);
    }

    else if (input.cmd.ToLower() == "prompt")
    {
        if (Collection.Array.IsEmpty(input.args)) AOs.PromptPreset = new string[] { "-r" };
        else AOs.PromptPreset = input.args;
    }

    else if (input.cmd.ToLower() == "console" || input.cmd.ToLower() == "terminal" || input.cmd.ToLower() == "cmd")
    {
        if (Collection.Array.IsEmpty(input.args)) Obsidian.Shell.StartApp("cmd");
        else Obsidian.Shell.CommandPrompt(string.Join(" ", input.args));
    }

    else if (input.cmd.ToLower() == "title")
    {
        if (Collection.Array.IsEmpty(input.args)) Error.NoArgs();
        else Console.Title = string.Join(" ", Lexer.SimplifyString(input.args));
    }

    else if (input.cmd.ToLower() == "shout")
    {
        if (Collection.Array.IsEmpty(input.args)) Error.NoArgs();
        else Console.WriteLine(string.Join(" ", Lexer.SimplifyString(input.args)));
    }

    else if (input.cmd.ToLower() == "pause")
    {
        if (Collection.Array.IsEmpty(input.args)) Console.Write("Press any Key to Continue.");
        else Console.Write(string.Join(" ", Lexer.SimplifyString(input.args)));

        Console.ReadKey();
        Console.WriteLine();
    }

    else if (input.cmd.ToLower() == "allinstapps" || input.cmd.ToLower() == "installedapps" || input.cmd.ToLower() == "allinstalledapps")
    {
        if (Collection.Array.IsEmpty(input.args)) Obsidian.Shell.StartApp("winget list");
        else Error.Args(input.args);
    }

    else if (input.cmd.ToLower() == "alarm" || input.cmd.ToLower() == "timer" || input.cmd.ToLower() == "stopwatch")
    {
        if (Collection.Array.IsEmpty(input.args)) Obsidian.Shell.StartApp("ms-clock:");
        else Error.Args(input.args);
    }

    else if (input.cmd.ToLower() == "calculator" || input.cmd.ToLower() == "math")
    {
        if (Collection.Array.IsEmpty(input.args)) Obsidian.Shell.StartApp("ms-calculator:");
        else Error.Args(input.args);
    }

    else if (input.cmd.ToLower() == "music" || input.cmd.ToLower() == "video" || input.cmd.ToLower() == "media")
    {
        if (Collection.Array.IsEmpty(input.args)) Obsidian.Shell.StartApp("mswindowsmusic:");
        else Error.Args(input.args);
    }

    else if (input.cmd.ToLower() == "paint")
    {
        if (Collection.Array.IsEmpty(input.args)) Obsidian.Shell.StartApp("mspaint");
        else Error.Args(input.args);
    }

    else if (input.cmd.ToLower() == "onenote")
    {
        if (Collection.Array.IsEmpty(input.args)) Obsidian.Shell.StartApp("onenote:");
        else Error.Args(input.args);
    }

    else if (input.cmd.ToLower() == "onedrive")
    {
        if (Collection.Array.IsEmpty(input.args))
        {
            string OneDrivePath = Environment.GetEnvironmentVariable("onedrive").ToString();
            if (!Collection.String.IsEmpty(OneDrivePath)) Obsidian.Shell.StartApp(OneDrivePath);
        }

        else Error.Args(input.args);
    }

    else if (input.cmd.ToLower() == "electron" || input.cmd.ToLower() == "builder" || input.cmd.ToLower() == "amdik" || input.cmd.ToLower() == "notepad")
    {
        if (Collection.Array.IsEmpty(input.args)) Obsidian.Shell.StartApp("notepad");
        else Error.Args(input.args);
    }

    else if (input.cmd.ToLower() == "paint3d")
    {
        if (Collection.Array.IsEmpty(input.args)) Obsidian.Shell.StartApp("ms-paint:");
        else Error.Args(input.args);
    }

    else if (input.cmd.ToLower() == "notepad")
    {
        if (Collection.Array.IsEmpty(input.args)) Obsidian.Shell.StartApp("onenote:");
        else Error.Args(input.args);
    }

    else if (input.cmd.ToLower() == "settings")
    {
        if (Collection.Array.IsEmpty(input.args)) Obsidian.Shell.StartApp("ms-settings:");
        else Error.Args(input.args);
    }

    else if (input.cmd.ToLower() == "screensnip")
    {
        if (Collection.Array.IsEmpty(input.args)) Obsidian.Shell.StartApp("ms-screenclip:");
        else Error.Args(input.args);
    }

    else if (input.cmd.ToLower() == "snipandsketch")
    {
        if (Collection.Array.IsEmpty(input.args)) Obsidian.Shell.StartApp("ms-ScreenSketch:");
        else Error.Args(input.args);
    }

    else if (input.cmd.ToLower() == "minecraft")
    {
        if (Collection.Array.IsEmpty(input.args)) Obsidian.Shell.StartApp("minecraft:");
        else Error.Args(input.args);
    }

    else if (input.cmd.ToLower() == "candycrush")
    {
        if (Collection.Array.IsEmpty(input.args)) Obsidian.Shell.StartApp("candycrushsodasaga:");
        else Error.Args(input.args);
    }

    else if (input.cmd.ToLower() == "run" || input.cmd.ToLower() == "start" || input.cmd.ToLower() == "call")
    {
        if (Collection.Array.IsEmpty(input.args)) Obsidian.Shell.StartApp("cmd");
        else Obsidian.Shell.StartApp(string.Join(" ", input.args));
    }

    else if (input.cmd.ToLower() == "cat")
    {
        if (Collection.Array.IsEmpty(input.args)) Error.TooManyArgs(input.args);
        else Features.cat(input.args);
    }

    else if (input.cmd.ToLower() == "cd" || input.cmd.ToLower() == "cd." || input.cmd.ToLower() == "cd..")
    {
        if (Collection.Array.IsEmpty(input.args))
        {
            if (input.cmd.ToLower() == "cd..") Directory.SetCurrentDirectory("..");
            else Console.WriteLine(Directory.GetCurrentDirectory());
        }

        else if (input.cmd.ToLower() == "cd") Directory.SetCurrentDirectory(string.Join(" ", Lexer.SimplifyString(input.args)));
        else Error.Args(input.args);
    }

    else if (input.cmd.ToLower() == "ls" || input.cmd.ToLower() == "dir") Features.ls(input.args);

    else if (input.cmd.ToLower() == "touch" || input.cmd.ToLower() == "create")
    {
        if (Collection.Array.IsEmpty(input.args)) Error.NoArgs();
        else if (input.args.Length > 1) Error.TooManyArgs(input.args);
        else Features.touch(input.args);
    }

    else if (input.cmd.ToLower() == "del" || input.cmd.ToLower() == "rm" || input.cmd.ToLower() == "delete" || input.cmd.ToLower() == "remove")
    {
        if (Collection.Array.IsEmpty(input.args)) Error.NoArgs();
        else if (input.args.Length > 1) Error.TooManyArgs(input.args);
        else Features.del(input.args);
    }

    else if (input.cmd.ToLower() == "ren" || input.cmd.ToLower() == "rename")
    {
        if (Collection.Array.IsEmpty(input.args)) Error.NoArgs();
        else if (input.args.Length > 1) Error.TooManyArgs(input.args);
        else Features.ren(input.args);
    }

    else if (input.cmd.ToLower() == "copy" || input.cmd.ToLower() == "xcopy" || input.cmd.ToLower() == "robocopy")
    {
        if (Collection.Array.IsEmpty(input.args)) Error.NoArgs();
        else if (input.args.Length > 1) Error.TooManyArgs(input.args);
        else Features.copy(input.cmd, input.args);
    }

    else if (input.cmd.ToLower() == "move")
    {
        if (Collection.Array.IsEmpty(input.args)) Error.NoArgs();
        else if (input.args.Length > 1) Error.TooManyArgs(input.args);
        else Features.move(input.args);
    }

    else if (input.cmd.ToLower() == "pixelate" || input.cmd.ToLower() == "leaf" || input.cmd.ToLower() == "corner")
    {
        if (Collection.Array.IsEmpty(input.args)) Obsidian.Shell.StartApp("https://youtu.be/PrKtOlwwnSg");
        else Obsidian.Shell.StartApp(Obsidian.Shell.Strings(string.Join(" ", input.args)));
    }

    else if (input.cmd.ToLower() == "commit")
    {
        if (Collection.Array.IsEmpty(input.args)) Error.NoArgs();
        else if (input.args.Length > 2) Error.TooManyArgs(input.args);
        else if (input.args.Length < 2) Error.TooFewArgs(input.args);
        else Features.commit(input.args);
    }

    else if (input.cmd.ToLower() == "read" || input.cmd.ToLower() == "type")
    {
        if (Collection.Array.IsEmpty(input.args)) Error.NoArgs();
        else if (input.args.Length > 1) Error.TooManyArgs(input.args);
        else if (input.args.Length < 1) Error.TooFewArgs(input.args);
        else Features.read(input.args);
    }

    else if (input.cmd.ToLower() == "update")
    {
        if (Collection.Array.IsEmpty(input.args)) Obsidian.Shell.CheckForUpdates();
        else Error.Args(input.args);
    }

    else if (input.cmd.ToLower() == "scan")
    {
        if (Collection.Array.IsEmpty(input.args))
        {
            Console.WriteLine("Scanning.");
            if (Obsidian.Shell.Scan()) Console.WriteLine("Your PC is working fine.");
        }

        else Error.Args(input.args);
    }

    else if (input.cmd.ToLower() == "lock") Features.lockPC(input.args);
    else if (input.cmd.ToLower() == "terminate") Features.terminate(input.args);
    else if (input.cmd.ToLower() == "generate")
    {
        if (Collection.Array.IsEmpty(input.args))
        {
            Random random = new Random();
            double Generic = random.NextDouble();
            Console.WriteLine(Generic);
        }

        else Error.Args(input.args);
    }

    else if (input.cmd.ToLower() == "ran")
    {
        if (Collection.Array.IsEmpty(input.args)) Obsidian.Shell.CommandPrompt("systeminfo");
        else Error.Args(input.args);
    }

    else if (input.cmd.ToLower() == "tree")
    {
        if (Collection.Array.IsEmpty(input.args)) Obsidian.Shell.CommandPrompt("tree");
        else Error.Args(input.args);
    }

    else if (input.cmd.ToLower() == "diagxt")
    {
        if (Collection.Array.IsEmpty(input.args)) Console.WriteLine(File.ReadAllText($"{Obsidian.rDir}\\Files.x72\\root\\Config.set"));
        else Error.Args(input.args);
    }

    else if (input.cmd.ToLower() == "restore")
    {
        if (Collection.Array.IsEmpty(input.args)) Obsidian.Shell.SYSRestore();
        else Error.Args(input.args);
    }

    else if (input.cmd.ToLower() == "backup" || input.cmd.ToLower() == "restorepoint")
    {
        if (Collection.Array.IsEmpty(input.args)) Features.backup();
        else Error.Args(input.args);
    }

    else if (input.cmd.ToLower() == "reset") Features.reset(input.args);

    // Run '.aos' files.
    else if (File.Exists($"{Obsidian.rDir}\\Files.x72\\etc\\{input.cmd}.aos") || ($"{Obsidian.rDir}\\Files.x72\\etc\\{input.cmd}".ToLower().EndsWith(".aos") && File.Exists($"{Obsidian.rDir}\\Files.x72\\etc\\{input.cmd}")))
    {
        string path = $"{Obsidian.rDir}\\Files.x72\\etc\\{input.cmd}";
        string filename = path.EndsWith(".aos") ? path : path + ".aos";
        foreach (string Line in File.ReadLines(filename))
        {
            (string, string[]) input_dot_aos = AOs.TakeInput(Line);
            run(AOs, input_dot_aos);
        }
    }

    else if (File.Exists(input.cmd + ".aos") || (input.cmd.ToLower().EndsWith(".aos") && File.Exists(input.cmd)))
    {
        string filename = input.cmd.EndsWith(".aos") ? input.cmd : input.cmd + ".aos";
        foreach (string Line in File.ReadLines(filename))
        {
            (string, string[]) input_dot_aos = AOs.TakeInput(Line);
            run(AOs, input_dot_aos);
        }
    }

    else
    {
        if (!Obsidian.Shell.SysEnvApps(input.cmd, input.args)) Error.Command(input.cmd);
    }
}
