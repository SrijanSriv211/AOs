partial class Obsidian
{
    public void ClearConsole()
    {
        SystemUtils.CommandPrompt("cls");
        Terminal.Print(version, ConsoleColor.Yellow, false);
        Terminal.Print($"  ({username})", ConsoleColor.White);

        // https://stackoverflow.com/a/72575526/18121288
        // Check if AOs is running in Windows Terminal or using using the environment variable %WT_SESSION%
        // If AOs is running in Windows Terminal in it will return a terminal session id,
        // if not then it will return an empty string pointing that AOs is running in Windows Console Host or some other terminal,
        // and if that's the case then suggest the user to use AOs in Windows Terminal for better experience.
        if (Utils.String.IsEmpty(Environment.GetEnvironmentVariable("WT_SESSION")))
            Terminal.Print("For better experience please run AOs in Windows Terminal.", ConsoleColor.DarkRed);
    }
}
