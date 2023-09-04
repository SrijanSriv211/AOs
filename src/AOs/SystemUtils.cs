using System.Diagnostics;
using System.Text;

class SystemUtils
{
    public Process process = new();

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

    public void CommandPrompt(string cmd_args)
    {
        process.StartInfo.FileName = Obsidian.default_else_shell;
        process.StartInfo.UseShellExecute = false;

        process.StartInfo.Arguments = $"/C {cmd_args}";

        process.Start();
        process.WaitForExit();
    }
}
