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
                // Remove whitespaces from the line.
                if (Utils.String.IsWhiteSpace(tok))
                    ClearToken();

                // '#' means that the following text is a comment.
                else if (tok == "#")
                    break;

                // ';' will be used to separate two different commands.
                // Useful for multiple commands in single line.
                else if (tok == ";")
                    AppendToken(TokenType.EOL);

                // '$' will be used to specify the index of args passed from the command-line.
                // It is useful for AOs scripts giving them the ability to take command-line arguments and,
                // change the behaviour of the script accordingly.
                else if (tok == "$")
                    Advance(TokenType.IDENTIFIER, char.IsDigit);

                // Advance if the current char token is an identifer
                else if (IsIdentifier(tok.FirstOrDefault()))
                {
                    AdvanceChar(IsIdentifier);

                    foreach (char ch in tok)
                    {
                        if (IsNumber(ch))
                            break;
                    }
                }

                // /*
                // ------------------------------------------------------
                // ----------------------- MATH TOK ---------------------
                // ------------------------------------------------------
                // */

                // // Advance if the current char token is a symbol
                // else if (IsSymbol(tok.FirstOrDefault()))
                //     Advance(TokenType.SYMBOL, IsSymbol);

                // // Advance if the current char token is an operator
                // else if (IsOperator(tok.FirstOrDefault()))
                //     Advance(TokenType.OPERATOR, IsOperator);

                // // Advance if the current char token is a chain: (), [], {}
                // else if (IsChainableSymbol(tok.FirstOrDefault()))
                //     AppendToken(TokenType.CHAIN);

                // else if (char.IsDigit(tok.FirstOrDefault()))
                //     Advance(TokenType.NUMBER, IsNumber);

                else
                    tok += line[i];
            }

            AppendToken(TokenType.EOL);
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
            while (i < this.line.Length && func(this.line[i]))
            {
                tok += this.line[i];
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

// partial class Lexer
// {
//     private string[] Tokenizer()
//     {
//         List<string> tokens = [];
//         string tok = "";

//         for (int i = 0; i < line.Length; i++)
//         {
//             tok += line[i].ToString();

//             if (Utils.String.IsWhiteSpace(tok))
//             {
//                 i++;
//                 while (i < line.Length && Utils.String.IsWhiteSpace(line[i].ToString()))
//                 {
//                     tok += line[i];
//                     i++;
//                 }

//                 i--;

//                 tokens.Add(tok);
//                 tok = "";
//             }

//             else if (tok == "#")
//                 break;

//             // Designed to extract command line args.
//             else if (tok == "$")
//             {
//                 i++;
//                 while (i < line.Length && char.IsDigit(line[i]))
//                 {
//                     tok += line[i];
//                     i++;
//                 }

//                 i--;

//                 tokens.Add(tok);
//                 tok = "";
//             }

//             else if (tok == "'" || tok == "\"" || tok == "%")
//             {
//                 char str_char_symbol = tok.First();

//                 i++;
//                 if (i < line.Length)
//                 {
//                     while (i < line.Length && line[i] != str_char_symbol)
//                     {
//                         if (line[i] == '\\')
//                         {
//                             i++;
//                             tok += line[i] switch
//                             {
//                                 '\\' => "\\",
//                                 '"' => "\"",
//                                 '\'' => "'",
//                                 'n' => "\n",
//                                 '0' => "\0",
//                                 't' => "\t",
//                                 'r' => "\r",
//                                 'b' => "\b",
//                                 'a' => "\a",
//                                 'f' => "\f",
//                                 _ => "\\" + line[i].ToString(),
//                             };
//                         }

//                         else
//                             tok += line[i];

//                         i++;
//                     }

//                     if (i >= line.Length)
//                     {
//                         string error_detail = "missing terminating " + str_char_symbol.ToString() + " character";
//                         Error.Syntax(line, tok, error_detail);
//                         tok = "";
//                     }

//                     else
//                         tok += line[i];
//                 }

//                 else
//                 {
//                     string error_detail = "unexpected end of tokens after " + str_char_symbol.ToString();
//                     Error.Syntax(line, tok, error_detail);
//                     tok = "";
//                 }

//                 tokens.Add(tok);
//                 tok = "";
//             }

//             else if (Is_symbol(tok))
//             {
//                 tokens.Add(tok);
//                 tok = "";
//             }

//             else if (Is_operator(tok))
//             {
//                 i++;
//                 while (i < line.Length && (Is_operator(line[i].ToString()) || Is_identifier(line[i].ToString())))
//                 {
//                     tok += line[i];
//                     i++;
//                 }

//                 i--;

//                 tokens.Add(tok);
//                 tok = "";
//             }

//             else if (Is_identifier(tok))
//             {
//                 i++;
//                 while (i < line.Length && Is_identifier(line[i].ToString()))
//                 {
//                     tok += line[i];
//                     i++;
//                 }

//                 i--;

//                 tokens.Add(tok);
//                 tok = "";
//             }

//             else if (Is_float(tok))
//             {
//                 i++;
//                 while (i < line.Length && Is_float(line[i].ToString()))
//                 {
//                     tok += line[i];
//                     i++;
//                 }

//                 i--;

//                 tokens.Add(tok);
//                 tok = "";
//             }

//             else
//             {
//                 tokens.Add(tok);
//                 tok = "";
//             }

//             if (i == line.Length-1 && tok != ";" && !Utils.String.IsEmpty(tok))
//             {
//                 tokens.Add(tok);
//                 tok = "";
//             }
//         }

//         return tokens.ToArray();
//     }
// }
