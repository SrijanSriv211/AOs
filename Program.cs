using System.Text;
Console.OutputEncoding = Encoding.UTF8;

new EntryPoint(args, main);

static void main(Obsidian AOs, List<(string cmd, string[] args)> input)
{
    Features features = new();
    Parser parser = new((arg) => Error.Command(arg));

    parser.Add(new string[]{ "cls", "clear" }, "Clear the screen", method: AOs.ClearConsole);
    parser.Add(new string[]{ "version", "ver", "-v" }, "Displays the AOs version.", method: AOs.PrintVersion);
    parser.Add(new string[]{ "about", "info" }, "About AOs", method: AOs.About);
    parser.Add(new string[]{ "shutdown" }, "Shutdown the host machine", method: features.Shutdown);
    parser.Add(new string[]{ "restart" }, "Restart the host machine", method: features.Restart);
    parser.Add(new string[]{ "quit", "exit" }, "Exit AOs", method: features.Exit);
    parser.Add(new string[]{ "reload", "refresh" }, "Restart AOs", method: features.Refresh);
    parser.Add(new string[]{ "credits" }, "Credits for AOs", method: AOs.Credits);

    parser.Add(new string[]{ "shout", "echo" }, "Displays messages", is_flag: false, method: features.Shout);
    parser.Add(new string[]{ "history" }, "Displays the history of Commands.", default_values: new string[]{""}, max_args_length: 1, method: features.GetSetHistory);
    parser.Add(new string[]{ "console", "terminal", "cmd" }, "Starts a new instance of the terminal.", default_values: new string[0], is_flag: false, method: features.Terminal);

    foreach (var i in input)
    {
        string lowercase_cmd = i.cmd.ToLower();

        if (Utils.String.IsEmpty(i.cmd))
            continue;

        else if (lowercase_cmd == "help" || Argparse.IsAskingForHelp(lowercase_cmd))
            parser.GetHelp(i.args ?? new string[]{""});

        else if (i.cmd == "AOs1000")
            Console.WriteLine("AOs1000!\nCONGRATULATIONS! For hitting 1000 LINES OF CODE in AOs 1.3!\nIt was the first program to ever reach these many LINES OF CODE!");

        else if (i.cmd == "∞" || double.TryParse(i.cmd, out double _) || Utils.String.IsString(i.cmd))
            Console.WriteLine(Utils.String.Strings(i.cmd));

        else
            parser.Execute(parser.Parse(lowercase_cmd, i.args));
    }
}
