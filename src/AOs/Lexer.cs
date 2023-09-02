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
        List<string> tokens = new();

        for (int i = 0; i < toks.Length; i++)
        {
            string tok = toks[i];

            if (Is_expr(tok))
            {
                i++;

                while (i < toks.Length && Is_expr(tok))
                {
                    tok += toks[i];
                    i++;
                }

                i--;
            }

            tokens.Add(tok);
        }

        List<string> current_list = new();
        string pattern = @"^[-+*/0-9()]+$";
        string expr = "";

        foreach (string tok in tokens.ToArray())
        {
            if ((Regex.IsMatch(tok, pattern) && !Is_operator(tok)) || Utils.String.IsEmpty(tok))
            {
                if (!Utils.String.IsEmpty(tok))
                {
                    expr += tok;
                    continue;
                }
            }

            else if (!Utils.String.IsEmpty(expr))
            {
                current_list.Add(Evaluate(expr));
                expr = "";
            }

            // Create a new sublist when encountering a semicolon
            else if (tok == ";")
            {
                Tokens.Add(current_list);
                current_list = new List<string>();
            }

            current_list.Add(tok);

            // Create a new sublist when encountering a semicolon
        //     if (i == ";")
        //     {
        //         Tokens.Add(current_list);
        //         current_list = new List<string>();
        //     }

        //     else
        //         current_list.Add(i);
        }

        // Add the last sublist to the result list
        Tokens.Add(current_list);
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

            else if (Is_operator(tok))
            {
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
