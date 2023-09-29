partial class Obsidian
{
    public void ClearConsole()
    {
        sys_utils.CommandPrompt("cls");
        new TerminalColor(version, ConsoleColor.Yellow, false);
        new TerminalColor($"  ({Environment.GetEnvironmentVariable("username")})", ConsoleColor.White);
    }
}
