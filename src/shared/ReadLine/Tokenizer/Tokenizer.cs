partial class ReadLine
{
    public partial class Tokenizer
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
                loop = false;
            }

            else if (tok == " ")
                Advance(TokenType.WHITESPACE, char.IsWhiteSpace);

            // ';' will be used to separate two different commands. Useful for multiple commands in single line.
            else if (tok == ";")
                AppendToken(TokenType.EOL);

            else if (">@!?:".Contains(tok.FirstOrDefault()))
                AppendToken(TokenType.SYMBOL);

            else if (IsIdentifier(tok.FirstOrDefault()) && tok.Length == 1)
            {
                AdvanceChar(IsIdentifier);

                // Remove all the common chars from the Expr string and tok to calculate if the tok is an expr or identifier.
                string UncommonChars = string.Concat(tok.Where(ch => !" ()[]{}+-*/%._=0123456789".Contains(ch)));

                // Remove whitespace from token only when it's an expr.
                tok = !Utils.String.IsEmpty(UncommonChars) ? tok : tok.Replace(" ", "");

                // If the token contains any letter then it's an identifier.
                TokenType type;

                if (!Utils.String.IsEmpty(UncommonChars))
                    type = (tok == "true" || tok == "false") ? TokenType.BOOL : TokenType.IDENTIFIER;

                else
                    type = TokenType.EXPR;

                AppendToken(type);
            }

            else if (tok == "\"" || tok == "'")
            {
                MakeString(tok.FirstOrDefault());
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
