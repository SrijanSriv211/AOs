partial class Features(Obsidian AOs)
{
    private readonly Utils.Https https = new();
    private readonly SystemUtils sys_utils = new();
    private readonly Obsidian AOs = AOs;
    private readonly DeveloperFeatures developer_features = new();
    private readonly ExperimentalFeatures experimental_features = new();

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

    public void Lock()
    {
        sys_utils.StartApp(@"C:\WINDOWS\system32\rundll32.exe", "user32.dll,LockWorkStation");
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

    public void PrintAOsSettings()
    {
        TerminalColor.Print("Default-Else-Shell: ", ConsoleColor.White);
        TerminalColor.Print(EntryPoint.Settings.default_else_shell, ConsoleColor.Gray);
        Console.WriteLine();

        TerminalColor.Print("Username: ", ConsoleColor.White);
        TerminalColor.Print(EntryPoint.Settings.username ?? "None", ConsoleColor.Gray);
        Console.WriteLine();

        TerminalColor.Print("Startup Apps (", ConsoleColor.White, false);
        TerminalColor.Print(Path.Combine(Obsidian.root_dir, "Files.x72\\etc\\Startup"), ConsoleColor.DarkGray, false);
        TerminalColor.Print("):", ConsoleColor.White);
        if (!Utils.Array.IsEmpty(EntryPoint.Settings.startlist))
        {
            for (int i = 0; i < EntryPoint.Settings.startlist.Length; i++)
            {
                TerminalColor.Print($"{i+1}. ", ConsoleColor.DarkGray, false);
                Console.WriteLine("{0," + -Utils.Maths.CalculatePadding(i+1, 10) + "}", EntryPoint.Settings.startlist[i]);
            }
        }

        else
            Console.WriteLine("None");

        Console.WriteLine();
        TerminalColor.Print(Path.Combine(Obsidian.root_dir, "Files.x72\\root\\settings.json"), ConsoleColor.DarkGray);
    }

    public void About()
    {
        TerminalColor.Print(Obsidian.about_AOs, ConsoleColor.White);
        TerminalColor.Print("For more information go to ", ConsoleColor.DarkGray, false);
        TerminalColor.Print(Obsidian.AOs_repo_link, ConsoleColor.Cyan);
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
            $"POWERTOYS DIRECTORY   : {Path.Combine(Obsidian.root_dir, "Files.x72\\etc\\PowerToys")}",
             "",
            $"GITHUB REPO     : {Obsidian.AOs_repo_link}",
             "SYSTEM LANGUAGE : en-in; English (India)"
        ];

        TerminalColor.Print(string.Join("\n", details), ConsoleColor.White);
    }

    public void Scan()
    {
        if (Obsidian.is_admin)
        {
            sys_utils.CommandPrompt("sfc /scannow");
            sys_utils.CommandPrompt("DISM /Online /Cleanup-Image /CheckHealth");
            sys_utils.CommandPrompt("DISM /Online /Cleanup-Image /ScanHealth");
            sys_utils.CommandPrompt("DISM /Online /Cleanup-image /Restorehealth");
            TerminalColor.Print($"Please check '{SystemUtils.CheckForEnvVarAndEXEs("%windir%")}\\Logs\\CBS\\CBS.log' ", ConsoleColor.White, false);
            TerminalColor.Print($"and '{SystemUtils.CheckForEnvVarAndEXEs("%windir%")}\\Logs\\DISM\\dism.log' for more details.", ConsoleColor.White);
        }

        else
        {
            _ = new Error("Please run AOs as Administrator to scan the integrity of all protected system files.", "runtime error");
            TerminalColor.Print("Type", ConsoleColor.Gray, false);
            TerminalColor.Print(" 'admin' ", ConsoleColor.White, false);
            TerminalColor.Print("to run AOs in Administrator", ConsoleColor.Gray);
        }
    }

    public void CheckForAOsUpdates()
    {
        string update_utility_path = Path.Combine(Obsidian.root_dir, "UPR.exe");
        if (File.Exists(update_utility_path))
        {
            Console.WriteLine("Checking for Updates");
            sys_utils.CommandPrompt($"\"{update_utility_path}\" {Obsidian.build_no}");
        }

        else
        {
            _ = new Error("Cannot find the update utility.", "runtime error");
            TerminalColor.Print("If the issue persists, please reinstall AOs. ", ConsoleColor.Gray, false);
            TerminalColor.Print("https://github.com/Light-Lens/AOs/releases/latest", ConsoleColor.White);
        }
    }

    public void ChangeToPrevDir()
    {
        Directory.SetCurrentDirectory("..");
    }

    public void ChangeCurrentDir(string dirname)
    {
        dirname = Utils.String.Strings(dirname);
        if (Utils.String.IsEmpty(dirname))
            TerminalColor.Print(Directory.GetCurrentDirectory(), ConsoleColor.White);

        else if (Directory.Exists(dirname))
            Directory.SetCurrentDirectory(dirname);

        else
            _ = new Error($"Folder with name '{dirname}' does not exist.", "runtime error");
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
            _ = new Error("`color` value was not defined. Please use a defined color value", "runtime error");
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

                    TerminalColor.Print(list_of_colors[i], (ConsoleColor)i);

                    Console.BackgroundColor = default_color;
                }

                else
                    TerminalColor.Print(list_of_colors[i], (ConsoleColor)i);
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
            sys_utils.CommandPrompt(string.Join(" ", args));
    }

    public void RunApp(string[] args)
    {
        if (Utils.Array.IsEmpty(args))
            sys_utils.StartApp(Obsidian.default_else_shell);

        else
        {
            string appname = args.First();
            string app_args = args.Length > 1 ? string.Join(" ", Utils.Array.Trim(args.Skip(1).ToArray())) : null;
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

    public void LS(string[] dirname)
    {
        dirname = Utils.Utils.SimplifyString(Utils.Array.Reduce(dirname));

        static void ShowDirs(string dir)
        {
            TerminalColor.Print($"{dir}\n", ConsoleColor.White);
            string[] entries = FileIO.FolderSystem.Read(dir);

            for (int i = 0; i < entries.Length; i++)
            {
                int padding = Utils.Maths.CalculatePadding(i+1, 50);

                if (i == 0)
                {
                    TerminalColor.Print(string.Format("{0," + -padding + "}", "Name") + "   Type\t", ConsoleColor.White, false);
                    TerminalColor.Print("    Creation Time\t", ConsoleColor.White, false);
                    Console.Write("       ");
                    TerminalColor.Print("Last Write Time", ConsoleColor.White);
                }

                TerminalColor.Print($"{i+1}. ", ConsoleColor.DarkGray, false);
                Console.Write("{0," + -padding + "}", entries[i]);

                if (File.Exists(entries[i]))
                    TerminalColor.Print("file\t", ConsoleColor.DarkGray, false);

                else if (Directory.Exists(entries[i]))
                    TerminalColor.Print("dir\t", ConsoleColor.DarkGray, false);

                TerminalColor.Print(File.GetCreationTime(entries[i]).ToString("dd-MM-yyyy   HH:mm:ss"), ConsoleColor.DarkGray, false);
                Console.Write("       ");
                TerminalColor.Print(File.GetLastWriteTime(entries[i]).ToString("dd-MM-yyyy   HH:mm:ss"), ConsoleColor.DarkGray);
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

    public void Touch(string[] content_to_create)
    {
        content_to_create = Utils.Utils.SimplifyString(Utils.Array.Reduce(content_to_create));

        foreach (string content in content_to_create)
        {
            if (content.ToLower() == "con")
                TerminalColor.Print("Hello CON!", ConsoleColor.White);

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
                TerminalColor.Print("Don't Delete CON!", ConsoleColor.Cyan);

            else if (Directory.Exists(content))
                FileIO.FolderSystem.Delete(content);

            else if (File.Exists(content))
                FileIO.FileSystem.Delete(content);

            else
            {
                _ = new Error($"'{content}' does not exist", "runtime error");
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
            TerminalColor.Print("Hello CON!", ConsoleColor.White);

        else if (Directory.Exists(old_name))
            FileIO.FolderSystem.Move(old_name, new_name);

        else if (File.Exists(old_name))
            FileIO.FileSystem.Move(old_name, new_name);

        else
            _ = new Error($"'{old_name}' does not exist", "runtime error");
    }

    public void Copy(string[] content_to_rename)
    {
        content_to_rename = Utils.Utils.SimplifyString(Utils.Array.Reduce(content_to_rename));

        string old_name = content_to_rename[0];
        string new_name = content_to_rename[1];

        if (old_name.ToLower() == "con" || new_name.ToLower() == "con")
            TerminalColor.Print("Hello CON!", ConsoleColor.White);

        else if (Directory.Exists(old_name))
            FileIO.FolderSystem.Copy(old_name, new_name);

        else if (File.Exists(old_name))
            FileIO.FileSystem.Copy(old_name, new_name);

        else
            _ = new Error($"'{old_name}' does not exist", "runtime error");
    }

    public void Pixelate(string[] websites_to_open)
    {
        static bool ValidateUrlWithUri(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult) && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }

        static void err(string website_url)
        {
            _ = new Error($"Can't open the website '{website_url}': Invalid URL", "runtime error");
        }

        string engine_name = "google", city_name = "muzaffarpur";
        var parser = new Argparse("pixelate", "Start a website in a web browser", err);
        parser.Add(["-e", "--engine"], "Search for a query on a specific search engine (google, bing, duckduckgo, youtube, wikipedia)", default_value: engine_name);
        parser.Add(["-w", "--weather"], "Display today's weather in a city", default_value: city_name);
        parser.Add(["-t", "--temp", "--temperature"], "Display today's temperature in a city", default_value: city_name);
        var parsed_args = parser.Parse(Utils.Array.Reduce(Utils.Array.Filter(websites_to_open)));

        List<string> queries = new();
        foreach (var arg in parsed_args)
        {
            string[] websites = Utils.Utils.SimplifyString(arg.Names);
            if (websites.Contains("-e"))
                engine_name = arg.Value;

            else if (websites.Contains("-w"))
            {
                TerminalColor.Print($"Fetching today's weather report for {arg.Value}", ConsoleColor.White);
                string weather = https.HttpsClient($"https://wttr.in/{arg.Value}?format=%C");
                Console.WriteLine(weather);
            }

            else if (websites.Contains("-t"))
            {
                TerminalColor.Print($"Fetching today's temperature report for {arg.Value}", ConsoleColor.White);
                string temp = https.HttpsClient($"https://wttr.in/{arg.Value}?format=%t");
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
            _ = new Error($"{filename}: No such file or directory", "runtime error");
        }

        var parser = new Argparse("read", "Displays the contents of a text file.", err);
        parser.Add(["-l", "--line"], "Shows information about a specific line", default_value: "-1");
        var parsed_args = parser.Parse(Utils.Array.Reduce(args));

        // Get all the files to read from.
        string filepath = "";
        int line_to_read = -1;
        foreach (var arg in parsed_args)
        {
            string[] filename = Utils.Utils.SimplifyString(arg.Names);
            if (filename.Contains("-l") && arg.KnownType == "Known" && !int.TryParse(arg.Value, out line_to_read))
            {
                _ = new Error($"{line_to_read}: Invalid line number", "runtime error");
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
                TerminalColor.Print($"{i+1}. ", ConsoleColor.DarkGray, false);
                Console.WriteLine(lines[i]);
            }
        }

        else
        {
            if (line_to_read-1 < lines.Length)
            {
                TerminalColor.Print($"{line_to_read}. ", ConsoleColor.DarkGray, false);
                Console.WriteLine(lines[line_to_read-1]);
            }
        }
    }


    public void Commit(string[] args)
    {
        static void err(string argument)
        {
            _ = new Error($"{argument}: Invalid argument.", "runtime error");
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
            string[] filenames = Utils.Utils.SimplifyString(arg.Names);
            if (filenames.Contains("-l") && arg.KnownType == "Known" && !int.TryParse(arg.Value, out line_to_read))
            {
                _ = new Error($"{line_to_read}: Invalid line number", "runtime error");
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

    public void WinRAR(string[] args)
    {
        var parser = new Argparse("zip", "Compress or Decompress files or folders", Error.UnrecognizedArgs);
        parser.Add(["-u", "--uncompress", "--decompress"], "Decompress zip files", is_flag: true);
        var parsed_args = parser.Parse(Utils.Array.Reduce(args));

        // Get all the filenames and the data to commit to.
        bool unzip = false;
        List<string> Content_to_zip = new();
        foreach (var arg in parsed_args)
        {
            string[] filenames = Utils.Utils.SimplifyString(arg.Names);
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

    public void Terminate(string[] appnames)
    {
        appnames = Utils.Array.Reduce(Utils.Array.Filter(appnames));

        if (Utils.Array.IsEmpty(appnames))
        {
            System.Diagnostics.Process[] all_process = System.Diagnostics.Process.GetProcesses();
            foreach (System.Diagnostics.Process p in all_process)
            {
                Console.Write("{0," + -Utils.Maths.CalculatePadding(100, 100) + "}", p.ProcessName);
                TerminalColor.Print($"Process ID: {p.Id}", ConsoleColor.DarkGray);
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
                    IntPtr handle = WindowManager.GetForegroundWindow();
                    WindowManager.GetWindowThreadProcessId(handle, out uint process_id);
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

    public void Filer(string[] args)
    {
        string filer_path = Path.Combine(Obsidian.root_dir, "filer_cli.exe");
        if (File.Exists(filer_path))
            sys_utils.CommandPrompt($"\"{filer_path}\" {string.Join("", args)}");

        else
        {
            _ = new Error("Cannot find the Filer.", "runtime error");
            TerminalColor.Print("If the issue persists, please reinstall AOs. ", ConsoleColor.Gray, false);
            TerminalColor.Print("https://github.com/Light-Lens/AOs/releases/latest", ConsoleColor.White);
        }
    }

    public void ControlVolume(string[] vol_level)
    {
        if (Utils.Array.IsEmpty(vol_level))
        {
            Console.WriteLine(WindowsVolumeControl.AudioManager.GetMasterVolume());
            return;
        }

        var parser = new Argparse("vol", "Control the host operating system volume", Error.UnrecognizedArgs);
        parser.Add(["-m"], "Mute/Unmute host operating system volume", is_flag: true);
        parser.Add(["-i"], "Increase/Decrease the volume by then given value", is_flag: false);
        var parsed_args = parser.Parse(Utils.Array.Reduce(vol_level));

        if (parsed_args[0].Names.Contains("-m"))
            WindowsVolumeControl.AudioManager.ToggleMasterVolumeMute();

        else if (parsed_args[0].Names.Contains("-i"))
        {
            if (int.TryParse(parsed_args[0].Value, out int level))
            {
                if (level < -100 || level > 100)
                    _ = new Error($"'{level}': Invalid volume level. The volume level must be between -100 and 100", "runtime error");

                else
                    WindowsVolumeControl.AudioManager.StepMasterVolume(level);
            }
        }

        else
        {
            if (int.TryParse(parsed_args[0].Names.FirstOrDefault(), out int level))
            {
                if (level < 0 || level > 100)
                    _ = new Error($"'{level}': Invalid volume level. The volume level must be between 0 and 100", "runtime error");

                else
                    WindowsVolumeControl.AudioManager.SetMasterVolume(level);
            }
        }
    }

    public void DevCMD(string[] args)
    {
        // Split the Toks into a cmd and Args variable and array respectively.
        string input_cmd = Utils.String.Strings(args.FirstOrDefault());
        string[] input_args = Utils.Array.Trim(args.Skip(1).ToArray());

        // Execute the developer commands.
        if (Utils.String.IsEmpty(input_cmd))
            return;

        else if (input_cmd.ToLower() == "help" || Argparse.IsAskingForHelp(input_cmd.ToLower()))
            this.developer_features.parser.GetHelp(input_args ?? [""]);

        else
            this.developer_features.parser.Execute(this.developer_features.parser.Parse(input_cmd, input_args));
    }

    public void ExperimentCMD(string[] args)
    {
        // Split the Toks into a cmd and Args variable and array respectively.
        string input_cmd = Utils.String.Strings(args.FirstOrDefault());
        string[] input_args = Utils.Array.Trim(args.Skip(1).ToArray());

        // Execute the developer commands.
        if (Utils.String.IsEmpty(input_cmd))
            return;

        else if (input_cmd.ToLower() == "help" || Argparse.IsAskingForHelp(input_cmd.ToLower()))
            this.experimental_features.parser.GetHelp(input_args ?? [""]);

        else
            this.experimental_features.parser.Execute(this.experimental_features.parser.Parse(input_cmd, input_args));
    }
}
