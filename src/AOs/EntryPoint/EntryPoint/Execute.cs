partial class EntryPoint
{
    private void Main(List<(Lexer.Tokenizer.Token cmd, Lexer.Tokenizer.Token[] args)> input)
    {
        foreach (var i in input)
        {
            if (Utils.String.IsEmpty(i.cmd.Name) && i.cmd.Type == Lexer.Tokenizer.TokenType.EOL)
                continue;

            else if (i.cmd.Name.Equals("help", StringComparison.CurrentCultureIgnoreCase) || Argparse.IsAskingForHelp(i.cmd.Name.ToLower()))
                parser.GetHelp(i.args.Select(x => x.Name).ToArray() ?? [""]);

            else if (i.cmd.Name == "AOs1000" && i.cmd.Type == Lexer.Tokenizer.TokenType.IDENTIFIER)
            {
                Terminal.Print("AOs1000!", ConsoleColor.White);
                Terminal.Print("CONGRATULATIONS! For hitting 1000 LINES OF CODE in AOs 1.3!", ConsoleColor.White);
                Terminal.Print("It was my first ever program to reach these many LINES OF CODE!", ConsoleColor.White);
            }

            else if (i.cmd.Type == Lexer.Tokenizer.TokenType.EXPR)
                Console.WriteLine(i.cmd.Name);

            else if (Utils.Https.ValidateUrlWithUri(i.cmd.Name))
                SystemUtils.StartApp(i.cmd.Name);

            else
                this.parser.Execute(this.parser.Parse(i.cmd, i.args));

            Console.WriteLine();
        }
    }

    private void Execute()
    {
        while (true)
        {
            try
            {
                Main(AOs.TakeInput());
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
                Main(AOs.TakeInput(input));
        }

        catch (Exception e)
        {
            CrashreportLog(e.ToString());
        }
    }
}
