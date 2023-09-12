partial class Features
{
    public void SwitchElseShell(string shell_name)
    {
        if (Utils.String.IsEmpty(shell_name))
            Console.WriteLine(Obsidian.default_else_shell);

        else if (shell_name == "cmd")
            Obsidian.default_else_shell = "cmd.exe";

        else if (shell_name == "ps" || shell_name == "powershell")
            Obsidian.default_else_shell = "powershell.exe";

        else
            Error.UnrecognizedArgs(shell_name);
    }
}
