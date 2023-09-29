class Features
{
    private readonly Utils.Https https = new();
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
        sys_utils.StartApp(Obsidian.AOs_binary_path);
        Exit();
    }

    public void Admin()
    {
        sys_utils.StartApp(Obsidian.AOs_binary_path, is_admin: true);
    }

    public void PrintVersion()
    {
        Console.WriteLine(AOs.version);
    }

    public void About()
    {
        new TerminalColor(Obsidian.about_AOs, ConsoleColor.White);
        new TerminalColor("For more information go to ", ConsoleColor.DarkGray, false);
        new TerminalColor(Obsidian.AOs_repo_link, ConsoleColor.Cyan);
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

    public void GenRandomNum()
    {
        Random random = new();
        Console.WriteLine(random.NextDouble());
    }

    public void SysInfo()
    {
        sys_utils.CommandPrompt("systeminfo");
    }

    public void Tree()
    {
        sys_utils.CommandPrompt("tree");
    }

    public void Diagxt()
    {
        string[] details = {
            $"NAME        : AOs {Obsidian.version_no}",
            $"BUILD       : Release Build {Obsidian.build_no}",
            $"VERSION     : {AOs.version}",
            $"PROCESS     : {Obsidian.AOs_binary_path}",
             "SYSTEM TYPE : x72",
             "",
             "AUTHOR           : Light-Lens (Srijan Srivastava)",
            $"REGISTERED OWNER : {Environment.GetEnvironmentVariable("username")}",
             "",
            $"ROOT DIRECTORY     : {Obsidian.root_dir}",
            $"SYSTEM DIRECTORY   : {Path.Combine(Obsidian.root_dir, "Files.x72\\root")}",
             "",
            $"GITHUB REPO     : {Obsidian.AOs_repo_link}",
             "SYSTEM LANGUAGE : en-in; English (India)"
        };

        new TerminalColor(string.Join("\n", details), ConsoleColor.White);
    }

    public void Scan()
    {
        if (Obsidian.is_admin)
        {
            sys_utils.CommandPrompt("sfc /scannow");
            sys_utils.CommandPrompt("DISM /Online /Cleanup-Image /CheckHealth");
            sys_utils.CommandPrompt("DISM /Online /Cleanup-Image /ScanHealth");
            sys_utils.CommandPrompt("DISM /Online /Cleanup-image /Restorehealth");
            new TerminalColor($"Please check '{SystemUtils.CheckForSysOrEnvApps("%windir%")}\\Logs\\CBS\\CBS.log' ", ConsoleColor.White, false);
            new TerminalColor($"and '{SystemUtils.CheckForSysOrEnvApps("%windir%")}\\Logs\\DISM\\dism.log' for more details.", ConsoleColor.White);
        }

        else
        {
            new Error("Please run AOs as Administrator to scan the integrity of all protected system files.");
            new TerminalColor("Type", ConsoleColor.Gray, false);
            new TerminalColor(" 'admin' ", ConsoleColor.White, false);
            new TerminalColor("to run AOs in Administrator", ConsoleColor.Gray);
        }
    }

    public void ChangeToPrevDir()
    {
        Directory.SetCurrentDirectory("..");
    }

    public void ChangeCurrentDir(string dirname)
    {
        if (Utils.String.IsEmpty(dirname))
            new TerminalColor(Directory.GetCurrentDirectory(), ConsoleColor.White);

        else
            Directory.SetCurrentDirectory(Utils.String.Strings(dirname));
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
            AOs.current_foreground_color = Obsidian.original_foreground_color;

        else
        {
            if (int.TryParse(color_name, out int color_num))
            {
                if (color_num < 0 || color_num > 15)
                    help_for_color();

                else
                    AOs.current_foreground_color = (ConsoleColor)color_num;
            }

            else
                help_for_color();
        }

        Console.ForegroundColor = AOs.current_foreground_color;
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
        Console.Write(string.Join("", Utils.Utils.SimplifyString(args)));
        Console.ReadKey();
        Console.WriteLine();
    }

    public void RunInTerminal(string[] args)
    {
        if (Utils.Array.IsEmpty(args))
            sys_utils.StartApp(Obsidian.default_else_shell);

        else
            sys_utils.CommandPrompt(string.Join("", args));
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
            app_names = Utils.Array.Reduce(Utils.Array.Filter(app_names));
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

        AOs.prompt_preset = Flags.ToArray();
        AOs.SetPrompt(Flags.ToArray());
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
                int padding = Utils.Maths.CalculatePadding(i+1, 100);

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

            else
                FileIO.FileSystem.Create(content);
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

    public void Pixelate(string[] websites_to_open)
    {
        static bool ValidateUrlWithUri(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        static void err(string website_url)
        {
            new Error($"Can't open the website '{website_url}': Invalid URL");
        }

        string engine_name = "google", city_name = "muzaffarpur";
        var parser = new Argparse("pixelate", "Start a website in a web browser", err);
        parser.Add(new string[]{"-e", "--engine"}, "Search for a query on a specific search engine (google, bing, duckduckgo, youtube, wikipedia)", default_value: engine_name);
        parser.Add(new string[]{"-w", "--weather"}, "Display today's weather in a city", default_value: city_name);
        parser.Add(new string[]{"-t", "--temp", "--temperature"}, "Display today's temperature in a city", default_value: city_name);

        websites_to_open = Utils.Utils.SimplifyString(Utils.Array.Reduce(Utils.Array.Filter(websites_to_open)));
        var parsed_args = parser.Parse(websites_to_open);

        List<string> queries = new();
        foreach (var arg in parsed_args)
        {
            if (arg.Names.Contains("-e"))
                engine_name = arg.Value;

            else if (arg.Names.Contains("-w"))
            {
                new TerminalColor($"Fetching today's weather report for {arg.Value}", ConsoleColor.White);
                string weather = https.HttpsClient($"https://wttr.in/{arg.Value}?format=%C");
                Console.WriteLine(weather);
            }

            else if (arg.Names.Contains("-t"))
            {
                new TerminalColor($"Fetching today's temperature report for {arg.Value}", ConsoleColor.White);
                string temp = https.HttpsClient($"https://wttr.in/{arg.Value}?format=%t");
                Console.WriteLine((temp[0] == '+') ? temp.Substring(1) : temp);
            }

            else if (arg.KnownType == "Unknown")
                queries.Add(arg.Names.First());
        }

        Dictionary<string, string> engines = new()
        {
            {"bing", "https://www.bing.com/search?q="},
            {"google", "https://www.google.com/search?q="},
            {"duckduckgo", "https://duckduckgo.com/?q="},
            {"ddg", "https://duckduckgo.com/?q="},
            {"youtube", "https://www.youtube.com/results?search_query="},
            {"yt", "https://www.youtube.com/results?search_query="},
            {"wikipedia", "https://en.wikipedia.org/wiki/"},
            {"wiki", "https://en.wikipedia.org/wiki/"}
        };

        foreach (string query in queries)
        {
            if (ValidateUrlWithUri(query))
                sys_utils.StartApp(query);

            else
                sys_utils.StartApp(engines[engine_name] + query);
        }
    }

    public void Read(string[] args)
    {
        static void err(string filename)
        {
            new Error($"{filename}: No such file or directory");
        }

        var parser = new Argparse("read", "Displays the contents of a text file.", err);
        parser.Add(new string[]{"-l", "--line"}, "Shows information about a specific line", default_value: "-1");

        args = Utils.Utils.SimplifyString(Utils.Array.Reduce(args));
        var parsed_args = parser.Parse(args);

        string filepath = "";
        int line_to_read = -1;
        foreach (var arg in parsed_args)
        {
            if (arg.Names.Contains("-l") && arg.KnownType == "Known" && !int.TryParse(arg.Value, out line_to_read))
            {
                new Error($"{line_to_read}: Invalid line number");
                return;
            }

            else if (arg.KnownType == "Unknown")
                filepath = arg.Names.First();
        }

        string[] lines = FileIO.FileSystem.ReadAllLines(filepath);
        if (line_to_read < 1)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                new TerminalColor($"{i+1}. ", ConsoleColor.DarkGray, false);
                Console.WriteLine(lines[i]);
            }
        }

        else
        {
            if (line_to_read-1 < lines.Length)
            {
                new TerminalColor($"{line_to_read}. ", ConsoleColor.DarkGray, false);
                Console.WriteLine(lines[line_to_read-1]);
            }
        }
    }


    public void Commit(string[] args)
    {
        static void err(string filename)
        {
            new Error($"{filename}: No such file or directory.");
        }

        var parser = new Argparse("commit", "Edit the contents of a text file", err);
        parser.Add(new string[]{"-l", "--line"}, "Edit specific line in a text file", default_value: "-1");

        args = Utils.Utils.SimplifyString(Utils.Array.Reduce(args));
        var parsed_args = parser.Parse(args);

        List<string> Content_to_commit = new();
        string filepath = "";
        int line_to_read = -1;
        foreach (var arg in parsed_args)
        {
            if (arg.Names.Contains("-l") && arg.KnownType == "Known" && !int.TryParse(arg.Value, out line_to_read))
            {
                new Error($"{line_to_read}: Invalid line number");
                return;
            }

            else if (arg.KnownType == "Unknown" && Utils.String.IsEmpty(filepath))
                filepath = arg.Names.First();

            else if (arg.KnownType == "Unknown")
                Content_to_commit.Add(arg.Names.First());
        }

        if (line_to_read == 0)
            FileIO.FileSystem.Overwrite(filepath, Content_to_commit.ToArray());

        else if (line_to_read < 1)
        {
            foreach (string content in Content_to_commit)
                FileIO.FileSystem.Write(filepath, content);
        }

        else
        {
            string[] lines = FileIO.FileSystem.ReadAllLines(filepath);
            if (line_to_read-1 < lines.Length)
            {
                for (int i = 0; i < Content_to_commit.Count; i++)
                    lines[line_to_read-1+i] = Content_to_commit[i];

                FileIO.FileSystem.Overwrite(filepath, lines);
            }
        }
    }

    public void WinRAR(string[] args)
    {
        var parser = new Argparse("zip", "Compress or Decompress files or folders", Error.UnrecognizedArgs);
        parser.Add(new string[]{"-u", "--uncompress", "--decompress"}, "Decompress zip files", is_flag: true);

        args = Utils.Utils.SimplifyString(Utils.Array.Reduce(args));
        var parsed_args = parser.Parse(args);

        bool unzip = false;
        List<string> Content_to_zip = new();
        foreach (var arg in parsed_args)
        {
            if (arg.Names.Contains("-u"))
                unzip = true;

            else if (arg.KnownType == "Unknown" && !Utils.String.IsEmpty(arg.Names.First()))
                Content_to_zip.Add(arg.Names.First());
        }

        if (unzip)
        {
            foreach (string content in Content_to_zip)
                FileIO.FolderSystem.Decompress(content, $"{content}.zip");
        }

        else
        {
            foreach (string content in Content_to_zip)
                FileIO.FolderSystem.Compress(content, $"{content}.zip");
        }
    }
}
