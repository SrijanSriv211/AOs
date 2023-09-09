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

    parser.Add(new string[]{ "!" }, "Switch the default-else shell (cmd, powershell)", default_values: new string[]{""}, max_args_length: 1, method: features.SwitchElseShell);

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
    parser.Add(new string[]{ "datetime" }, "Display today's date", method: features.GetDateTime);

    parser.Add(new string[]{ "shout", "echo" }, "Display messages", is_flag: false, method: features.Shout);
    parser.Add(new string[]{ "history" }, "Display the history of Commands", default_values: new string[]{""}, max_args_length: 1, method: features.GetSetHistory);
    parser.Add(new string[]{ ">", "console", "terminal", "cmd", "call" }, "Call a specified program or command, given the full or sysenv path in terminal", default_values: new string[0], method: features.RunInTerminal);
    parser.Add(new string[]{ "title" }, "Change the title for AOs window", default_values: new string[]{""}, method: features.ChangeTitle);
    parser.Add(new string[]{ "color" }, "Change the default AOs foreground colors", default_values: new string[]{""}, max_args_length: 1, method: features.ChangeColor);
    parser.Add(new string[]{ "wait", "timeout" }, "Suspend processing of a command for the given number of seconds", default_values: new string[]{""}, max_args_length: 1, method: features.Wait);
    parser.Add(new string[]{ "pause" }, "Suspend processing of a command and display the message", default_values: new string[0], method: features.Pause);
    parser.Add(new string[]{ "open", "run", "start" }, "Start a specified program or command, given the full or sysenv path", default_values: new string[0], method: features.RunApp);

    foreach (var i in input)
    {
        if (Utils.String.IsEmpty(i.cmd))
            continue;

        else if (i.cmd.ToLower() == "help" || Argparse.IsAskingForHelp(i.cmd.ToLower()))
        {
            parser.GetHelp(i.args ?? new string[]{""});
        }

        else if (i.cmd == "AOs1000")
            Console.WriteLine("AOs1000!\nCONGRATULATIONS! For hitting 1000 LINES OF CODE in AOs 1.3!\nIt was the first program to ever reach these many LINES OF CODE!");

        else
            parser.Execute(parser.Parse(i.cmd, i.args));
    }
}
