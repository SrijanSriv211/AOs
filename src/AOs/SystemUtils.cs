using System.Diagnostics;
using System.Security.Principal;

class SystemUtils
{
    public Process process = new();
    private readonly bool is_AOs_admin = new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);

    public int CommandPrompt(string cmd_args)
    {
        process.StartInfo.FileName = Obsidian.default_else_shell;
        process.StartInfo.UseShellExecute = false;

        process.StartInfo.Arguments = $"/C {cmd_args}";

        if (is_AOs_admin)
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

        if (is_admin || is_AOs_admin)
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
        if (!Utils.String.IsEmpty(Environment.GetEnvironmentVariable(input_cmd)))
            return Environment.GetEnvironmentVariable(input_cmd);

        else
        {
            string[] file_exts = { ".exe", ".msi", ".bat", ".cmd" };
            foreach (string ext in file_exts)
            {
                string exec_name = input_cmd + ext;
                if (!Utils.String.IsEmpty(LocateEXE(exec_name.ToLower())))
                    return LocateEXE(exec_name.ToLower());
            }
        }

        return input_cmd;
    }

    public static string LocateEXE(string Filename)
    {
        string[] folder_paths = Environment.GetEnvironmentVariable("path")?.Split(';');
        foreach (string folder in folder_paths)
        {
            string exec_path = Path.Combine(folder, Filename);

            if (File.Exists(exec_path))
                return exec_path;
        }

        return "";
    }
}
