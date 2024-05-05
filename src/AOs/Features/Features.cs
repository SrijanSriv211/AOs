partial class Features()
{
    public static Obsidian AOs;

    public static void Exit()
    {
        Environment.Exit(0);
    }

    public static void Restart()
    {
        SystemUtils.CommandPrompt("shutdown /r /t 0");
        Environment.Exit(0);
    }

    public static void Shutdown()
    {
        SystemUtils.CommandPrompt("shutdown /s /t 0");
        Environment.Exit(0);
    }

    public static void Lock()
    {
        WindowsControl.SystemManager.LockWorkStation();
    }

    public static void Sleep()
    {
        // https://www.codeproject.com/Tips/480049/Shut-Down-Restart-Log-off-Lock-Hibernate-or-Sleep
        WindowsControl.SystemManager.SetSuspendState(false, true, true);
    }

    public static void Refresh()
    {
        SystemUtils.StartApp(Obsidian.AOs_binary_path);
        Exit();
    }

    public static void Admin()
    {
        SystemUtils.StartApp(Obsidian.AOs_binary_path, is_admin: true);
    }

    public static void PrintVersion()
    {
        Console.WriteLine(AOs.version);
    }

    // A search engine which can search for any file, folder or app in your PC like Ueli or PowerToys Run
    public static void Rij(string[] input)
    {
        string query = string.Join("", input);

        if (Utils.String.IsEmpty(query) || !File.Exists(Path.Combine(Obsidian.root_dir, "Files.x72\\root\\search_index")))
            EntryPoint.SearchIndex();

        //TODO: Improve the search algorithm and make it more accurate.
        if (!Utils.String.IsEmpty(query))
        {
            string[] search_indexes = FileIO.FileSystem.ReadAllLines(Path.Combine(Obsidian.root_dir, "Files.x72\\root\\search_index"));

            List<string[]> index = [];
            for (int i = 0; i < search_indexes.Length; i++)
            {
                foreach (string root_path in EntryPoint.Settings.search_index.search_paths)
                {
                    if (search_indexes[i].StartsWith(root_path))
                        search_indexes[i] = search_indexes[i].Replace($"{root_path}\\", "");

                    index.Add(search_indexes[i].Split("\\"));
                }
            }

            List<(string, int, int)> output = [];
            for (int i = 0; i < index.Count; i++)
            {
                Utils.SpellCheck checker = new(index[i].ToList());
                (string, int) check = checker.Check(query, 1).FirstOrDefault();
                output.Add((check.Item1, check.Item2, i));
            }

            output.Sort((x, y) => x.Item2.CompareTo(y.Item2));
            Console.WriteLine(string.Join("\\", index[output.First().Item3]));
        }
    }

    public static void PrintAOsSettings()
    {
        Terminal.Print("Default-Else-Shell: ", ConsoleColor.White);
        Terminal.Print(EntryPoint.Settings.default_else_shell);
        Console.WriteLine();

        Terminal.Print("Username: ", ConsoleColor.White);
        Terminal.Print(EntryPoint.Settings.username ?? "None");
        Console.WriteLine();

        Terminal.Print("Startup Apps (", ConsoleColor.White, false);
        Terminal.Print(Path.Combine(Obsidian.root_dir, "Files.x72\\etc\\Startup"), ConsoleColor.DarkGray, false);
        Terminal.Print("):", ConsoleColor.White);
        if (!Utils.Array.IsEmpty(EntryPoint.Settings.startlist))
        {
            for (int i = 0; i < EntryPoint.Settings.startlist.Length; i++)
            {
                Terminal.Print($"{i+1}. ", ConsoleColor.DarkGray, false);
                Console.WriteLine("{0," + -Utils.Maths.CalculatePadding(i+1, 10) + "}", EntryPoint.Settings.startlist[i]);
            }
        }

        else
            Console.WriteLine("None");

        Console.WriteLine();
        Terminal.Print(Path.Combine(Obsidian.root_dir, "Files.x72\\root\\settings.json"), ConsoleColor.DarkGray);
    }

    public static void About()
    {
        Terminal.Print(Obsidian.about_AOs, ConsoleColor.White);
        Terminal.Print("For more information go to ", ConsoleColor.DarkGray, false);
        Terminal.Print(Obsidian.AOs_repo_link, ConsoleColor.Cyan);
    }

    public static void GetTime()
    {
        Console.WriteLine(DateTime.Now.ToString("h:mm:ss tt"));
    }

    public static void GetDate()
    {
        Console.WriteLine(DateTime.Now.ToString("dddd, dd MMMM yyyy"));
    }

    public static void GetDateTime()
    {
        Console.WriteLine(DateTime.Now.ToString("h:mm:ss tt"));
        Console.WriteLine(DateTime.Now.ToString("dddd, dd MMMM yyyy"));
    }

    public static void GenRandomNum()
    {
        Random random = new();
        Console.WriteLine(random.NextDouble());
    }

    public static void SysInfo()
    {
        SystemUtils.CommandPrompt("systeminfo");
    }

    public static void Tree()
    {
        SystemUtils.CommandPrompt("tree");
    }

    public static void Diagxt()
    {
        string[] details = [
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
            $"USER DIRECTORY     : {Path.Combine(Obsidian.root_dir, "Files.x72\\etc")}",
            $"SYSTEM DIRECTORY   : {Path.Combine(Obsidian.root_dir, "Files.x72\\root")}",
            $"STARTUP DIRECTORY  : {Path.Combine(Obsidian.root_dir, "Files.x72\\etc\\Startup")}",
            $"SETTINGS FILE      : {Path.Combine(Obsidian.root_dir, "Files.x72\\root\\settings.json")}",
            "",
            $"GITHUB REPO     : {Obsidian.AOs_repo_link}",
             "SYSTEM LANGUAGE : en-in; English (India)"
        ];

        Terminal.Print(string.Join("\n", details), ConsoleColor.White);
    }

    public static void Scan()
    {
        if (Obsidian.is_admin)
        {
            SystemUtils.CommandPrompt("sfc /scannow");
            SystemUtils.CommandPrompt("DISM /Online /Cleanup-Image /CheckHealth");
            SystemUtils.CommandPrompt("DISM /Online /Cleanup-Image /ScanHealth");
            SystemUtils.CommandPrompt("DISM /Online /Cleanup-image /Restorehealth");
            Terminal.Print($"Please check '{SystemUtils.CheckForEnvVarAndEXEs("%windir%")}\\Logs\\CBS\\CBS.log' ", ConsoleColor.White, false);
            Terminal.Print($"and '{SystemUtils.CheckForEnvVarAndEXEs("%windir%")}\\Logs\\DISM\\dism.log' for more details.", ConsoleColor.White);
        }

        else
        {
            new Error("Please run AOs as Administrator to scan the integrity of all protected system files.", "runtime error");
            Terminal.Print("Type", is_newline: false);
            Terminal.Print(" 'admin' ", ConsoleColor.White, false);
            Terminal.Print("to run AOs in Administrator");
        }
    }

    public static void ChangeToPrevDir()
    {
        Directory.SetCurrentDirectory("..");
    }

    public static void ChangeCurrentDir(string dirname)
    {
        dirname = Utils.String.Strings(dirname);
        if (Utils.String.IsEmpty(dirname))
            Terminal.Print(Directory.GetCurrentDirectory(), ConsoleColor.White);

        else if (Directory.Exists(dirname))
            Directory.SetCurrentDirectory(dirname);

        else
            new Error($"Folder with name '{dirname}' does not exist.", "runtime error");
    }

    public static void SwitchElseShell(string shell_name)
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

    public static void GetSetHistory(string arg)
    {
        if (Utils.String.IsEmpty(arg))
            History.Get();

        else if (arg == "-c" || arg == "--clear")
            History.Clear();

        else
            Error.UnrecognizedArgs(arg);
    }

    public static void ChangeColor(string color_name)
    {
        void help_for_color()
        {
            new Error("`color` value was not defined. Please use a defined color value", "runtime error");
            string[] list_of_colors =
            [
                "Black", "DarkBlue", "DarkGreen", "DarkCyan", "DarkRed",
                "DarkMagenta", "DarkYellow", "Gray", "DarkGray", "Blue",
                "Green", "Cyan", "Red", "Magenta", "Yellow", "White"
            ];

            for (int i = 0; i < list_of_colors.Length; i++)
            {
                Console.Write($"{i}. ");
                if (i == 0)
                {
                    var default_color = Console.BackgroundColor;
                    Console.BackgroundColor = ConsoleColor.White;

                    Terminal.Print(list_of_colors[i], (ConsoleColor)i);

                    Console.BackgroundColor = default_color;
                }

                else
                    Terminal.Print(list_of_colors[i], (ConsoleColor)i);
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

    public static void Wait(string timespan)
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

    public static void Shout(string[] args)
    {
        Console.WriteLine(string.Join("", args));
    }

    public static void Pause(string[] args)
    {
        Console.Write(string.Join("", args));
        Console.ReadKey();
        Console.WriteLine();
    }

    public static void RunInTerminal(string[] args)
    {
        if (Utils.Array.IsEmpty(args))
            SystemUtils.StartApp(Obsidian.default_else_shell);

        else
            SystemUtils.CommandPrompt(string.Join(" ", args));
    }

    public static void RunApp(string[] args)
    {
        if (Utils.Array.IsEmpty(args))
            SystemUtils.StartApp(Obsidian.default_else_shell);

        else
        {
            string appname = args.First();
            string app_args = args.Length > 1 ? string.Join(" ", Utils.Array.Trim(args.Skip(1).ToArray())) : null;
            SystemUtils.StartApp(appname, app_args);
        }
    }

    public static void ChangeTitle(string[] args)
    {
        if (Utils.Array.IsEmpty(args))
            Console.Title = Obsidian.is_admin ? "AOs (Administrator)" : "AOs";

        else
        {
            string title = string.Join("", args);
            Console.Title = Obsidian.is_admin ? $"{title} (Administrator)" : $"{title}";
        }
    }

    public static void Cat(string[] app_names)
    {
        if (Utils.Array.IsEmpty(app_names))
            SystemUtils.ListInstalledApps();

        else
        {
            app_names = Utils.Array.Reduce(Utils.Array.Filter(app_names));
            foreach (string appname in app_names)
                SystemUtils.FindAndRunInstalledApps(appname);
        }
    }

    public static void ModifyPrompt(string[] prompt)
    {
        List<string> Flags = [];
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

        if (!Argparse.IsAskingForHelp([.. Flags]))
            AOs.prompt_preset = [.. Flags];

        AOs.SetPrompt([.. Flags]);
    }

    public static void LS(string[] dirname)
    {
        dirname = Utils.Array.Reduce(dirname);

        void ShowDirs(string dir)
        {
            Terminal.Print($"{dir}\n", ConsoleColor.White);
            string[] entries = FileIO.FolderSystem.Read(dir).Select(Path.GetFileName).ToArray();

            for (int i = 0; i < entries.Length; i++)
            {
                int padding = Utils.Maths.CalculatePadding(i+1, 50);

                if (i == 0)
                {
                    Terminal.Print(string.Format("{0," + -padding + "}", "Name") + "   Type\t", ConsoleColor.White, false);
                    Terminal.Print("    Creation Time\t", ConsoleColor.White, false);
                    Console.Write("       ");
                    Terminal.Print("Last Write Time", ConsoleColor.White);
                }

                Terminal.Print($"{i+1}. ", ConsoleColor.DarkGray, false);
                Console.Write("{0," + -padding + "}", entries[i]);

                if (File.Exists(entries[i]))
                    Terminal.Print("file\t", ConsoleColor.DarkGray, false);

                else if (Directory.Exists(entries[i]))
                    Terminal.Print("dir\t", ConsoleColor.DarkGray, false);

                Terminal.Print(File.GetCreationTime(entries[i]).ToString("dd-MM-yyyy   HH:mm:ss"), ConsoleColor.DarkGray, false);
                Console.Write("       ");
                Terminal.Print(File.GetLastWriteTime(entries[i]).ToString("dd-MM-yyyy   HH:mm:ss"), ConsoleColor.DarkGray);
            }
        }

        if (Utils.Array.IsEmpty(dirname))
            ShowDirs(Directory.GetCurrentDirectory());

        else
        {
            foreach (string dir in dirname)
                ShowDirs(dir);
        }
    }

    public static void Touch(string[] content_to_create)
    {
        content_to_create = Utils.Array.Reduce(content_to_create);

        foreach (string content in content_to_create)
        {
            if (content.ToLower() == "con")
                Terminal.Print("Hello CON!", ConsoleColor.White);

            else if (content.EndsWith("\\") || content.EndsWith("/"))
                FileIO.FolderSystem.Create(content);

            else
                FileIO.FileSystem.Create(content);
        }
    }

    public static void Delete(string[] content_to_delete)
    {
        content_to_delete = Utils.Array.Reduce(content_to_delete);

        foreach (string content in content_to_delete)
        {
            if (content.ToLower() == "con")
                Terminal.Print("Don't Delete CON!", ConsoleColor.Cyan);

            else if (Directory.Exists(content))
                FileIO.FolderSystem.Delete(content);

            else if (File.Exists(content))
                FileIO.FileSystem.Delete(content);

            else
            {
                new Error($"'{content}' does not exist", "runtime error");
                return;
            }
        }
    }

    public static void Move(string[] content_to_rename)
    {
        content_to_rename = Utils.Array.Reduce(content_to_rename);

        string old_name = content_to_rename[0];
        string new_name = content_to_rename[1];

        if (old_name.ToLower() == "con" || new_name.ToLower() == "con")
            Terminal.Print("Hello CON!", ConsoleColor.White);

        else if (Directory.Exists(old_name))
            FileIO.FolderSystem.Move(old_name, new_name);

        else if (File.Exists(old_name))
            FileIO.FileSystem.Move(old_name, new_name);

        else
            new Error($"'{old_name}' does not exist", "runtime error");
    }

    public static void Copy(string[] content_to_rename)
    {
        content_to_rename = Utils.Array.Reduce(content_to_rename);

        string old_name = content_to_rename[0];
        string new_name = content_to_rename[1];

        if (old_name.ToLower() == "con" || new_name.ToLower() == "con")
            Terminal.Print("Hello CON!", ConsoleColor.White);

        else if (Directory.Exists(old_name))
            FileIO.FolderSystem.Copy(old_name, new_name);

        else if (File.Exists(old_name))
            FileIO.FileSystem.Copy(old_name, new_name);

        else
            new Error($"'{old_name}' does not exist", "runtime error");
    }

    public static void Pixelate(string[] websites_to_open)
    {
        void err(string website_url)
        {
            new Error($"Can't open the website '{website_url}': Invalid URL", "runtime error");
        }

        string engine_name = "google", city_name = "muzaffarpur";
        var parser = new Argparse("pixelate", "Start a website in a web browser", err);
        parser.Add(["-e", "--engine"], "Search for a query on a specific search engine (google, bing, duckduckgo, youtube, wikipedia)", default_value: engine_name);
        parser.Add(["-w", "--weather"], "Display today's weather in a city", default_value: city_name);
        parser.Add(["-t", "--temp", "--temperature"], "Display today's temperature in a city", default_value: city_name);
        var parsed_args = parser.Parse(Utils.Array.Reduce(Utils.Array.Filter(websites_to_open)));

        List<string> queries = [];
        foreach (var arg in parsed_args)
        {
            string[] websites = arg.Names;
            if (websites.Contains("-e"))
                engine_name = arg.Value;

            else if (websites.Contains("-w"))
            {
                Terminal.Print($"Fetching today's weather report for {arg.Value}", ConsoleColor.White);
                string weather = Utils.Https.HttpsClient($"https://wttr.in/{arg.Value}?format=%C");
                Console.WriteLine(weather);
            }

            else if (websites.Contains("-t"))
            {
                Terminal.Print($"Fetching today's temperature report for {arg.Value}", ConsoleColor.White);
                string temp = Utils.Https.HttpsClient($"https://wttr.in/{arg.Value}?format=%t");
                Console.WriteLine((temp[0] == '+') ? temp.Substring(1) : temp);
            }

            else if (arg.KnownType == "Unknown")
                queries.Add(websites.First());
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
            if (Utils.Https.ValidateUrlWithUri(query))
                SystemUtils.StartApp(query);

            else
                SystemUtils.StartApp(engines[engine_name] + query);
        }
    }

    public static void Read(string[] args)
    {
        void err(string filename)
        {
            new Error($"{filename}: No such file or directory", "runtime error");
        }

        var parser = new Argparse("read", "Displays the contents of a text file.", err);
        parser.Add(["-l", "--line"], "Shows information about a specific line", default_value: "-1");
        var parsed_args = parser.Parse(Utils.Array.Reduce(args));

        // Get all the files to read from.
        string filepath = "";
        int line_to_read = -1;
        foreach (var arg in parsed_args)
        {
            string[] filename = arg.Names;
            if (filename.Contains("-l") && arg.KnownType == "Known" && !int.TryParse(arg.Value, out line_to_read))
            {
                new Error($"{line_to_read}: Invalid line number", "runtime error");
                return;
            }

            else if (arg.KnownType == "Unknown")
                filepath = filename.First();
        }

        // Read the files.
        string[] lines = FileIO.FileSystem.ReadAllLines(filepath);
        if (line_to_read < 1)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                Terminal.Print($"{i+1}. ", ConsoleColor.DarkGray, false);
                Console.WriteLine(lines[i]);
            }
        }

        else
        {
            if (line_to_read-1 < lines.Length)
            {
                Terminal.Print($"{line_to_read}. ", ConsoleColor.DarkGray, false);
                Console.WriteLine(lines[line_to_read-1]);
            }
        }
    }


    public static void Commit(string[] args)
    {
        void err(string argument)
        {
            new Error($"{argument}: Invalid argument.", "runtime error");
        }

        // Initialize the parser.
        var parser = new Argparse("commit", "Edit the contents of a text file", err);
        parser.Add(["-l", "--line"], "Edit specific line in a text file", default_value: "-1");
        var parsed_args = parser.Parse(Utils.Array.Reduce(args));

        // Get all the filenames and the data to commit to.
        string filepath = "";
        int line_to_read = -1;
        List<string> Content_to_commit = new();
        foreach (var arg in parsed_args)
        {
            string[] filenames = arg.Names;
            if (filenames.Contains("-l") && arg.KnownType == "Known" && !int.TryParse(arg.Value, out line_to_read))
            {
                new Error($"{line_to_read}: Invalid line number", "runtime error");
                return;
            }

            else if (arg.KnownType == "Unknown" && Utils.String.IsEmpty(filepath))
                filepath = filenames.First();

            else if (arg.KnownType == "Unknown")
                Content_to_commit.Add(filenames.First());
        }

        // Write/Overwrite the files with their respective data.
        if (line_to_read == 0)
            FileIO.FileSystem.Overwrite(filepath, [.. Content_to_commit]);

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

    public static void WinRAR(string[] args)
    {
        var parser = new Argparse("zip", "Compress or Decompress files or folders", Error.UnrecognizedArgs);
        parser.Add(["-u", "--uncompress", "--decompress"], "Decompress zip files", is_flag: true);
        var parsed_args = parser.Parse(Utils.Array.Reduce(args));

        // Get all the filenames and the data to commit to.
        bool unzip = false;
        List<string> Content_to_zip = new();
        foreach (var arg in parsed_args)
        {
            string[] filenames = arg.Names;
            if (filenames.Contains("-u"))
                unzip = true;

            else if (arg.KnownType == "Unknown" && !Utils.String.IsEmpty(filenames.First()))
                Content_to_zip.Add(filenames.First());
        }

        // Compress/Decompress the files with their respective data.
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

    public static void Terminate(string[] appnames)
    {
        appnames = Utils.Array.Reduce(Utils.Array.Filter(appnames));

        if (Utils.Array.IsEmpty(appnames))
        {
            System.Diagnostics.Process[] all_process = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process p in all_process)
            {
                Console.Write("{0," + -Utils.Maths.CalculatePadding(100, 100) + "}", p.ProcessName);
                Terminal.Print($"Process ID: {p.Id}", ConsoleColor.DarkGray);
            }
        }

        else
        {
            System.Diagnostics.Process[] all_process = [];
            foreach (string appname in appnames)
            {
                string name = Utils.String.Strings(appname);

                if (Utils.String.IsEmpty(name))
                {
                    IntPtr handle = WindowsControl.WindowManager.GetForegroundWindow();
                    WindowsControl.WindowManager.GetWindowThreadProcessId(handle, out uint process_id);
                    all_process = [System.Diagnostics.Process.GetProcessById((int)process_id)];
                }

                else
                    all_process = System.Diagnostics.Process.GetProcessesByName(appname);
            }

            // Close the first instance of the process
            foreach (System.Diagnostics.Process process in all_process)
            {
                if (!process.CloseMainWindow())
                    process.Kill();
            }
        }
    }

    public static void ControlVolume(string[] vol_level)
    {
        if (Utils.Array.IsEmpty(vol_level))
        {
            Console.WriteLine(WindowsControl.SystemVolume.AudioManager.GetMasterVolume());
            return;
        }

        var parser = new Argparse("vol", "Control the host operating system volume", Error.UnrecognizedArgs);
        parser.Add(["-m"], "Mute/Unmute host operating system volume", is_flag: true);
        parser.Add(["-i"], "Increase/Decrease the volume by then given value", is_flag: false);
        var parsed_args = parser.Parse(Utils.Array.Reduce(vol_level));

        if (parsed_args[0].Names.Contains("-m"))
            WindowsControl.SystemVolume.AudioManager.ToggleMasterVolumeMute();

        else if (parsed_args[0].Names.Contains("-i"))
        {
            if (int.TryParse(parsed_args[0].Value, out int level))
            {
                if (level < -100 || level > 100)
                    new Error($"'{level}': Invalid volume level. The volume level must be between -100 and 100", "runtime error");

                else
                    WindowsControl.SystemVolume.AudioManager.StepMasterVolume(level);
            }
        }

        else
        {
            if (int.TryParse(parsed_args[0].Names.FirstOrDefault(), out int level))
            {
                if (level < 0 || level > 100)
                    new Error($"'{level}': Invalid volume level. The volume level must be between 0 and 100", "runtime error");

                else
                    WindowsControl.SystemVolume.AudioManager.SetMasterVolume(level);
            }
        }
    }

    public static void ItsMagic()
    {
        // Rickroll!!
        // https://youtu.be/dQw4w9WgXcQ

        SystemUtils.StartApp("https://youtu.be/dQw4w9WgXcQ");
    }

    public static void SwitchApp(string appid)
    {
        if (Utils.String.IsEmpty(appid))
        {
            System.Diagnostics.Process[] all_process = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process p in all_process)
            {
                if (!Utils.String.IsEmpty(p.MainWindowTitle))
                {
                    Console.Write("{0," + -Utils.Maths.CalculatePadding(100, 100) + "}", p.MainWindowTitle);
                    Terminal.Print($"Process ID: {p.Id}", ConsoleColor.DarkGray);
                }
            }
        }

        else
        {
            if (int.TryParse(appid, out int int_appid))
            {
                System.Diagnostics.Process process = System.Diagnostics.Process.GetProcessById(int_appid);
                WindowsControl.WindowManager.SetForegroundWindow(process.MainWindowHandle);
            }

            else
            {
                new Error($"Invalid App ID: {appid}. App ID must be a number.", "runtime error");
                Terminal.Print("Please try 'switch' to get an App ID", ConsoleColor.White);
            }
        }
    }
}
