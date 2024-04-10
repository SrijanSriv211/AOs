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
            SetPrompt(this.prompt_preset);
            Terminal.Print(this.prompt, ConsoleColor.White, false);

            // CMD = Console.ReadLine().Trim();
            CMD = Terminal.TakeInput(cursor_start_pos: this.prompt.Length).Trim();

            if (Utils.String.IsEmpty(CMD))
                return []; // (cmd: "", args: new string[0])
        }

        if (CMD.First() == '_')
            CMD = CMD[1..].Trim();

        // Set history.
        History.Set(CMD);

        // Some lexer stuff.
        return new Lexer.Parser(CMD, new Lexer.Tokenizer(CMD).tokens).output;
    }
}
