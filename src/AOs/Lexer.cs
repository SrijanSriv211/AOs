using System.Data;
using System.Text.RegularExpressions;

class Lexer
{
    public List<List<string>> Tokens = new();
    private readonly string line;

    public Lexer(string line)
    {
        this.line = line.Trim();
        Parse(Tokenizer());
    }

    private void Parse(string[] toks)
    {
        List<string> current_list = new();

        for (int i = 0; i < toks.Length; i++)
        {
            string tok = toks[i];

            if (tok == ";")
            {
                Tokens.Add(current_list);
                current_list = new List<string>();
            }

            else if (Is_operator(tok) && Is_identifier_string_only_search(toks[i+1].ToString()))
            {
                current_list.Add(tok + toks[i+1]);
                i++;
            }

            else
                current_list.Add(tok);
        }

        // Add the last sublist to the result list
        Tokens.Add(current_list);

        // dotnet run -- -c "   prompt -p ~\"Hello world!\"$ ;test;1+2;2-3 +34/9 *48 -ab - k"
        foreach (var i in Tokens)
        {
            foreach (var j in i)
                Console.WriteLine(j);

            Console.WriteLine("[---]");
        }
    }

    private string Evaluate(string expr)
    {
        string result = "";
        DataTable dt = new();

        expr = expr.Replace(" ", "");

        try
        {
            result = dt.Compute(expr, "").ToString();
            if (result == "∞")
                Error.ZeroDivision();
        }

        catch (Exception e)
        {
            string error_detail = e.Message;
            int colon_index = error_detail.IndexOf(':');

            if (colon_index >= 0)
                error_detail = error_detail.Substring(colon_index + 1).Trim();

            Error.Syntax(error_detail);
            result = "";
        }

        return result;
    }

    private string[] Tokenizer()
    {
        List<string> tokens = new();
        string tok = "";

        for (int i = 0; i < line.Length; i++)
        {
            tok += line[i].ToString();

            if (Utils.String.IsEmpty(tok))
            {
                i++;
                while (i < line.Length && Utils.String.IsEmpty(line[i].ToString()))
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

            else if (tok == "'" || tok == "\"")
            {
                char str_char_symbol = tok[0];

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
                    tok += line[i];

                    if (i >= line.Length)
                    {
                        string error_detail = "missing terminating " + str_char_symbol.ToString() + " character";
                        Error.Syntax(error_detail);
                    }
                }
                
                else
                {
                    string error_detail = "unexpected end of tokens after " + str_char_symbol.ToString();
                    Error.Syntax(error_detail);
                }

                tokens.Add(Utils.String.Strings(tok));
                tok = "";
            }

            else if (Is_symbol(tok))
            {
                tokens.Add(tok);
                tok = "";
            }

            else if (Is_expr(tok))
            {
                i++;
                while (i < line.Length && Is_expr(line[i].ToString()))
                {
                    if (i < line.Length-1 && Utils.String.WhiteSpace(line[i+1].ToString()))
                    {
                        tok += line[i];
                        i += 2;
                    }

                    if (i < line.Length-1 && Is_operator(line[i].ToString()) && Is_identifier_string_only_search(line[i+1].ToString()))
                    {
                        if (Utils.String.WhiteSpace(line[i-1].ToString()))
                            tok += "\n";

                        break;
                    }

                    tok += line[i];
                    i++;
                }

                i--;

                tokens.Add(tok);
                tok = "";
            }

            // else if (Is_operator(tok))
            // {
            //     i++;

            //     // Check if the next character is also an operator or a digit
            //     while (i + 1 < line.Length && (Is_operator(line[i].ToString()) || Is_float(line[i].ToString()) || Is_identifier(line[i].ToString())))
            //     {
            //         tok += line[i];
            //         i++;
            //     }

            //     i--;

            //     tokens.Add(tok);
            //     tok = "";
            // }

            // else if (Is_float(tok))
            // {
            //     i++;
            //     while (i < line.Length && Is_float(line[i].ToString()))
            //     {
            //         tok += line[i];
            //         i++;
            //     }

            //     i--;

            //     tokens.Add(tok);
            //     tok = "";
            // }

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

            else
            {
                tokens.Add(tok);
                tok = "";
            }

            if (i == line.Length-1 && tok != ";")
            {
                tokens.Add(tok);
                tok = "";
            }
        }

        return tokens.ToArray();
    }

    private bool Is_expr(string str)
    {
        return Is_float(str) || Is_operator(str);
    }

    private bool Is_operator(string str)
    {
        return str == "+" || str == "-" || str == "*" || str == "/" || str == "%" || str == "(" || str == ")";
    }

    private bool Is_symbol(string c)
    {
        return c == ">" || c == "@" || c == ";";
    }

    private bool Is_identifier(string str)
    {
        if (Utils.String.IsEmpty(str))
            return false;

        for (int i = 0; i < str.Length; i++)
        {
            if (!char.IsLetterOrDigit(str[i]) && str[i] != '_')
                return false;
        }

        return true;
    }

    private bool Is_identifier_string_only_search(string str)
    {
        if (Utils.String.IsEmpty(str))
            return false;

        for (int i = 0; i < str.Length; i++)
        {
            if (!char.IsLetter(str[i]) && str[i] != '_')
                return false;
        }

        return true;
    }

    private bool Is_float(string str)
    {
        if (Utils.String.IsEmpty(str))
            return false;

        int dot_count = 0;
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] == '.' && dot_count < 2)
            {
                dot_count++;
                continue;
            }

            else if (!char.IsDigit(str[i]))
                return false;
        }

        return true;
    }
}
