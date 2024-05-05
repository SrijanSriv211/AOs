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
            ReadLineConfig Config = new() {
                Toggle_color_coding = EntryPoint.Settings.readline.color_coding,
                Toggle_autocomplete = EntryPoint.Settings.readline.auto_complete_suggestions,
                LeftCursorStartPos = Console.CursorLeft,
                TopCursorStartPos = Console.CursorTop,
                SyntaxHighlightCodes = {
                    {ReadLine.Tokenizer.TokenType.STRING, ConsoleColor.Yellow},
                    {ReadLine.Tokenizer.TokenType.STRING, ConsoleColor.Yellow},
                    {ReadLine.Tokenizer.TokenType.EXPR, ConsoleColor.Cyan},
                    {ReadLine.Tokenizer.TokenType.BOOL, ConsoleColor.Magenta},
                    {ReadLine.Tokenizer.TokenType.SYMBOL, ConsoleColor.White},
                    {ReadLine.Tokenizer.TokenType.COMMENT, ConsoleColor.DarkGray}
                }
            };
            CMD = Terminal.TakeInput(this.prompt, ConsoleColor.White, Config: Config).Trim();

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

        // Some lexer stuff.
        return new Lexer.Parser(CMD, new Lexer.Tokenizer(CMD).tokens).output;
    }
}
