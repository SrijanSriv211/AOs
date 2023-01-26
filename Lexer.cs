using System.Data;
using System.Linq;
using System.Collections.Generic;

class Lexer
{
        private string Command;
        public string[] Tokens = {};
        public Lexer(string _Line)
        {
            Command = _Line.Trim();
            if (!BracketsNQuotes()) Tokenizer();
        }

        // Modify the public Tokens variable.
        private void Tokenizer()
        {
            string Lexemes = "";
            string Operators = "+-*/().";
            string Expression = "";
            string NonStringStr = ExcludeDataInString(Command).Trim();
            string NoSpaceLexeme = "";

            int StrIdx = 0;
            bool IsString = false;
            string[] String = DataInString(Command);
            string[] Exprs = MakeNumber(Command);

            List<string> Toks = new List<string>();
            for (int i = 0; i < NonStringStr.Length; i++)
            {
                Lexemes += NonStringStr[i].ToString();
                if (Lexemes.EndsWith("#"))
                {
                    Lexemes = "";
                    Expression = "";
                    break;
                }

                else if (Lexemes.EndsWith(" "))
                {
                    NoSpaceLexeme = Lexemes.Substring(0, Lexemes.Length - 1);
                    if (Collection.String.IsEmpty(Expression))
                    {
                        if (!Collection.String.IsEmpty(NoSpaceLexeme)) Toks.Add(NoSpaceLexeme);
                        Toks.Add(NonStringStr[i].ToString());
                        Lexemes = "";
                        Expression = "";
                    }

                    else Expression += " ";
                }

                else if (Lexemes.EndsWith("\"") || Lexemes.EndsWith("'"))
                {
                    if (IsString)
                    {
                        Toks.Add(String[StrIdx]);
                        IsString = false;
                        StrIdx++;
                    }

                    else IsString = true;

                    NoSpaceLexeme = Lexemes.Substring(0, Lexemes.Length - 1);
                    if (!Collection.String.IsEmpty(NoSpaceLexeme)) Toks.Add(NoSpaceLexeme);
                    Lexemes = "";
                    Expression = "";
                }

                else if (Operators.Contains(Lexemes.Last()) || int.TryParse(Lexemes.Last().ToString(), out _))
                {
                    Expression += NonStringStr[i].ToString();
                    if (Exprs.Any(Expression.Contains))
                    {
                        string CalculatedAns = Evaluator(Expression);
                        if (!Collection.String.IsEmpty(CalculatedAns.Trim()))
                        {
                            Lexemes = Lexemes.Replace(Expression, CalculatedAns);
                            Expression = "";
                        }

                        else Expression = "";
                    }
                }

                else if (Lexemes.EndsWith(">") || Lexemes.EndsWith("@"))
                {
                    NoSpaceLexeme = Lexemes.Substring(0, Lexemes.Length - 1);
                    if (!Collection.String.IsEmpty(NoSpaceLexeme)) Toks.Add(NoSpaceLexeme);
                    Toks.Add(NonStringStr[i].ToString());
                    Lexemes = "";
                    Expression = "";
                }

                else Expression = "";
            }

            if (!Collection.String.IsEmpty(Lexemes))
            {
                Toks.Add(Lexemes);
                Lexemes = "";
                Expression = "";
            }

            Tokens = Toks.ToArray();
        }

        private string Evaluator(string _Expr)
        {
            string Calculate = "";
            DataTable dt = new DataTable();

            // Try evaluating the expression.
            try
            {
                Calculate = dt.Compute(_Expr, "").ToString() ?? ""; // Evaluate the expression.

                // Division by 0 is not a problem but still throw this error message.
                if (Calculate.ToString().Contains("∞")) Error.ZeroDivision(Command);
            }

            // Throw an error message.
            catch (System.Exception e)
            {
                string ErrorMsg = e.Message.ToString().Substring(13).Trim();
                if (ErrorMsg == "divide by zero.") Error.ZeroDivision(Command);
            }

            return Calculate;
        }

        // Check for any irregularities in line.
        private bool BracketsNQuotes()
        {
            string NonStringData = ExcludeDataInString(Command);

            // Double and Single quotes
            int SQUOTE = Collection.String.Count(NonStringData, "'");
            int DQUOTE = Collection.String.Count(NonStringData, "\"");

            // Parenthesis
            int LPAREN = Collection.String.Count(NonStringData, "(");
            int RPAREN = Collection.String.Count(NonStringData, ")");

            // Square brackets
            int LSQUARE = Collection.String.Count(NonStringData, "[");
            int RSQUARE = Collection.String.Count(NonStringData, "]");
            
            if ((SQUOTE != 0 && !IsEven(SQUOTE)) || (DQUOTE != 0 && !IsEven(DQUOTE)))
            {
                Error.Syntax("Unterminated string literal");
                return true;
            }

            else if (LPAREN != 0 || RPAREN != 0)
            {
                if (LPAREN > RPAREN)
                {
                    Error.Syntax("'(' was never closed");
                    return true;
                }

                else if (LPAREN < RPAREN)
                {
                    Error.Syntax("Unmatched ')'");
                    return true;
                }
            }

            else if (LSQUARE != 0 || RSQUARE != 0)
            {
                if (LSQUARE > RSQUARE)
                {
                    Error.Syntax("'[' was never closed");
                    return true;
                }

                else if (LSQUARE < RSQUARE)
                {
                    Error.Syntax("Unmatched ']'");
                    return true;
                }
            }

            return false;
        }

