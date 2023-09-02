using System.Text;
Console.OutputEncoding = Encoding.UTF8;

new EntryPoint(args, main);

void main(Obsidian AOs, List<(string cmd, string[] args)> input)
{
    //TODO: Write a custom Argparse class/system to handle commands.
    // var parser = new Argparse("AOs", "A Command-line utility for improved efficiency and productivity.");
    // parser.Add(new string[]{ "cls", "clear" }, "Clear the screen", is_flag: true, method: AOs.ClearConsole);
    // parser.Add(new string[]{ "version", "ver", "-v" }, "Displays the AOs version.", is_flag: true, method: AOs.PrintVersion);
    // parser.Add(new string[]{ "about", "info" }, "About AOs", is_flag: true, method: parser.PrintHelp);
    // parser.Add(new string[]{ "shutdown" }, "Shutdown the host machine", is_flag: true, method: Features.Shutdown);
    // parser.Add(new string[]{ "restart" }, "Restart the host machine", is_flag: true, method: Features.Restart);
    // parser.Add(new string[]{ "quit", "exit" }, "Exit AOs", is_flag: true, method: Features.Exit);
    // parser.Add(new string[]{ "reload", "refresh" }, "Restart AOs", is_flag: true, method: Features.Refresh);
    // parser.Add(new string[]{ "credits" }, "Credits for AOs", is_flag: true, method: AOs.Credits);

    // // parser.Add(new string[]{ "shout", "echo" }, "Displays messages", default_value: "", is_flag: false, method: Features.Shout);
    // parser.Add(new string[]{ "shout", "echo" }, "Displays messages", is_flag: false, method: Features.Shout);
    // parser.Add(new string[]{ "history" }, "Displays the history of Commands.", default_value: "", is_flag: false, method: Features.GetSetHistory);

    // foreach (var i in input)
    // {
    //     string lowercase_cmd = i.cmd.ToLower();

    //     if (Utils.String.IsEmpty(i.cmd))
    //         continue;

    //     else if (lowercase_cmd == "help" || Argparse.IsAskingForHelp(lowercase_cmd))
    //         parser.GetHelp(i.args.FirstOrDefault(""));

    //     else if (i.cmd == "AOs1000")
    //         Console.WriteLine("AOs1000!\nCONGRATULATIONS! For hitting 1000 LINES OF CODE in AOs 1.3!\nIt was the first program to ever reach these many LINES OF CODE!");

    //     else if (i.cmd == "∞" || double.TryParse(i.cmd, out double _) || Utils.String.IsString(i.cmd))
    //         Console.WriteLine(Utils.String.Strings(i.cmd));

    //     else
    //     {
    //         string[] cmd_to_be_parsed = new string[]{ lowercase_cmd }.Concat(i.args).ToArray();
    //         var parsed_args = parser.Parse(cmd_to_be_parsed, error_func: (arg) => Error.Command(arg), cmd_name: lowercase_cmd);
    //         foreach (var arg in parsed_args)
    //         {
    //             Console.WriteLine($"[({string.Join(", ", arg.Names)}), {arg.Value}, is_flag: {arg.Is_flag}]");
    //             if (arg.Is_flag)
    //             {
    //                 var action = arg.Method as Action; // Cast the stored delegate to Action
    //                 action?.Invoke(); // Invoke the delegate with no arguments
    //             }

    //             else
    //             {
    //                 var action = arg.Method as Action<string[]>; // Cast the stored delegate to Action<string[]>
    //                 action?.Invoke(i.args); // Invoke the delegate with the provided arguments (i.args)
    //             }
    //         }
    //     }
    // }

    // shout "Hello world!";1+3;"1+2";
}
