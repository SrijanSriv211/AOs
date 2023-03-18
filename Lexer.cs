using System.Data;
using System.Text;
using System.Linq;
using System.Collections.Generic;

class Lexer
{
    public string[] Tokens;
    public Lexer(string line)
    {
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

            else if (tok == ">" || tok == "@" || tok == "!")
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

    public static string SimplifyString(string str)
    {
        if (Collection.String.IsString(str))
            return Obsidian.Shell.Strings(str);

        else
            return str;
    }
}

class argparse
{
    public string[] free_args = new string[0];
    private string program_name = string.Empty;
    private List<Argument> arguments = new List<Argument>();

    public argparse(string prog)
    {
        program_name = prog;
    }
    
    public void AddArgument(string name, string help = "", bool required = false, string default_value = null)
    {
        arguments.Add(new Argument(name, help, required, default_value));
    }

    // Method to parse the command line arguments and return a dictionary of parsed arguments
    public Dictionary<string, string> Parse(string[] args)
    {
        var parsed_args = new Dictionary<string, string>();

        string arg_name = string.Empty;
        string arg_val = string.Empty;

        for (int i = 0; i < args.Length; i++)
        {
            var arg = args[i];
            if (arguments.Find(a => a.Name == arg) != null)
            {
                arg_name = arg;
                if (arguments.Find(a => a.Name == arg && a.IsFlag) != null)
                    arg_val = "true";

                else
                {
                    List<string> list = args.ToList();
                    int index = list.IndexOf(arg);
                    if (index != -1 && index < args.Length-1)
                        arg_val = args[index + 1];

                    else
                        new Error($"Missing value for argument: {arg}");
                }
            }

            else
            {
                List<string> list = free_args.ToList();
                list.Add(arg);
                free_args = list.ToArray();
            }

            // Store the parsed argument to the dict.
            if (arguments.Find(a => a.Name == arg_name) != null)
                parsed_args[arg_name] = arg_val;
        }

        foreach (var argument in arguments)
        {
            if (argument.Required && !parsed_args.ContainsKey(argument.Name))
                new Error($"Missing required argument: {argument.Name}");

            if (!parsed_args.ContainsKey(argument.Name))
                parsed_args[argument.Name] = argument.DefaultValue;
        }

        return parsed_args;
    }

    public void PrintHelp()
    {
        System.Console.WriteLine($"Usage: {program_name} [OPTIONS]");
        System.Console.WriteLine();
        System.Console.WriteLine("Options:");
        foreach (var argument in arguments)
        {
            var argName = $"{argument.Name}";
            var defaultValue = argument.DefaultValue != null ? $" (default: {argument.DefaultValue})" : "";
            System.Console.WriteLine($"{argName}: {argument.Help}{defaultValue}");
        }
    }

    private class Argument
    {
        public string Name { get; set; }
        public string Help { get; set; }
        public bool Required { get; set; }
        public string DefaultValue { get; set; }
        public bool IsFlag { get; set; }

        public Argument(string name, string help, bool required, string default_value)
        {
            Name = name;
            Help = help;
            Required = required;
            DefaultValue = default_value;
            IsFlag = (default_value == null);
        }
    }
}
