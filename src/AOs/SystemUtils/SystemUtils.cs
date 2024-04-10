using System.Diagnostics;

partial class SystemUtils
{
    public static Process process = new();

    public static void Init__() => process = new();

    public static void ListInstalledApps()
    {
        process.StartInfo.FileName = "powershell.exe";
        process.StartInfo.Arguments = $"\"Get-StartApps | Select-Object -ExpandProperty Name\"";

        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;

        try
        {
            process.Start();

            string[] AppNames = Utils.Array.Trim(process.StandardOutput.ReadToEnd().Split("\n"));

            process.WaitForExit();

            for (int i = 0; i < AppNames.Length; i++)
            {
                Terminal.Print($"{i+1}. ", ConsoleColor.DarkGray, false);
                Console.WriteLine(AppNames[i]);
            }
        }

        catch (Exception e)
        {
            new Error(e.Message, "runtime error");
            EntryPoint.CrashreportLog(e.ToString());
        }
    }

    public static void FindAndRunInstalledApps(string appname)
    {
        process.StartInfo.FileName = "powershell.exe";
        process.StartInfo.Arguments = $"\"Get-StartApps {appname} | Select-Object -ExpandProperty AppID\"";

        process.StartInfo.UseShellExecute = false;
        process.StartInfo.RedirectStandardOutput = true;

        try
        {
            process.Start();
            string AppID = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            if (!Utils.String.IsEmpty(AppID))
                CommandPrompt($"start explorer {Path.Combine("shell:appsfolder", AppID)}");

            else
                new Error($"Cannot find the app '{appname}'", "runtime error");
        }

        catch (Exception e)
        {
            new Error(e.Message, "runtime error");
            EntryPoint.CrashreportLog(e.ToString());
        }
    }

    public static void Track(int time_to_pause_in_milliseconds = 1000, int total_seconds = 100, string description = "Waiting...")
    {
        Console.WriteLine(description);

        for (int time_elapsed = 1; time_elapsed <= total_seconds; time_elapsed++)
        {
            Terminal.Print($"\r{time_elapsed}", ConsoleColor.White, false);
            Thread.Sleep(time_to_pause_in_milliseconds);
        }

        Console.WriteLine();
    }
}
