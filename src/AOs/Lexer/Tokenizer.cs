partial class Lexer
{
    private string[] Tokenizer()
    {
        List<string> tokens = new();
        string tok = "";

        for (int i = 0; i < line.Length; i++)
        {
            tok += line[i].ToString();

            if (Utils.String.IsWhiteSpace(tok))
            {
                i++;
                while (i < line.Length && Utils.String.IsWhiteSpace(line[i].ToString()))
                {
                    tok += line[i];
                    i++;
                }

                i--;

                tokens.Add(tok);
                tok = "";
            }

            else if (tok == "#")
                break;

            // Designed to extract command line args.
            else if (tok == "$")
            {
                i++;
                while (i < line.Length && char.IsDigit(line[i]))
                {
                    tok += line[i];
                    i++;
                }

                i--;

                tokens.Add(tok);
                tok = "";
            }

            else if (tok == "'" || tok == "\"" || tok == "%")
            {
                char str_char_symbol = tok.First();

                i++;
                if (i < line.Length)
                {
                    while (i < line.Length && line[i] != str_char_symbol)
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
                                _ => "\\" + line[i].ToString(),
                            };
                        }

                        else
                            tok += line[i];

                        i++;
                    }

                    if (i >= line.Length)
                    {
                        string error_detail = "missing terminating " + str_char_symbol.ToString() + " character";
                        Error.Syntax(error_detail);
                        tok = "";
                    }

                    else
                        tok += line[i];
                }
                
                else
                {
                    string error_detail = "unexpected end of tokens after " + str_char_symbol.ToString();
                    Error.Syntax(error_detail);
                    tok = "";
                }

                tokens.Add(tok);
                tok = "";
            }

            else if (Is_symbol(tok))
            {
                tokens.Add(tok);
                tok = "";
            }

            else if (Is_operator(tok))
            {
                i++;
                while (i < line.Length && (Is_operator(line[i].ToString()) || Is_identifier(line[i].ToString())))
                {
                    tok += line[i];
                    i++;
                }

                i--;

                tokens.Add(tok);
                tok = "";
            }

            else if (Is_identifier(tok))
            {
                i++;
                while (i < line.Length && Is_identifier(line[i].ToString()))
                {
                    tok += line[i];
                    i++;
                }

                i--;

                tokens.Add(tok);
                tok = "";
            }

            else if (Is_float(tok))
            {
                i++;
                while (i < line.Length && Is_float(line[i].ToString()))
                {
                    tok += line[i];
                    i++;
                }

                i--;

                tokens.Add(tok);
                tok = "";
            }

            else
            {
                tokens.Add(tok);
                tok = "";
            }

            if (i == line.Length-1 && tok != ";" && !Utils.String.IsEmpty(tok))
            {
                tokens.Add(tok);
                tok = "";
            }
        }

        return tokens.ToArray();
    }
}
