namespace Lexer
{
    partial class Tokenizer
    {
        public enum TokenType
        {
            EOL = 0,
            IDENTIFIER,
            STRING,
            NUMBER,
            OPERATOR,
            SYMBOL,
            CHAIN
        }

        public struct Token(string Name, TokenType Type)
        {
            public string Name = Name;
            public TokenType Type = Type;
        }

        private bool IsExpr(string str)
        {
            string expression_pattern = @"^[-+*/0-9().]+$";
            string operator_pattern = @"^[-+*/]+$";
            str = str.Replace(" ", "");

            return System.Text.RegularExpressions.Regex.IsMatch(str, expression_pattern) && !System.Text.RegularExpressions.Regex.IsMatch(str, operator_pattern);
        }

        private bool IsOperator(char ch)
        {
            return ch == '+' || ch == '-' || ch == '*' || ch == '/' || ch == '%';
        }

        private bool IsChainableSymbol(char ch)
        {
            return ch == '(' || ch == ')' || ch == '[' || ch == ']' || ch == '{' || ch == '}';
        }

        private bool IsSymbol(char ch)
        {
            return ch == '>' || ch == '@' || ch == '!';
        }

        private static bool IsValidCharForFilenameInWindows(char ch)
        {
            return ch == '`' || ch == '~' || ch == '!' || ch == '@' || ch == '$' ||
                ch == '^' || ch == '&' || ch == '?' || ch == '(' || ch == ')' ||
                ch == '=' || ch == '+' || ch == '-' || ch == '*' || ch == '/' ||
                ch == '%' || ch == '_' || ch == '.' || ch == '[' || ch == ']' ||
                ch == ',' || ch == '{' || ch == '}' || ch == '|' || ch == ':' ||
                ch == '<' || ch == '>' || ch == '\\';
        }

        private bool IsIdentifier(char ch)
        {
            // Check if a char is empty or not, if not then check if the char is a letter or a digit or a symbol.
            // If no in any of these cases then return false, otherwise true.
            if (Utils.String.IsEmpty(ch.ToString()) || !(char.IsLetterOrDigit(ch) || IsValidCharForFilenameInWindows(ch)))
                return false;

            return true;
        }

        private bool IsNumber(char ch)
        {
            // Check if the given char contains any properties of a number or not.
            if (Utils.String.IsEmpty(ch.ToString()) || !(char.IsDigit(ch) || ch == '.'))
                return false;

            return true;
        }
    }
}
