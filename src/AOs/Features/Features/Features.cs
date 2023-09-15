using System.Diagnostics;

partial class Features
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

    public void ChangeCurrentDir(string dirname)
    {
        if (Utils.String.IsEmpty(dirname))
            new TerminalColor(Directory.GetCurrentDirectory(), ConsoleColor.White);

        else
            Directory.SetCurrentDirectory(Utils.String.Strings(dirname));
    }

    public void ChangeToPrevDir()
    {
        Directory.SetCurrentDirectory("..");
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
        Console.WriteLine();
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
            sys_utils.StartApp(Obsidian.default_else_shell);

        else
        {
            string appname = args.First();
            string app_args = args.Length > 1 ? string.Join("", Utils.Array.Trim(args.Skip(1).ToArray())) : null;

            sys_utils.StartApp(appname, app_args);
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

    public void Cat(string[] app_names)
    {
        if (Utils.Array.IsEmpty(app_names))
            sys_utils.ListInstalledApps();

        else
        {
            app_names = Utils.Array.Reduce(app_names);
            foreach (string appname in app_names)
                sys_utils.FindAndRunInstalledApps(appname);
        }
    }

    public void ModifyPrompt(string[] prompt)
    {
        List<string> Flags = new();
        for (int i = 0; i < prompt.Length; i++)
        {
            if (Utils.String.IsString(prompt[i]))
                Flags.Add(Utils.String.Strings(prompt[i]));

            else
            {
                string flag = "";
                for (int j = 0; j < prompt[i].Length; j++)
                {
                    if (prompt[i][j] == '-')
                    {
                        j++;
                        if (prompt[i][j] == '-')
                        {
                            j++;
                            while (j < prompt[i].Length && char.IsLetter(prompt[i][j]))
                            {
                                flag += prompt[i][j];
                                j++;
                            }

                            j--;

                            Flags.Add("--" + flag);
                            flag = "";
                        }

                        else
                        {
                            while (j < prompt[i].Length && char.IsLetter(prompt[i][j]))
                            {
                                flag += prompt[i][j];
                                j++;
                            }

                            j--;

                            Flags.Add("-" + flag);
                            flag = "";
                        }
                    }

                    else
                    {
                        while (j < prompt[i].Length && prompt[i][j] != '-')
                        {
                            flag += prompt[i][j];
                            j++;
                        }

                        j--;
                        Flags.Add(flag);
                        flag = "";
                    }
                }
            }
        }

        Obsidian.PromptDummy = AOs.SetPrompt(Flags.ToArray());
    }

    public void LS(string[] dirname)
    {
        dirname = Utils.Utils.SimplifyString(Utils.Array.Reduce(dirname));

        static void ShowDirs(string dir)
        {
            new TerminalColor(dir, ConsoleColor.White);
            string[] entries = FileIO.FolderSystem.Read(dir);

            for (int i = 0; i < entries.Length; i++)
            {
                int padding = Math.Max(100 - (int)Math.Log10(i+1), 0);

                new TerminalColor($"{i+1}. ", ConsoleColor.DarkGray, false);
                Console.Write("{0," + -padding + "}", entries[i]);

                if (File.Exists(entries[i]))
                    new TerminalColor("FILE", ConsoleColor.DarkGray);

                else if (Directory.Exists(entries[i]))
                    new TerminalColor("FOLDER", ConsoleColor.DarkGray);
            }

            Console.WriteLine();
        }

        if (Utils.Array.IsEmpty(dirname))
            ShowDirs(Directory.GetCurrentDirectory());

        else
        {
            foreach (string dir in dirname)
                ShowDirs(dir);
        }
    }

    public void Touch(string[] content_to_create)
    {
        content_to_create = Utils.Utils.SimplifyString(Utils.Array.Reduce(content_to_create));

        foreach (string content in content_to_create)
        {
            if (content.ToLower() == "con")
                new TerminalColor("Hello CON!", ConsoleColor.White);

            else if (content.EndsWith("\\") || content.EndsWith("/"))
                FileIO.FolderSystem.Create(content);

            else if (File.Exists(content))
                FileIO.FileSystem.Create(content);

            else
            {
                new Error($"'{content}' does not exist");
                return;
            }
        }
    }

    public void Delete(string[] content_to_delete)
    {
        content_to_delete = Utils.Utils.SimplifyString(Utils.Array.Reduce(content_to_delete));

        foreach (string content in content_to_delete)
        {
            if (content.ToLower() == "con")
                new TerminalColor("Don't Delete CON!", ConsoleColor.Cyan);

            else if (Directory.Exists(content))
                FileIO.FolderSystem.Delete(content);

            else if (File.Exists(content))
                FileIO.FileSystem.Delete(content);

            else
            {
                new Error($"'{content}' does not exist");
                return;
            }
        }
    }

    public void Move(string[] content_to_rename)
    {
        content_to_rename = Utils.Utils.SimplifyString(Utils.Array.Reduce(content_to_rename));

        string old_name = content_to_rename[0];
        string new_name = content_to_rename[1];

        if (old_name.ToLower() == "con" || new_name.ToLower() == "con")
            new TerminalColor("Hello CON!", ConsoleColor.White);

        else if (Directory.Exists(old_name))
            FileIO.FolderSystem.Move(old_name, new_name);

        else if (File.Exists(old_name))
            FileIO.FileSystem.Move(old_name, new_name);

        else
            new Error($"'{old_name}' does not exist");
    }

    public void Copy(string[] content_to_rename)
    {
        content_to_rename = Utils.Utils.SimplifyString(Utils.Array.Reduce(content_to_rename));

        string old_name = content_to_rename[0];
        string new_name = content_to_rename[1];

        if (old_name.ToLower() == "con" || new_name.ToLower() == "con")
            new TerminalColor("Hello CON!", ConsoleColor.White);

        else if (Directory.Exists(old_name))
            FileIO.FolderSystem.Copy(old_name, new_name);

        else if (File.Exists(old_name))
            FileIO.FileSystem.Copy(old_name, new_name);

        else
            new Error($"'{old_name}' does not exist");
    }
}
