using System.Diagnostics;

partial class SystemUtils
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
                TerminalColor.Print($"{i+1}. ", ConsoleColor.DarkGray, false);
                Console.WriteLine(AppNames[i]);
            }
        }

        catch (Exception e)
        {
            _ = new Error(e.Message, "runtime error");
            EntryPoint.CrashreportLog(e.ToString());
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
                _ = new Error($"Cannot find the app '{appname}'", "runtime error");
        }

        catch (Exception e)
        {
            _ = new Error(e.Message, "runtime error");
            EntryPoint.CrashreportLog(e.ToString());
        }
    }

    public static void Track(int time_to_pause_in_milliseconds = 1000, int total_seconds = 100, string description = "Waiting...")
    {
        Console.WriteLine(description);

        for (int time_elapsed = 1; time_elapsed <= total_seconds; time_elapsed++)
        {
            TerminalColor.Print($"\r{time_elapsed}", ConsoleColor.White, false);
            Thread.Sleep(time_to_pause_in_milliseconds);
        }

        Console.WriteLine();
    }
}
