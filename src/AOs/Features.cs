using System.Diagnostics;

class Features
{
    private readonly SystemUtils sys_utils = new();
    private readonly Obsidian AOs;

    public Features(Obsidian AOs)
    {
        this.AOs = AOs;
    }

    public void Exit()
    {
        Environment.Exit(0);
    }

    public void Restart()
    {
        sys_utils.CommandPrompt("shutdown /r /t0");
    }

    public void Shutdown()
    {
        sys_utils.CommandPrompt("shutdown /s /t0");
    }

    public void Refresh()
    {
        string AOsBinaryFilepath = Process.GetCurrentProcess().MainModule.FileName;
        sys_utils.StartApp(AOsBinaryFilepath);
        Exit();
    }

    public void GetTime()
    {
        Console.WriteLine(DateTime.Now.ToString("h:mm:ss tt"));
    }

    public void GetDate()
    {
        Console.WriteLine(DateTime.Now.ToString("dddd, dd MMMM yyyy"));
    }

    public void GetDateTime()
    {
        Console.WriteLine(DateTime.Now.ToString("h:mm:ss tt"));
        Console.WriteLine(DateTime.Now.ToString("dddd, dd MMMM yyyy"));
    }

    public void GetSetHistory(string arg)
    {
        if (Utils.String.IsEmpty(arg))
            History.Get();

        else if (arg == "-c" || arg == "--clear")
            History.Clear();

        else
            Error.UnrecognizedArgs(arg);
    }

    public void ChangeColor(string color_name)
    {
        if (Utils.String.IsEmpty(color_name))
            AOs.Default_color = Obsidian.original_color_of_terminal;

        else
        {
            ConsoleColor new_color = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), color_name, true);
            AOs.Default_color = new_color;
        }
    }

    public void Wait(string timespan)
    {
        int sec;
        if (Utils.String.IsEmpty(timespan))
        {
            Console.Write("No. of Seconds$ ");
            sec = Convert.ToInt32(Console.ReadLine());
        }

        else if (!int.TryParse(timespan, out sec))
        {
            Error.UnrecognizedArgs(timespan);
            return;
        }

        SystemUtils.Track(total_seconds: sec);
    }

    public void Shout(string[] args)
    {
        Console.WriteLine(string.Join("", args));
    }

    public void Pause(string[] args)
    {
        if (Utils.Array.IsEmpty(args))
            Console.Write("Press any key to continue...");

        else
            Console.Write(string.Join("", args));

        Console.ReadKey();
    }

    public void RunInTerminal(string[] args)
    {
        if (Utils.Array.IsEmpty(args))
            sys_utils.StartApp(Obsidian.default_else_shell);

        else
        {
            string cmd_name = args.FirstOrDefault();
            string[] cmd_args = Utils.Array.Trim(args.Skip(1).ToArray());
            // sys_utils.CommandPrompt(string.Join("", args));
            sys_utils.CommandPrompt(cmd_name, cmd_args);
        }
    }

    public void RunApp(string[] args)
    {
        if (Utils.Array.IsEmpty(args))
            sys_utils.StartApp(app_name: Obsidian.default_else_shell);

        else
        {
            string app_name = args.First();
            string[] app_args = args.Length > 1 ? Utils.Array.Trim(args.Skip(1).ToArray()) : null;

            sys_utils.StartApp(app_name: app_name, app_args: app_args);
        }
    }

    public void ChangeTitle(string[] args)
    {
        if (Utils.Array.IsEmpty(args))
            Console.Title = Obsidian.is_admin ? "AOs (Administrator)" : "AOs";

        else
            Console.Title = Obsidian.is_admin ? $"{string.Join("", args)} (Administrator)" : $"{string.Join("", args)}";
    }
}
