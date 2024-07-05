namespace Lexer
{
    partial class Tokenizer
    {
        public List<Token> tokens = [];
        public string line;

        private bool loop = true;
        private string tok = "";
        private int i;

        public Tokenizer(string line)
        {
            this.line = line;
            Tokenize();
        }

        public void Tokenize()
        {
            // Loop through all chars in the line.
            for (i = 0; i < line.Length; i++)
            {
                tok += line[i];
                CheckForToken();

                if (!loop)
                    break;
            }

            if (!Utils.String.IsEmpty(tok))
                CheckForToken();

            tok = "";
            AppendToken(TokenType.EOL);
        }

        private void CheckForToken()
        {
            // '#' means that the following text is a comment.
            if (tok == "#")
            {
                bool MakeComment(char _) => true;
                Advance(TokenType.COMMENT, MakeComment);
            }

            else if (Utils.String.IsWhiteSpace(tok))
                Advance(TokenType.WHITESPACE, char.IsWhiteSpace);

            // ';' will be used to separate two different commands. Useful for multiple commands in single line.
            else if (tok == ";")
            {
                i++;
                AppendToken(TokenType.EOL);
            }

            else if (">@!?:".Contains(tok.FirstOrDefault()))
            {
                i++;
                AppendToken(TokenType.SYMBOL);
            }

            else if (IsIdentifier(tok.FirstOrDefault()) && tok.Length == 1)
            {
                AdvanceChar(IsIdentifier);

                // Remove all the common chars from the Expr string and tok to calculate if the tok is an expr or identifier.
                string UncommonChars = string.Concat(tok.Where(ch => !" ()[]{}+-*/%._=0123456789".Contains(ch)));

                // If the token contains any other char then it's an identifier.
                TokenType type = !Utils.String.IsEmpty(UncommonChars) ? TokenType.IDENTIFIER : TokenType.EXPR;

                if (type == TokenType.EXPR)
                {
                    AdvanceChar(IsExpr);
                    // '_' can be used to seperate the numbers to make it more readable.
                    // I've taken this feature from python. It's a cool feature to have.
                    // For eg,
                    // 100_000_000 -> 100000000
                    tok = tok.Replace(" ", "").Replace("_", "");
                }

                AppendToken(type);
            }

            else if (tok == "\"" || tok == "'")
            {
                if (!MakeString(tok.FirstOrDefault()))
                {
                    tokens = [];
                    loop = false;
                }

                AppendToken(TokenType.STRING);
            }
        }

        private void ClearToken()
        {
            i--; // Move to previous char
            tok = "";
        }

        private void AppendToken(TokenType type)
        {
            tokens.Add(new Token(tok, type));
            ClearToken();
        }

        private void AdvanceChar(Func<char, bool> func)
        {
            i++;
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
    }
}
