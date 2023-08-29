using System.Data;

class Lexer
{
    public List<List<string>> Tokens = new List<List<string>>();
    public Lexer(string line)
    {
        List<string> CurrentList = new List<string>();

        string[] Toks = Parse(Tokenize(line.Trim()));
        foreach (string i in Toks)
        {
            if (Collection.String.IsEmpty(i))
                continue;

            // Create a new sublist when encountering a semicolon
            if (i == ";")
            {
                Tokens.Add(CurrentList);
                CurrentList = new List<string>();
            }

            else
                CurrentList.Add(i);
        }

        // Add the last sublist to the result list
        Tokens.Add(CurrentList);
    }

    private string Evaluate(string expr)
    {
        string Calculate = "";
        DataTable dt = new DataTable();

        try
        {
            Calculate = dt.Compute(string.Join("", expr), "").ToString() ?? ""; // Evaluate the expression.
            if (Calculate.ToString().Contains("∞"))
                Error.ZeroDivision();
        }

        catch (System.Exception e)
        {
            if (e.Message.ToLower().Contains("divide by zero."))
                Error.ZeroDivision();
        }

        return Calculate;
    }

    public string[] Parse(string[] toks)
    {
        string expr = "";
        List<string> tokens = new List<string>();

        for (int i = 0; i < toks.Length; i++)
        {
            if (isFloat(toks[i]) || toks[i] == "+" || toks[i] == "-" || toks[i] == "*" || toks[i] == "/" || toks[i] == "(" || toks[i] == ")")
            {
                expr += toks[i];
                if (i == toks.Length-1 || !(isFloat(toks[i+1]) || toks[i+1] == "+" || toks[i+1] == "-" || toks[i+1] == "*" || toks[i+1] == "/" || toks[i+1] == "(" || toks[i+1] == ")"))
                {
                    string output = Evaluate(expr);
                    if (Collection.String.IsEmpty(output))
                    {
                        if (i == toks.Length-1)
                            tokens.Add(expr);

                        else
                        {
                            i++;
                            tokens.Add(expr + toks[i]);
                        }
                    }

                    else
                        tokens.Add(output);

                    expr = "";
                }
            }

            else
                tokens.Add(toks[i]);
        }

        return tokens.ToArray();
    }

    public string[] Tokenize(string line)
    {
        string tok = "";
        List<string> tokens = new List<string>();

        for (int i = 0; i < line.Length; i++)
        {
            tok += line[i].ToString();
            if (Collection.String.IsEmpty(tok))
            {
                tokens.Add(" ");
                tok = "";
            }

            else if (tok == "#")
                break;

            else if (line[i].ToString() == ";")
            {
                tokens.Add(tok.Substring(0, tok.Length-1));
                tokens.Add(line[i].ToString());
                tok = "";
            }

            else if (tok == ">" || tok == "@" || tok == "!")
            {
                tokens.Add(tok);
                tok = "";
            }

            else if (tok == "+" || tok == "-" || tok == "*" || tok == "/")
            {
                tokens.Add(tok);
                tok = "";
            }

            else if (Collection.String.IsEmpty(tok[tok.Length-1].ToString()))
            {
                tokens.Add(tok.Trim());
                tok = "";
            }

            else if (isInt(tok))
            {
                i++;
                while (i < line.Length && isFloat(line[i].ToString()))
                {
                    tok += line[i];
                    i++;
                }

                i--;

                tokens.Add(tok);
                tok = "";
            }

            else if (tok == "\"" || tok == "'")
            {
                char str_tok = tok[0];

                i++;
                while (i < line.Length && line[i] != str_tok)
                {
                    if (line[i] == '\\')
                    {
                        i++;
                        switch (line[i])
                        {
                            case '\\':
                                tok += "\\";
                                break;

                            case '"':
                                tok += "\"";
                                break;

                            case '\'':
                                tok += "'";
                                break;

                            case 'n':
                                tok += "\n";
                                break;

                            case '0':
                                tok += "\0";
                                break;

                            case 't':
                                tok += "\t";
                                break;

                            case 'r':
                                tok += "\r";
                                break;

                            case 'b':
                                tok += "\b";
                                break;

                            case 'a':
                                tok += "\a";
                                break;

                            case 'f':
                                tok += "\f";
                                break;

                            default:
                                tok += "\\" + line[i].ToString();
                                break;
                        }
                    }

                    else
                        tok += line[i];

                    i++;
                }

                if (i < line.Length)
                    tok += line[i];

                if (i >= line.Length)
                {
                    Error.Syntax("Unterminated string literal");
                    tokens = new List<string>();
                    return tokens.ToArray();
                }

                tokens.Add(tok);
                tok = "";
            }

            else if (i == line.Length-1)
            {
                tokens.Add(tok);
                tok = "";
            }
        }

        return tokens.ToArray();
    }

    private bool isInt(string str)
    {
        if (Collection.String.IsEmpty(str))
            return false;

        foreach (char c in str)
        {
            if (!char.IsDigit(c))
                return false;
        }

        return true;
    }

    private bool isFloat(string str)
    {
        if (Collection.String.IsEmpty(str))
            return false;

        int dotCount = 0;
        foreach (char c in str)
        {
            if (char.IsDigit(c))
                continue;

            else if (c == '.' && dotCount < 2)
            {
                dotCount++;
                continue;
            }

            return false;
        }

        return true;
    }

    public static string[] SimplifyString(string[] str)
    {
        List<string> tempArgs = new List<string>();
        for (int i = 0; i < str.Length; i++)
            tempArgs.Add(Collection.String.Strings(str[i]));

        return tempArgs.ToArray();
    }
}
