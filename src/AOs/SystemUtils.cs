using System.Diagnostics;

class SystemUtils
{
    public Process process = new();

    public void ListInstalledApps()
    {
        this.process.StartInfo.FileName = "powershell.exe";
        this.process.StartInfo.Arguments = $"\"Get-StartApps | Select-Object -ExpandProperty Name\"";

        this.process.StartInfo.UseShellExecute = false;
        this.process.StartInfo.RedirectStandardOutput = true;

        try
        {
            this.process.Start();

            string[] AppNames = Utils.Array.Trim(process.StandardOutput.ReadToEnd().Split("\n"));

            this.process.WaitForExit();

            for (int i = 0; i < AppNames.Length; i++)
            {
                new TerminalColor($"{i+1}. ", ConsoleColor.DarkGray, false);
                Console.WriteLine(AppNames[i]);
            }
        }

        catch (Exception e)
        {
            new Error(e.Message);
        }
    }

    public void FindAndRunInstalledApps(string appname)
    {
        this.process.StartInfo.FileName = "powershell.exe";
        this.process.StartInfo.Arguments = $"\"Get-StartApps {appname} | Select-Object -ExpandProperty AppID\"";

        this.process.StartInfo.UseShellExecute = false;
        this.process.StartInfo.RedirectStandardOutput = true;

        try
        {
            this.process.Start();
            string AppID = process.StandardOutput.ReadToEnd();
            this.process.WaitForExit();

            if (!Utils.String.IsEmpty(AppID))
                CommandPrompt($"start explorer {Path.Combine("shell:appsfolder", AppID)}");

            else
                new Error($"Cannot find the app '{appname}'");
        }

        catch (Exception e)
        {
            new Error(e.Message);
        }
    }

    public int CommandPrompt(string cmd_args, bool supress_error_msg=false)
    {
        this.process.StartInfo.FileName = Obsidian.default_else_shell;
        this.process.StartInfo.Arguments = $"/C {cmd_args}";
        this.process.StartInfo.UseShellExecute = false;

        if (supress_error_msg)
            this.process.StartInfo.RedirectStandardError = true;

        if (Obsidian.is_admin)
            this.process.StartInfo.Verb = "runas";

        try
        {
            this.process.Start();
            this.process.WaitForExit();
        }

        catch (Exception e)
        {
            new Error(e.Message);
        }

        return this.process.ExitCode;
    }

    public int CommandPrompt(string cmd_name, string[] cmd_args)
    {
        this.process.StartInfo.FileName = cmd_name;
        this.process.StartInfo.UseShellExecute = false;

        if (cmd_args != null)
            this.process.StartInfo.Arguments = string.Join("", cmd_args);

        if (Obsidian.is_admin)
            this.process.StartInfo.Verb = "runas";

        try
        {
            this.process.Start();
            this.process.WaitForExit();
        }

        catch (Exception e)
        {
            new Error(e.Message);
        }

        return this.process.ExitCode;
    }

    public void StartApp(string appname, string appargs=null, bool is_admin=false)
    {
        this.process.StartInfo.FileName = appname;
        this.process.StartInfo.UseShellExecute = true;
        this.process.StartInfo.CreateNoWindow = false;

        if (appargs != null)
            this.process.StartInfo.Arguments = appargs;

        if (is_admin || Obsidian.is_admin)
            this.process.StartInfo.Verb = "runas";

        try
        {
            this.process.Start();
        }

        catch (Exception e)
        {
            new Error($"Error: Cannot open the app.\n{e.Message}");
        }
    }

    public bool RunSysOrEnvApps(string input_cmd, string[] input_args)
    {
        if (File.Exists(input_cmd))
        {
            if (input_cmd.EndsWith(".aos"))
            {
                string AOsBinaryFilepath = Process.GetCurrentProcess().MainModule.FileName;
                CommandPrompt(AOsBinaryFilepath, new string[]{ $"\"{input_cmd}\"" });
                return true;
            }

            else
            {
                CommandPrompt(input_cmd, input_args);
                return true;
            }
        }

        else
        {
            int return_val = CommandPrompt($"{input_cmd} {string.Join("", input_args)}", true);
            return return_val == 0;
        }
    }

    public static string CheckForSysOrEnvApps(string input)
    {
        if (input.StartsWith("%") && input.EndsWith("%") && input.Length > 1 && !Utils.String.IsEmpty(Environment.GetEnvironmentVariable(input.Substring(1, input.Length-2).ToLower())))
            return Environment.GetEnvironmentVariable(input.Substring(1, input.Length-2).ToLower());

        else
        {
            string[] file_exts = { "", ".aos", ".exe", ".msi", ".bat", ".cmd" }; // "" empty string at the start of this array means that an extention as already passed.
            foreach (string ext in file_exts)
            {
                string exec_name = input + ext;
                if (!Utils.String.IsEmpty(LocateExecutable(exec_name.ToLower())))
                    return LocateExecutable(exec_name.ToLower());
            }
        }

        return input;
    }

    public static string LocateExecutable(string Filename)
    {
        List<string> folder_paths = Environment.GetEnvironmentVariable("path")?.Split(';').ToList();
        folder_paths.Add(Path.Combine(Obsidian.root_dir, "Files.x72\\etc\\PowerToys"));

        foreach (string folder in folder_paths)
        {
            string exec_path = Path.Combine(folder, Filename);

            if (File.Exists(exec_path))
                return exec_path;
        }

        return "";
    }

    public static void Track(int time_to_pause_in_milliseconds = 1000, int total_seconds = 100, string description = "Waiting...")
    {
        Console.WriteLine(description);

        for (int time_elapsed = 1; time_elapsed <= total_seconds; time_elapsed++)
        {
            Thread.Sleep(time_to_pause_in_milliseconds);
            new TerminalColor($"\r{time_elapsed}", ConsoleColor.White, false);
        }

        Console.WriteLine();
    }
}
