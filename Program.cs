using System.Collections.Generic;
using System.Text;
Console.OutputEncoding = Encoding.UTF8;

SystemUtils sys_utils = new();
new EntryPoint(args, main);

void CheckForError(string input_cmd, string[] input_args)
{
    input_cmd = SystemUtils.CheckForSysOrEnvApps(input_cmd);

    for (int i = 0; i < input_args.Length; i++)
        input_args[i] = SystemUtils.CheckForSysOrEnvApps(input_args[i]);

    if (!sys_utils.RunSysOrEnvApps(input_cmd, input_args))
        Error.Command(input_cmd);
}

void main(Obsidian AOs, List<(string cmd, string[] args)> input)
{
    Features features = new(AOs, sys_utils);
    Parser parser = new(CheckForError);

    parser.Add(
        new string[]{ "!" }, "Switch the default-else shell",
        supported_args: new Dictionary<string[], string>
                        {
                            {new string[]{ "cmd" }, "Switch the default-else shell to cmd.exe"},
                            {new string[]{ "ps", "powershell" }, "Switch the default-else shell to powershell.exe"}
                        },
        default_values: new string[]{""}, max_args_length: 1, method: features.SwitchElseShell
    );

    parser.Add(new string[]{ "cls", "clear" }, "Clear the screen", method: AOs.ClearConsole);
    parser.Add(new string[]{ "version", "ver", "-v" }, "Displays the AOs version", method: AOs.PrintVersion);
    parser.Add(new string[]{ "about", "info" }, "About AOs", method: AOs.About);
    parser.Add(new string[]{ "shutdown" }, "Shutdown the host machine", method: features.Shutdown);
    parser.Add(new string[]{ "restart" }, "Restart the host machine", method: features.Restart);
    parser.Add(new string[]{ "quit", "exit" }, "Exit AOs", method: features.Exit);
    parser.Add(new string[]{ "reload", "refresh" }, "Restart AOs", method: features.Refresh);
    parser.Add(new string[]{ "credits" }, "Credit for AOs", method: AOs.Credits);
    parser.Add(new string[]{ "time", "clock" }, "Display current time", method: features.GetTime);
    parser.Add(new string[]{ "date", "calendar" }, "Display today's date", method: features.GetDate);
    parser.Add(new string[]{ "datetime" }, "Display today's time and date", method: features.GetDateTime);

    parser.Add(new string[]{ "shout", "echo" }, "Display messages", is_flag: false, method: features.Shout);

    parser.Add(
        new string[]{ "history" }, "Display the history of Commands",
        supported_args: new Dictionary<string[], string>
                        {
                            {new string[]{ "-c", "--clear" }, "Clear the history"},
                        },
        default_values: new string[]{""}, max_args_length: 1, method: features.GetSetHistory
    );

    parser.Add(new string[]{ ">", "console", "terminal", "cmd", "call" }, "Call a specified program or command, given the full or sysenv path in terminal", default_values: new string[0], method: features.RunInTerminal);
    parser.Add(new string[]{ "title" }, "Change the title for AOs window", default_values: new string[0]{}, method: features.ChangeTitle);
    parser.Add(new string[]{ "color" }, "Change the default AOs foreground colors", default_values: new string[]{""}, max_args_length: 1, method: features.ChangeColor);
    parser.Add(new string[]{ "wait", "timeout" }, "Suspend processing of a command for the given number of seconds", default_values: new string[]{""}, max_args_length: 1, method: features.Wait);
    parser.Add(new string[]{ "pause" }, "Suspend processing of a command and display the message", default_values: new string[0], method: features.Pause);
    parser.Add(new string[]{ "open", "run", "start" }, "Start a specified program or command, given the full or sysenv path", default_values: new string[0], method: features.RunApp);
    parser.Add(new string[]{ "cat", "allinstapps", "installedapps", "allinstalledapps" }, "Start an installed program from the system", default_values: new string[0]{}, method: features.Cat);
    parser.Add(new string[]{ "prompt" }, "Change the command prompt", default_values: new string[0]{}, method: features.ModifyPrompt);
    parser.Add(new string[]{ "ls", "dir" }, "Displays a list of files and subdirectories in a directory", default_values: new string[0]{}, method: features.LS);
    parser.Add(new string[]{ "cd" }, "Change the current directory", default_values: new string[]{""}, max_args_length: 1, method: features.ChangeCurrentDir);
    parser.Add(new string[]{ "cd.." }, "Change to previous directory", method: features.ChangeToPrevDir);
    parser.Add(new string[]{ "touch", "create" }, "Create a one or more files or folders", min_args_length: 1, method: features.Touch);
    parser.Add(new string[]{ "del", "delete", "rm", "rmdir" }, "Delete one or more files or folders", min_args_length: 1, method: features.Delete);
    parser.Add(new string[]{ "ren", "rename", "rn" }, "Rename a file or folder", min_args_length: 2, max_args_length: 2, method: features.Move);
    parser.Add(new string[]{ "mv", "move" }, "Move a file or folder", min_args_length: 2, max_args_length: 2, method: features.Move);
    parser.Add(new string[]{ "cp", "copy" }, "Copy a file or folder", min_args_length: 2, max_args_length: 2, method: features.Copy);
    parser.Add(new string[]{ "pixelate", "leaf", "corner" }, "Start a website in a web browser", min_args_length: 1, method: features.Pixelate);
    parser.Add(new string[]{ "read", "type" }, "Display the contents of a text file", min_args_length: 1, max_args_length: 3, method: features.Read);
    parser.Add(new string[]{ "commit", "write" }, "Edit the contents of a text file", min_args_length: 2, method: features.Commit);

    foreach (var i in input)
    {
        if (Utils.String.IsEmpty(i.cmd))
            continue;

        else if (i.cmd.ToLower() == "help" || Argparse.IsAskingForHelp(i.cmd.ToLower()))
            parser.GetHelp(i.args ?? new string[]{""});

        else if (i.cmd == "AOs1000")
            Console.WriteLine("AOs1000!\nCONGRATULATIONS! For hitting 1000 LINES OF CODE in AOs 1.3!\nIt was the first program to ever reach these many LINES OF CODE!");

        else
            parser.Execute(parser.Parse(i.cmd, i.args));
    }
}
