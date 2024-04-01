partial class Obsidian
{
    public string[] prompt_preset;
    public ConsoleColor current_foreground_color;

    public readonly string version;

    private readonly SystemUtils sys_utils;
    private string prompt;

    public Obsidian()
    {
        this.prompt_preset = [];
        this.current_foreground_color = original_foreground_color;

        this.version = $"AOs 2024 [Version {version_no}]";
        this.sys_utils = new();

        this.prompt = "$ ";
    }

    public List<(string cmd, string[] args)> TakeInput(string input="")
    {
        List<(string cmd, string[] args)> output = [];
        string CMD = input.Trim();

        Console.ForegroundColor = current_foreground_color;
        if (Utils.String.IsEmpty(CMD))
        {
            SetPrompt(this.prompt_preset);
            TerminalColor.Print(this.prompt, ConsoleColor.White, false);

            CMD = Console.ReadLine().Trim();

            if (Utils.String.IsEmpty(CMD))
                return []; // (cmd: "", args: new string[0])
        }

        if (CMD.First() == '_')
            CMD = CMD.Substring(1).Trim();

        // Set history.
        History.Set(CMD);

        // Some lexer stuff.
        List<string[]> ListOfToks = new Lexer.Lexer(CMD).Tokens;
        foreach (string[] Toks in ListOfToks)
        {
            if (Utils.String.IsEmpty(Toks.FirstOrDefault()))
                continue;

            // Split the Toks into a cmd and Args variable and array respectively.
            string input_cmd = Utils.String.Strings(Toks.FirstOrDefault());
            string[] input_args = Utils.Array.Trim(Toks.Skip(1).ToArray());

            // Add input_cmd & input_args to output.
            output.Add((input_cmd, input_args));
        }

        return output;
    }
}
