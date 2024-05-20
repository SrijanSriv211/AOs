namespace Lexer
{
    partial class Tokenizer
    {
        public enum TokenType
        {
            EOL = 0,
            COMMENT = 0,
            WHITESPACE,
            IDENTIFIER,
            STRING,
            BOOL,
            EXPR,
            SYMBOL,
        }

        public struct Token(string Name, TokenType Type)
        {
            public string Name = Name;
            public TokenType Type = Type;
        }

        private bool MakeString(char string_literal)
        {
            i++;
            if (i >= line.Length)
            {
                string error_detail = "unexpected end of tokens after " + string_literal;
                Error.Syntax(line, tok, error_detail);
                ClearToken();
                return false;
            }

            tok = "";

            while (i < line.Length && line[i] != string_literal)
            {
                if (line[i] == '\\')
                {
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
                        _ => "\\" + line[i].ToString()
                    };
                }

                else
                    tok += line[i];

                i++; // Move to next char
            }

            if (i >= line.Length)
            {
                string error_detail = "missing terminating " + string_literal + " character";
                Error.Syntax(line, tok, error_detail);
                tok = "";
                return false;
            }

            i++;
            return true;
        }

        private static bool IsValidCharForFilenameInWindows(char ch)
        {
            return "`~!@$^&?()=+-*/%_.[],{}|:<>\\".Contains(ch);
        }

        private bool IsIdentifier(char ch)
        {
            // Check if a char is empty or not, if not then check if the char is a letter or a digit or a symbol.
            // If no in any of these cases then return false, otherwise true.
            if (Utils.String.IsEmpty(ch.ToString()) || !(char.IsLetterOrDigit(ch) || IsValidCharForFilenameInWindows(ch)))
                return false;

            return true;
        }
    }
}
