partial class Terminal
{
    partial class ReadLine
    {
        private class Tokenizer(string line) : Lexer.Tokenizer(line)
        {
            private new int CheckForToken()
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
                    MakeString(tok.FirstOrDefault());
                    return AppendToken(TokenType.STRING);
                }

                return 0;
            }

            private new void MakeString(char string_literal)
            {
                while (i < line.Length && line[i] != string_literal)
                {
                    if (line[i] == '\\')
                    {
                        tok += line[i];
                        i++;
                        tok += line[i] switch
                        {
                            '\\' => "\\",
                            '"' => "\"",
                            '\'' => "'",
                            'n' => "\n",
                            '0' => "\0",
                            't' => "\t",
                            'r' => "\r",
                            'b' => "\b",
                            'a' => "\a",
                            'f' => "\f",
                            _ => line[i].ToString()
                        };
                    }

                    else
                        tok += line[i];

                    i++; // Move to next char
                }

                i++;
            }
        }
    }
}
