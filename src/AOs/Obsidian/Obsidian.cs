partial class Obsidian
{
    public string[] prompt_preset;
    public ConsoleColor current_foreground_color;

    public readonly string version;
    public readonly string username;

    private string prompt;

    public Obsidian()
    {
        default_else_shell = EntryPoint.Settings.default_else_shell ?? "cmd.exe";
        this.username = EntryPoint.Settings.username ?? Environment.GetEnvironmentVariable("username");

        this.prompt_preset = [];
        this.current_foreground_color = original_foreground_color;

        this.version = $"AOs 2024 [Version {version_no}]";
        this.prompt = "$ ";
    }

    public List<(Lexer.Tokenizer.Token cmd, Lexer.Tokenizer.Token[] args)> TakeInput(string input="")
    {
        string CMD = input.Trim();

        Console.ForegroundColor = current_foreground_color;
        if (Utils.String.IsEmpty(CMD))
        {
            // Set and print the prompt
            SetPrompt(this.prompt_preset);

            // Take input
            Dictionary<Creadf.Tokenizer.TokenType, ConsoleColor> SyntaxHighlightCodes = [];
            SyntaxHighlightCodes.Add(Creadf.Tokenizer.TokenType.STRING, ConsoleColor.Yellow);
            SyntaxHighlightCodes.Add(Creadf.Tokenizer.TokenType.EXPR, ConsoleColor.Cyan);
            SyntaxHighlightCodes.Add(Creadf.Tokenizer.TokenType.BOOL, ConsoleColor.Magenta);
            SyntaxHighlightCodes.Add(Creadf.Tokenizer.TokenType.SYMBOL, ConsoleColor.White);
            SyntaxHighlightCodes.Add(Creadf.Tokenizer.TokenType.COMMENT, ConsoleColor.DarkGray);
            SyntaxHighlightCodes.Add(Creadf.Tokenizer.TokenType.SEMICOLON, ConsoleColor.DarkGray);

            CreadfConfig config = new(
                LeftCursorStartPos: prompt.Length,
                TopCursorStartPos: Console.CursorTop,
                ToggleColorCoding: EntryPoint.Settings.readline.color_coding,
                ToggleAutoComplete: EntryPoint.Settings.readline.auto_complete_suggestions,
                Suggestions: GetAllSuggestions(),
                SyntaxHighlightCodes: SyntaxHighlightCodes,
                CreadfHistory: CreadfHistory
            );

            CMD = Terminal.TakeInput(Config: config, this.prompt, ConsoleColor.White).Trim();

            // Return an empty list if the input is empty
            if (Utils.String.IsEmpty(CMD))
                return []; // (cmd: "", args: new string[0])
        }

        //* This is an optional feature.
        // Basically, you can type any command with or without prefix '_' for eg,
        // 'diagxt' and '_diagxt' will do the same thing. This is here because
        // in the Python version of AOs (AOs 1.3) '_' as prefix was compulsory.
        // So this is basically just a simple easter egg or some hidden feature.
        if (CMD.First() == '_')
            CMD = CMD[1..].Trim();

        // Set history.
        History.Set(CMD);
        CreadfHistory.Add(CMD);

        // Some lexer stuff.
        return new Lexer.Parser(CMD, new Lexer.Tokenizer(CMD).tokens).output;
    }
}
