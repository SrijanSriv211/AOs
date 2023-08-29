using System.Diagnostics;

class Features
{
    public static void Shout(string[] args)
    {
        Console.WriteLine(string.Join(" ", Lexer.SimplifyString(args)));
    }

    public static void Exit()
    {
        Environment.Exit(0);
    }

    public static void Restart()
    {
        Shell.CommandPrompt("shutdown /r /t0");
    }

    public static void Shutdown()
    {
        Shell.CommandPrompt("shutdown /s /t0");
    }

    public static void Refresh()
    {
        string AOsBinaryFilepath = Process.GetCurrentProcess().MainModule.FileName;
        Shell.StartApp(AOsBinaryFilepath);
        Exit();
    }

    public static void About()
    {
        Console.WriteLine("Command-line utility for improved efficiency and productivity.");
    }
}
