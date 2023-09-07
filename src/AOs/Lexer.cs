using System.Data;
using System.Text.RegularExpressions;

class Lexer
{
    public List<string[]> Tokens = new();
    private readonly string line;

    public Lexer(string line)
    {
        this.line = line.Trim();
        Parse(Tokenizer());

        foreach (string[] toks in Tokens)
        {
            for (int i = 0; i < toks.Length; i++)
            {
                string tok = toks[i];
                if (tok.StartsWith("%") && tok.EndsWith("%") && tok.Length > 1)
                    toks[i] = SystemUtils.RunSysOrEnvApps(tok.Substring(1, tok.Length-2).ToLower());

                else if (Utils.String.IsString(tok) || tok == "∞" || double.TryParse(tok, out double _))
                    toks[i] = Utils.String.Strings(tok);
            }
        }

        // if (Utils.String.IsString(input_cmd) || input_cmd == "∞" || double.TryParse(input_cmd, out double _))
        //     input_cmd = Utils.String.Strings(input_cmd);

        // else if (input_cmd.StartsWith("%") && input_cmd.EndsWith("%"))
        //     input_cmd = SystemUtils.RunSysOrEnvApps(input_cmd.Substring(1, input_cmd.Length-2).ToLower());

        // for (int i = 0; i < input_args.Length; i++)
        // {
        //     string arg = input_args[i];
        //     input_args[i] = Utils.String.IsString(arg) ? Utils.String.Strings(arg) : SystemUtils.RunSysOrEnvApps(arg.ToLower());
        // }
    }

    private void Parse(string[] toks)
    {
        List<string> current_list = new();

        for (int i = 0; i < toks.Length; i++)
        {
            string tok = toks[i];

            if (tok == ";")
            {
                this.Tokens.Add(current_list.ToArray());
                current_list = new List<string>();
            }

            else if (Is_expr(tok))
            {
                string expr = tok;
                string whitespaces = "";

                i++;
                while (i < toks.Length && (Is_expr(toks[i]) || Utils.String.IsWhiteSpace(toks[i])))
                {
                    if (i < toks.Length-1 && Utils.String.IsWhiteSpace(toks[i]) && !Is_expr(toks[i+1]))
                    {
                        whitespaces = toks[i];
                        break;
                    }

                    expr += toks[i];
                    i++;
                }

                i--;

                current_list.Add(Evaluate(expr));
                if (Utils.String.IsWhiteSpace(whitespaces))
                    current_list.Add(whitespaces);
            }

            else
                current_list.Add(tok);
        }

        // Add the last sublist to the result list
        this.Tokens.Add(current_list.ToArray());
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

            else if (tok == "'" || tok == "\"")
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

                            continue;
                        }

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

            if (i == line.Length-1 && tok != ";" && !Utils.String.IsEmpty(tok))
            {
                tokens.Add(tok);
                tok = "";
            }
        }

        return tokens.ToArray();
    }

    private bool Is_expr(string str)
    {
        string expression_pattern = @"^[-+*/0-9().]+$";
        string operator_pattern = @"^[-+*/]+$";
        str = str.Replace(" ", "");

        return Regex.IsMatch(str, expression_pattern) && !Regex.IsMatch(str, operator_pattern);
    }

    private bool Is_operator(string str)
    {
        return str == "+" || str == "-" || str == "*" || str == "/" ||
               str == "%" || str == "(" || str == ")";
    }

    private bool Is_symbol(string c)
    {
        return c == ">" || c == "@" || c == "!" || c == ";";
    }

    private bool Is_valid_char_for_filename_in_windows(char c)
    {
        // "`-=~!@#$%^&()_+[]{};'.,"
        return c == '`' || c == '-' || c == '=' || c == '~' || c == '!' ||
               c == '@' || c == '$' || c == '%' || c == '^' || c == '&' ||
               c == '(' || c == ')' || c == '_' || c == '+' || c == '[' ||
               c == ']' || c == '{' || c == '}' || c == '.' || c == ',' ||
               c == '?';
    }

    private bool Is_identifier(string str)
    {
        if (Utils.String.IsEmpty(str))
            return false;

        for (int i = 0; i < str.Length; i++)
        {
            if (!char.IsLetterOrDigit(str[i]) && !Is_valid_char_for_filename_in_windows(str[i]))
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
