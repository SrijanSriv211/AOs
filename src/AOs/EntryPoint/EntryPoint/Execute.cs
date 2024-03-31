partial class EntryPoint
{
    private void Main(List<(string cmd, string[] args)> input)
    {
        foreach (var i in input)
        {
            if (Utils.String.IsEmpty(i.cmd))
                continue;

            else if (i.cmd.Equals("help", StringComparison.CurrentCultureIgnoreCase) || Argparse.IsAskingForHelp(i.cmd.ToLower()))
                this.parser.GetHelp(i.args ?? [""]);

            else if (i.cmd == "AOs1000")
            {
                TerminalColor.Print("AOs1000!", ConsoleColor.White);
                TerminalColor.Print("CONGRATULATIONS! For hitting 1000 LINES OF CODE in AOs 1.3!", ConsoleColor.White);
                TerminalColor.Print("It was the first program to ever reach these many LINES OF CODE!", ConsoleColor.White);
            }

            else
                this.parser.Execute(this.parser.Parse(i.cmd, i.args));
        }
    }

    private void Execute()
    {
        while (true)
        {
            try
            {
                this.Main(AOs.TakeInput());
            }

            catch (Exception e)
            {
                CrashreportLog(e.ToString());
            }
        }
    }

    private void Execute(string input)
    {
        try
        {
            if (!Utils.String.IsEmpty(input))
                this.Main(AOs.TakeInput(input));
        }

        catch (Exception e)
        {
            CrashreportLog(e.ToString());
        }
    }

    private void Execute(string[] inputs)
    {
        foreach (string input in inputs)
        {
            try
            {
                if (!Utils.String.IsEmpty(input))
                    this.Main(AOs.TakeInput(input));
            }

            catch (Exception e)
            {
                CrashreportLog(e.ToString());
            }
        }
    }
}
