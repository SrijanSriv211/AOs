namespace Lexer
{
    partial class Tokenizer
    {
        public List<Token> tokens = [];

        private string line;
        private string tok = "";
        private int i;

        public Tokenizer(string line)
        {
            this.line = line;

            // Loop through all chars in the line.
            for (i = 0; i < line.Length; i++)
            {
                int tok_state = CheckForToken(tok);
                if (tok_state == -1)
                    break;

                else if (tok_state == 0)
                    tok += line[i];
            }

            if (!Utils.String.IsEmpty(tok))
                CheckForToken(tok);

            tok = "";
            AppendToken(TokenType.EOL);
        }

        private void ClearToken()
        {
            i--; // Move to previous char
            tok = "";
        }

        private void AppendToken(TokenType type)
        {
            tokens.Add(new Token(tok.Trim(), type));
            ClearToken();
        }

        private void AdvanceChar(Func<char, bool> func)
        {
            while (i < line.Length && func(line[i]))
            {
                tok += line[i];
                i++; // Move to next char
            }
        }

        private void Advance(TokenType type, Func<char, bool> func)
        {
            AdvanceChar(func);
            AppendToken(type);
        }

        private int CheckForToken(string tok)
        {
            // '#' means that the following text is a comment.
            if (tok == "#")
                return -1;

            else if (Utils.String.IsWhiteSpace(tok))
            {
                ClearToken(); // Remove whitespaces from the line.
                return 1;
            }

            // ';' will be used to separate two different commands.
            // Useful for multiple commands in single line.
            else if (tok == ";")
            {
                AppendToken(TokenType.EOL);
                return 1;
            }

            else if (">@!".Contains(tok.FirstOrDefault()))
            {
                AppendToken(TokenType.SYMBOL);
                return 1;
            }

            else if (IsIdentifier(tok.FirstOrDefault()) && !IsExpr(tok.FirstOrDefault()) && tok.Length == 1)
            {
                AdvanceChar(IsIdentifier);
                AppendToken(TokenType.IDENTIFIER);
                return 1;
            }

            else if (IsExpr(tok.FirstOrDefault()) && tok.Length == 1)
            {
                AdvanceChar(IsExprWithLetter);
                // If the token contains any letter then it's an identifier.
                // Remove whitespace from token only when it's an expr.
                tok = tok.Any(char.IsLetter) ? tok : tok.Replace(" ", "");
                AppendToken(tok.Any(char.IsLetter) ? TokenType.IDENTIFIER : TokenType.EXPR);
                return 1;
            }

            else if (tok == "\"" || tok == "'")
            {
                if (!MakeString(tok.FirstOrDefault()))
                {
                    tokens = [];
                    return -1;
                }

                AppendToken(TokenType.STRING);
                return 1;
            }

            return 0;
        }
    }
}
