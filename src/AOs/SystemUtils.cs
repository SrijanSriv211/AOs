using System.Diagnostics;

class SystemUtils
{
    public Process process = new();

    public int CommandPrompt(string cmd_args)
    {
        process.StartInfo.FileName = Obsidian.default_else_shell;
        process.StartInfo.UseShellExecute = false;

        process.StartInfo.Arguments = $"/C {cmd_args}";

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

        if (is_admin)
            process.StartInfo.Verb = "runas";

        try
        {
            process.Start();
        }

        catch (Exception e)
        {
            new Error($"Error: Cannot open the app.\n{e}");
        }
    }

    public static string RunSysOrEnvApps(string input_cmd)
    {
        // string[] file_exts = {".exe", ".msi", ".bat", ".cmd"};

        if (!Utils.String.IsEmpty(Environment.GetEnvironmentVariable(input_cmd)))
            return Environment.GetEnvironmentVariable(input_cmd);

        return input_cmd;
    }
}
