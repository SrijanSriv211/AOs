namespace Lexer
{
    partial class Parser
    {
        public readonly List<(Tokenizer.Token cmd, Tokenizer.Token[] args)> output = [];

        private readonly string line;
        private readonly System.Data.DataTable dt = new();
        private readonly List<Tokenizer.Token> tokens;

        public Parser(string line, List<Tokenizer.Token> tokens)
        {
            this.line = line;
            this.tokens = tokens;

            Parse();
        }

        public void Parse()
        {
            List<Tokenizer.Token> CurrentTokens = [];

            foreach (Tokenizer.Token tok in tokens)
            {
                if (tok.Type == Tokenizer.TokenType.EOL)
                {
                    Tokenizer.Token cmd = CurrentTokens.FirstOrDefault();
                    Tokenizer.Token[] args = Trim(CurrentTokens.Skip(1).ToArray());

                    output.Add((cmd, args));
                    CurrentTokens = [];
                }

                else if (tok.Type == Tokenizer.TokenType.EXPR)
                    CurrentTokens.Add(new Tokenizer.Token(Evaluate(tok.Name), tok.Type));

                else if (tok.Type == Tokenizer.TokenType.STRING)
                    CurrentTokens.Add(new Tokenizer.Token(Utils.String.Strings(tok.Name), tok.Type));

                else if (tok.Name.StartsWith("%") && tok.Name.EndsWith("%") && tok.Type == Tokenizer.TokenType.IDENTIFIER)
                    CurrentTokens.Add(new Tokenizer.Token(SystemUtils.CheckForEnvVarAndEXEs(tok.Name), tok.Type));

                else if (tok.Type != Tokenizer.TokenType.COMMENT)
                    CurrentTokens.Add(tok);
            }
        }

        // https://stackoverflow.com/a/70991343/18121288
        // Trim the leading and trailing part of an array.
        //* This is a rewrite of Utils.Array.Trim() method, to work with Parser's CurrentTokens List of Tokenizer.Token
        private Tokenizer.Token[] Trim(Tokenizer.Token[] arr)
        {
            // Define predicate to test for non-empty strings
            static bool IsNotEmpty(string s) => !Utils.String.IsEmpty(s);

            var FirstIndex = Array.FindIndex(arr.Select(x => x.Name).ToArray(), IsNotEmpty);

            // Nothing to return if it's all whitespace anyway
            if (FirstIndex < 0)
                return [];

            var LastIndex = Array.FindLastIndex(arr.Select(x => x.Name).ToArray(), IsNotEmpty);

            // Calculate size of the relevant middle-section from the indices
            var NewArraySize = LastIndex - FirstIndex + 1;

            // Create new array and copy items to it
            var Results = new Tokenizer.Token[NewArraySize];
            Array.Copy(arr.ToArray(), FirstIndex, Results, 0, NewArraySize);

            return Results;
        }
    }
}
