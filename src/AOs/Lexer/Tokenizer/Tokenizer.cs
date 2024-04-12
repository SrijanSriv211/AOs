namespace Lexer
{
    partial class Tokenizer
    {
        public List<Token> tokens = [];
        public string line;
        public bool disable_error = false;

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
                int tok_state = CheckForToken();
                if (tok_state == -1)
                    break;

                else if (tok_state == 0)
                    tok += line[i];
            }

            if (!Utils.String.IsEmpty(tok))
                CheckForToken();

            tok = "";
            AppendToken(TokenType.EOL);
        }

        private void ClearToken()
        {
            i--; // Move to previous char
            tok = "";
        }

        private int AppendToken(TokenType type)
        {
            tokens.Add(new Token(tok, type));
            ClearToken();
            return 1;
        }

        private void AdvanceChar(Func<char, bool> func)
        {
            while (i < line.Length && func(line[i]))
            {
                tok += line[i];
                i++; // Move to next char
            }
        }

        private int Advance(TokenType type, Func<char, bool> func)
        {
            AdvanceChar(func);
            return AppendToken(type);
        }

        private int CheckForToken()
        {
            // '#' means that the following text is a comment.
            if (tok == "#")
            {
                bool MakeComment(char _) => true;
                Advance(TokenType.COMMENT, MakeComment);
                return -1;
            }

            else if (Utils.String.IsWhiteSpace(tok))
                return Advance(TokenType.WHITESPACE, char.IsWhiteSpace);

            // ';' will be used to separate two different commands. Useful for multiple commands in single line.
            else if (tok == ";")
                return AppendToken(TokenType.EOL);

            else if (">@!?:".Contains(tok.FirstOrDefault()))
                return AppendToken(TokenType.SYMBOL);

            else if (IsIdentifier(tok.FirstOrDefault()) && tok.Length == 1)
            {
                AdvanceChar(IsIdentifier);

                // Remove all the common chars from the Expr string and tok to calculate if the tok is an expr or identifier.
                string UncommonChars = string.Concat(tok.Where(ch => !" ()[]{}+-*/%._=0123456789".Contains(ch)));

                // Remove whitespace from token only when it's an expr.
                tok = !Utils.String.IsEmpty(UncommonChars) ? tok : tok.Replace(" ", "");

                // If the token contains any letter then it's an identifier.
                return AppendToken(!Utils.String.IsEmpty(UncommonChars) ? TokenType.IDENTIFIER : TokenType.EXPR);
            }

            else if (tok == "\"" || tok == "'")
            {
                if (!MakeString(tok.FirstOrDefault()))
                {
                    tokens = [];
                    return -1;
                }

                return AppendToken(TokenType.STRING);
            }

            return 0;
        }
    }
}
