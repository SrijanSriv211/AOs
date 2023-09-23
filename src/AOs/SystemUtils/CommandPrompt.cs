partial class SystemUtils
{
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
            EntryPoint.CrashreportLogging(e.ToString());
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
            EntryPoint.CrashreportLogging(e.ToString());
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
            EntryPoint.CrashreportLogging(e.ToString());
        }
    }
}
