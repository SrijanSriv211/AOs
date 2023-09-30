partial class Lexer
{
    private bool Is_expr(string str)
    {
        string expression_pattern = @"^[-+*/0-9().]+$";
        string operator_pattern = @"^[-+*/]+$";
        str = str.Replace(" ", "");

        return System.Text.RegularExpressions.Regex.IsMatch(str, expression_pattern) && !System.Text.RegularExpressions.Regex.IsMatch(str, operator_pattern);
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
        return c == '`' || c == '~' || c == '!' || c == '@' || c == '$' ||
               c == '^' || c == '&' || c == '?' || c == '(' || c == ')' ||
               c == '=' || c == '+' || c == '-' || c == '*' || c == '/' ||
               c == '%' || c == '_' || c == '.' || c == '[' || c == ']' ||
               c == ',' || c == '{' || c == '}' || c == '|' || c == ':' ||
               c == '<' || c == '>' || c == '\\';
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
