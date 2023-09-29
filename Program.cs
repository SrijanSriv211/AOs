using System.Text;

Console.OutputEncoding = Encoding.UTF8;
new EntryPoint(args, main);

static void main(Obsidian AOs, Parser parser, List<(string cmd, string[] args)> input)
{
    foreach (var i in input)
    {
        if (Utils.String.IsEmpty(i.cmd))
            continue;

        else if (i.cmd.ToLower() == "help" || Argparse.IsAskingForHelp(i.cmd.ToLower()))
            parser.GetHelp(i.args ?? new string[]{""});

        else if (i.cmd == "AOs1000")
            new TerminalColor("AOs1000!\nCONGRATULATIONS! For hitting 1000 LINES OF CODE in AOs 1.3!\nIt was the first program to ever reach these many LINES OF CODE!", ConsoleColor.White);

        else
            parser.Execute(parser.Parse(i.cmd, i.args));
    }
}
