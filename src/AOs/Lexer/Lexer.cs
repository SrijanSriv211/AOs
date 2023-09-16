partial class Lexer
{
    public List<string[]> Tokens = new();
    private readonly string line;

    public Lexer(string line)
    {
        this.line = line.Trim();
        Parse(Tokenizer());

        foreach (string[] tokens in Tokens)
        {
            for (int i = 0; i < tokens.Length; i++)
            {
                string token = tokens[i];
                if (token.StartsWith("%") && token.EndsWith("%"))
                    tokens[i] = SystemUtils.CheckForSysOrEnvApps(token);
            }
        }
    }
}
