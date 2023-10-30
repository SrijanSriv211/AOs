partial class Obsidian
{
    public void ClearConsole()
    {
        sys_utils.CommandPrompt("cls");
        TerminalColor.Print(version, ConsoleColor.Yellow, false);
        TerminalColor.Print($"  ({Environment.GetEnvironmentVariable("username")})", ConsoleColor.White);
    }
}
