using System.Diagnostics;

class SystemUtils
{
    public Process process = new();

    public int CommandPrompt(string cmd_args)
    {
        process.StartInfo.FileName = Obsidian.default_else_shell;
        process.StartInfo.UseShellExecute = false;

        process.StartInfo.Arguments = $"/C {cmd_args}";

        if (Obsidian.is_admin)
            process.StartInfo.Verb = "runas";

        process.Start();
        process.WaitForExit();
        return process.ExitCode;
    }

    public void StartApp(string app_name, string[] app_args=null, bool is_admin=false)
    {
        process.StartInfo.UseShellExecute = true;
        process.StartInfo.FileName = app_name;
        process.StartInfo.CreateNoWindow = false;

        if (app_args != null)
            process.StartInfo.Arguments = string.Join("", app_args);

        if (is_admin || Obsidian.is_admin)
            process.StartInfo.Verb = "runas";

        try
        {
            process.Start();
        }

        catch (Exception e)
        {
            new Error($"Error: Cannot open the app.\n{e.Message}");
        }
    }

    public bool RunSysOrEnvApps(string input_cmd, string[] input_args)
    {
        string arguments = string.Join("", input_args);

        if (File.Exists(input_cmd))
        {
            // if (input_cmd.EndsWith(".aos"))
            // {
            //     string AOsBinaryFilepath = Process.GetCurrentProcess().MainModule.FileName;
            //     CommandPrompt($"{AOsBinaryFilepath} {arguments}");
            //     return true;
            // }

            // else
            // {
                StartApp(input_cmd, input_args);
                return true;
            // }
        }

        return false;
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
        string[] folder_paths = Environment.GetEnvironmentVariable("path")?.Split(';');
        foreach (string folder in folder_paths)
        {
            string exec_path = Path.Combine(folder, Filename);

            if (File.Exists(exec_path))
                return exec_path;
        }

        // ELSE CASE
        // code here.

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
