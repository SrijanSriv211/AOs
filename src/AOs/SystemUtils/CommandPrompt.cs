partial class SystemUtils
{
    public static int CommandPrompt(string cmd_args, bool supress_error_msg=false)
    {
        process.StartInfo.FileName = Obsidian.default_else_shell;
        process.StartInfo.Arguments = $"/C {cmd_args}";
        process.StartInfo.UseShellExecute = false;

        if (supress_error_msg)
            process.StartInfo.RedirectStandardError = true;

        if (Obsidian.is_admin)
            process.StartInfo.Verb = "runas";

        else
            process.StartInfo.Verb = "";

        try
        {
            process.Start();
            process.WaitForExit();
        }

        catch (Exception e)
        {
            new Error(e.Message, "runtime error");
            EntryPoint.CrashreportLog(e.ToString());
        }

        return process.ExitCode;
    }

    public static int CommandPrompt(string cmd_name, string[] cmd_args)
    {
        process.StartInfo.FileName = cmd_name;
        process.StartInfo.UseShellExecute = false;

        if (cmd_args != null)
            process.StartInfo.Arguments = string.Join("", cmd_args);

        if (Obsidian.is_admin)
            process.StartInfo.Verb = "runas";

        else
            process.StartInfo.Verb = "";

        try
        {
            process.Start();
            process.WaitForExit();
        }

        catch (Exception e)
        {
            new Error(e.Message, "runtime error");
            EntryPoint.CrashreportLog(e.ToString());
        }

        return process.ExitCode;
    }

    public static void StartApp(string appname, string appargs=null, bool is_admin=false)
    {
        process.StartInfo.FileName = appname;
        process.StartInfo.UseShellExecute = true;
        process.StartInfo.CreateNoWindow = false;

        if (appargs != null)
            process.StartInfo.Arguments = appargs;

        if (is_admin || Obsidian.is_admin)
            process.StartInfo.Verb = "runas";

        else
            process.StartInfo.Verb = "";

        try
        {
            process.Start();
        }

        catch (Exception e)
        {
            new Error($"Error: Cannot open the app.\n{e.Message}", "runtime error");
            EntryPoint.CrashreportLog(e.ToString());
        }
    }
}