        // Extract all the number and symbols in an Expression string.
        private string[] MakeNumber(string _Line)
        {
            string Lexemes = "";
            string Operators = "+-*/().";
            string Expression = "";
            string NonStringStr = ExcludeDataInString(Command);

            List<string> ExprToks = new List<string>();
            for (int i = 0; i < NonStringStr.Length; i++)
            {
                Lexemes += NonStringStr[i].ToString();
                if (Lexemes.EndsWith(" "))
                {
                    Lexemes = "";
                    Expression += " ";
                }

                else if (Operators.Contains(Lexemes.Last()) || int.TryParse(Lexemes.Last().ToString(), out _))
                    Expression += NonStringStr[i].ToString();

                else
                {
                    if (!Collection.String.IsEmpty(Expression.Trim()) && Expression.Trim().Any(char.IsDigit))
                        ExprToks.Add(Expression.Trim());

                    Lexemes = "";
                    Expression = "";
                }

                if (i == NonStringStr.Length - 1)
                {
                    if (!Collection.String.IsEmpty(Expression.Trim()) && Expression.Trim().Any(char.IsDigit))
                        ExprToks.Add(Expression.Trim());

                    Lexemes = "";
                    Expression = "";
                }
            }

            return ExprToks.ToArray();
        }

        // Extract all in-lang string from a C# string.
        private string[] DataInString(string _Line)
        {
            char StrQuote = '\0';
            bool EscapeChar = false;
            string InStringData = "";
            List<string> Strings = new List<string>();

            // Loop over every char and store those which exist in In-Glass String.
            for (int i = 0; i < _Line.Length; i++)
            {
                if (EscapeChar)
                {
                    if (_Line[i] == '\\') InStringData += "\\";

                    // Punctuation characters.
                    else if (_Line[i] == '\'') InStringData += "'";
                    else if (_Line[i] == '"') InStringData += "\"";

                    // Control characters.
                    else if (_Line[i] == 'n') InStringData += "\n";
                    else if (_Line[i] == 't') InStringData += "\t";
                    else if (_Line[i] == 'b') InStringData += "\b";
                    else if (_Line[i] == 'r') InStringData += "\r";
                    else if (_Line[i] == 'f') InStringData += "\f";
                    else if (_Line[i] == 'v') InStringData += "\v";
                    else InStringData += _Line[i];

                    EscapeChar = false;
                }

                else
                {
                    if (_Line[i] == '"' && !EscapeChar)
                    {
                        InStringData += _Line[i];
                        if (StrQuote == '\0') StrQuote = '"';
                        else if (StrQuote == '"')
                        {
                            StrQuote = '\0';
                            Strings.Add(InStringData);
                            InStringData = "";
                        }
                    }

                    else if (_Line[i] == '\'' && !EscapeChar)
                    {
                        InStringData += _Line[i];
                        if (StrQuote == '\0') StrQuote = '\'';
                        else if (StrQuote == '\'')
                        {
                            StrQuote = '\0';
                            Strings.Add(InStringData);
                            InStringData = "";
                        }
                    }

                    else if (_Line[i] == '\\' && StrQuote != '\0') EscapeChar = true;
                    else if (StrQuote != '\0') InStringData += _Line[i];
                }
            }

            return Strings.ToArray();
        }

        // Exclude all data in a string.
        private string ExcludeDataInString(string _Line)
        {
            char StrQuote = '\0';
            bool EscapeChar = false;
            string NonStringData = "";

            // Loop over every char and store those which does not exist in In-Glass String.
            for (int i = 0; i < _Line.Length; i++)
            {
                if (_Line[i] == '"' && !EscapeChar)
                {
                    if (StrQuote == '\0')
                    {
                        StrQuote = '"';
                        NonStringData += _Line[i];
                    }

                    else if (StrQuote == '"')
                    {
                        StrQuote = '\0';
                        NonStringData += _Line[i];
                    }
                }

                else if (_Line[i] == '\'' && !EscapeChar)
                {
                    if (StrQuote == '\0')
                    {
                        StrQuote = '\'';
                        NonStringData += _Line[i];
                    }

                    else if (StrQuote == '\'')
                    {
                        StrQuote = '\0';
                        NonStringData += _Line[i];
                    }
                }

                else if (StrQuote == '\0') NonStringData += _Line[i];

                if (EscapeChar) EscapeChar = false;
                else if (_Line[i] == '\\' && StrQuote != '\0') EscapeChar = true;
            }

            return NonStringData;
        }

        // Check whether the given number is Odd or Even.
        private bool IsEven(int _Number)
        {
            return (_Number % 2 == 0);
        }
}
