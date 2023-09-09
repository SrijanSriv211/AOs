using System.Diagnostics;

class Features
{
    private readonly SystemUtils sys_utils;
    private readonly Obsidian AOs;

    public Features(Obsidian AOs, SystemUtils sys_utils)
    {
        this.AOs = AOs;
        this.sys_utils = sys_utils;
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

    public void SwitchElseShell(string shell_name)
    {
        if (Utils.String.IsEmpty(shell_name))
            Console.WriteLine(Obsidian.default_else_shell);

        else if (shell_name == "cmd")
            Obsidian.default_else_shell = "cmd.exe";

        else if (shell_name == "ps" || shell_name == "powershell")
            Obsidian.default_else_shell = "powershell.exe";

        else
            Error.UnrecognizedArgs(shell_name);
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
        static void help_for_color()
        {
            new Error("`color` value was not defined. Please use a defined color value");
            string[] list_of_colors = new string[]
            {
                "Black", "DarkBlue", "DarkGreen", "DarkCyan", "DarkRed",
                "DarkMagenta", "DarkYellow", "Gray", "DarkGray", "Blue",
                "Green", "Cyan", "Red", "Magenta", "Yellow", "White"
            };

            for (int i = 0; i < list_of_colors.Length; i++)
            {
                Console.Write($"{i}. ");
                if (i == 0)
                {
                    var default_color = Console.BackgroundColor;
                    Console.BackgroundColor = ConsoleColor.White;

                    new TerminalColor(list_of_colors[i], (ConsoleColor)i);

                    Console.BackgroundColor = default_color;
                }

                else
                    new TerminalColor(list_of_colors[i], (ConsoleColor)i);
            }
        }

        if (Utils.String.IsEmpty(color_name))
            AOs.Default_color = Obsidian.original_color_of_terminal;

        else
        {
            if (int.TryParse(color_name, out int color_num))
            {
                if (color_num < 0 || color_num > 15)
                    help_for_color();

                else
                    AOs.Default_color = (ConsoleColor)color_num;
            }

            else
                help_for_color();
        }

        Console.ForegroundColor = AOs.Default_color;
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
        Console.WriteLine(string.Join("", Utils.Utils.SimplifyString(args)));
    }

    public void Pause(string[] args)
    {
        if (Utils.Array.IsEmpty(args))
            Console.Write("Press any key to continue...");

        else
            Console.Write(string.Join("", Utils.Utils.SimplifyString(args)));

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
        {
            string title = string.Join("", Utils.Utils.SimplifyString(args));
            Console.Title = Obsidian.is_admin ? $"{title} (Administrator)" : $"{title}";
        }
    }
}
