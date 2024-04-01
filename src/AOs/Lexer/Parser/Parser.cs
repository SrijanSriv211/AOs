namespace Lexer
{
    partial class Parser(string line)
    {
        private readonly string line = line;

        public List<string[]> Parse(List<Tokenizer.Token> tokens)
        {
            List<string[]> Tokens = [];
            List<string> CurrentTokList = [];

            foreach (Tokenizer.Token tok in tokens)
            {
                if (tok.Type == Tokenizer.TokenType.EOL)
                {
                    if (tok.Name != ";")
                        break;

                    Tokens.Add([.. CurrentTokList]);
                    CurrentTokList = [];
                }

                else if (tok.Type == Tokenizer.TokenType.EXPR)
                    CurrentTokList.Add(Evaluate(tok.Name));

                else if (tok.Name.StartsWith("%") && tok.Name.EndsWith("%"))
                    CurrentTokList.Add(SystemUtils.CheckForEnvVarAndEXEs(tok.Name));

                else
                    CurrentTokList.Add(tok.Name);
            }

            return Tokens;
        }
    }
}
