using System.Data;
using System.Text;
using System.Collections.Generic;

class Lexer
{
    public string[] Tokens;
    public Lexer(string line)
    {
        if (!BracketsNQuotes(line.Trim()))
            Tokens = Parse(Tokenizer(line.Trim()));
    }

    private string Evaluate(string expr)
    {
        string Calculate = "";
        DataTable dt = new DataTable();

        try
        {
            Calculate = dt.Compute(string.Join("", expr), "").ToString() ?? ""; // Evaluate the expression.
            if (Calculate.ToString().Contains("∞"))
                Error.ZeroDivision(expr);
        }

        catch (System.Exception e)
        {
            if (e.Message.ToLower().Contains("divide by zero."))
                Error.ZeroDivision(expr);
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

    public string[] Tokenizer(string line)
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

            else if (Collection.String.IsEmpty(tok[tok.Length-1].ToString()))
            {
                tokens.Add(tok.Trim());
                tok = "";
            }

            else if (tok == "#")
                break;

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

                tok += line[i];
                if (i >= line.Length)
                    Error.Syntax("Unterminated string literal");

                tokens.Add(tok);
                tok = "";
            }

            else if (tok == "+" || tok == "-" || tok == "*" || tok == "/")
            {
                tokens.Add(tok);
                tok = "";
            }

            else if (tok == ">" || tok == "@")
            {
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

    private string ExcludeDataInString(string line)
    {
        char quote = '\0';
        bool escape = false;
        StringBuilder nonStringData = new StringBuilder();

        foreach (char c in line)
        {
            if (c == '"' || c == '\'')
            {
                if (escape)
                {
                    nonStringData.Append(c);
                    escape = false;
                }

                else if (quote == '\0')
                {
                    quote = c;
                    nonStringData.Append(c);
                }

                else if (quote == c)
                {
                    quote = '\0';
                    nonStringData.Append(c);
                }
            }

            else if (quote == '\0')
            {
                nonStringData.Append(c);
            }

            if (c == '\\' && quote != '\0')
            {
                escape = true;
            }

            else
            {
                escape = false;
            }
        }

        return nonStringData.ToString();
    }

    private bool BracketsNQuotes(string cmd)
    {
        string NonStringData = ExcludeDataInString(cmd);

        // Check for unmatched parentheses
        if (HasUnevenBrackets(NonStringData, "(", ")"))
        {
            Error.Syntax("Unmatched parenthesis");
            return true;
        }

        // Check for unmatched square brackets
        if (HasUnevenBrackets(NonStringData, "[", "]"))
        {
            Error.Syntax("Unmatched square brackets");
            return true;
        }

        return false;
    }

    private bool HasUnevenQuotes(string input, string quote)
    {
        return Collection.String.Count(input, quote) % 2 != 0;
    }

    private bool HasUnevenBrackets(string input, string open, string close)
    {
        int openCount = Collection.String.Count(input, open);
        int closeCount = Collection.String.Count(input, close);
        return openCount != closeCount;
    }

    public static string[] SimplifyString(string[] str)
    {
        List<string> tempArgs = new List<string>();
        for (int i = 0; i < str.Length; i++)
        {
            if (Collection.String.IsString(str[i]))
                tempArgs.Add(Obsidian.Shell.Strings(str[i]));

            else
                tempArgs.Add(str[i]);
        }

        return tempArgs.ToArray();
    }
}
