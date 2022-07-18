using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AOs
{
    class Error
    {
        // No arguments.
        static public void NoArgs()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"NoArgumentError: No arguments were passed");
            Console.ResetColor();
        }

        // Unrecognized arguments.
        static public void Args(string[] _Flag)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Unrecognized arguments: {string.Join(", ", Collections.Array.Reduce(_Flag))}");
            Console.ResetColor();
        }

        static public void Args(string _Flag)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Unrecognized arguments: {_Flag}");
            Console.ResetColor();
        }

        // Not appropriate number of arguments.
        static public void TooFewArgs(string[] _Flag)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Too few arguments: {string.Join(", ", Collections.Array.Reduce(_Flag))}");
            Console.ResetColor();
        }

        static public void TooFewArgs(string _Flag)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Too few arguments: {_Flag}");
            Console.ResetColor();
        }

        static public void TooManyArgs(string[] _Flag)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Too many arguments: {string.Join(", ", Collections.Array.Reduce(_Flag))}");
            Console.ResetColor();
        }

        static public void TooManyArgs(string _Flag)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Too many arguments: {_Flag}");
            Console.ResetColor();
        }

        // Unrecognized Command.
        static public void Command(string _Command, string _Details="Command does not exist")
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"'{_Command}', {_Details}");
            Console.ResetColor();
        }

        // Unrecognized syntax.
        static public void Syntax(string _Details)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"SyntaxError: {_Details}");
            Console.ResetColor();
        }

        // Division by 0.
        static public void ZeroDivision(string _Line)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("ZeroDivisionError: Division by 0");
            Console.ResetColor();
        }
    }

    class Collections
    {
        public class String
        {
            // Count the number of times a substring appears in a string.
            static public int Count(string _Source, string _SubString)
            {
                return Regex.Matches(_Source, Regex.Escape(_SubString)).Count;
            }

            // Remove extra spaces from a string.
            static public string Reduce(string _Line)
            {
                return string.Join(" ", _Line.Split(new char[0], StringSplitOptions.RemoveEmptyEntries).ToList().Select(x => x.Trim()));
            }

            // Check whether a string is empty.
            static public bool IsEmpty(string _Line)
            {
                if (string.IsNullOrEmpty(_Line) || string.IsNullOrWhiteSpace(_Line)) return true;
                else return false;
            }

            // Check if a string has special chars like @, !, >.
            static public bool HasSpecialChars(string _Line)
            {
                return _Line.Any(c => !Char.IsLetterOrDigit(c));
            }

            // Check if the string is wrapped with string quotes or not.
            static public bool IsString(string _Line)
            {
                if (_Line.StartsWith("\"") && _Line.EndsWith("\"")) return true;
                else if (_Line.StartsWith("'") && _Line.EndsWith("'")) return true;
                return false;
            }
        }

        public class Array
        {
            // Remove empty strings from an array.
            static public string[] Reduce(string[] arr)
            {
                return arr.Where(x => !String.IsEmpty(x)).ToArray();
            }

            // Remove duplicate elements from an array.
            static public string[] Filter(string[] arr)
            {
                return arr.Distinct().ToArray();
            }

            // Trim the leading part of an array.
            static public string[] TrimStart(string[] arr)
            {
                int Count = 0;
                foreach (string i in arr)
                {
                    if (String.IsEmpty(i)) Count++;
                    else break;
                }

                return arr.Skip(Count).ToArray();
            }

            // Trim the trailing part of an array.
            static public string[] TrimEnd(string[] arr)
            {
                int Count = 0;
                foreach (string i in arr.ToArray().Reverse())
                {
                    if (String.IsEmpty(i)) Count++;
                    else break;
                }

                return arr.ToArray().Reverse().Skip(Count).Reverse().ToArray();
            }

            // Trim the leading and trailing part of an array.
            static public string[] Trim(string[] arr)
            {
                // No need to search through nothing
                if (arr == null || arr.Length == 0) return System.Array.Empty<string>();

                // Define predicate to test for non-empty strings
                Predicate<string> IsNotEmpty = s => !String.IsEmpty(s);

                var FirstIndex = System.Array.FindIndex(arr, IsNotEmpty);

                // Nothing to return if it's all whitespace anyway
                if (FirstIndex < 0) return System.Array.Empty<string>();

                var LastIndex = System.Array.FindLastIndex(arr, IsNotEmpty);

                // Calculate size of the relevant middle-section from the indices
                var NewArraySize = LastIndex - FirstIndex + 1;

                // Create new array and copy items to it
                var Results = new string[NewArraySize];
                System.Array.Copy(arr, FirstIndex, Results, 0, NewArraySize);

                return Results;
            }
        }
    }

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
                    if (Collections.String.IsEmpty(Expression))
                    {
                        if (!Collections.String.IsEmpty(NoSpaceLexeme)) Toks.Add(NoSpaceLexeme);
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
                    if (!Collections.String.IsEmpty(NoSpaceLexeme)) Toks.Add(NoSpaceLexeme);
                    Lexemes = "";
                    Expression = "";
                }

                else if (Operators.Contains(Lexemes.Last()) || int.TryParse(Lexemes.Last().ToString(), out _))
                {
                    Expression += NonStringStr[i].ToString();
                    if (Exprs.Any(Expression.Contains))
                    {
                        string CalculatedAns = Evaluator(Expression);
                        if (!Collections.String.IsEmpty(CalculatedAns.Trim()))
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
                    if (!Collections.String.IsEmpty(NoSpaceLexeme)) Toks.Add(NoSpaceLexeme);
                    Toks.Add(NonStringStr[i].ToString());
                    Lexemes = "";
                    Expression = "";
                }

                else Expression = "";
            }

            if (!Collections.String.IsEmpty(Lexemes))
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
                Calculate = dt.Compute(_Expr, "").ToString(); // Evaluate the expression.

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
            int SQUOTE = Collections.String.Count(NonStringData, "'");
            int DQUOTE = Collections.String.Count(NonStringData, "\"");

            // Parenthesis
            int LPAREN = Collections.String.Count(NonStringData, "(");
            int RPAREN = Collections.String.Count(NonStringData, ")");

            // Square brackets
            int LSQUARE = Collections.String.Count(NonStringData, "[");
            int RSQUARE = Collections.String.Count(NonStringData, "]");
            
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
                    if (!Collections.String.IsEmpty(Expression.Trim()) && Expression.Trim().Any(char.IsDigit))
                        ExprToks.Add(Expression.Trim());

                    Lexemes = "";
                    Expression = "";
                }

                if (i == NonStringStr.Length - 1)
                {
                    if (!Collections.String.IsEmpty(Expression.Trim()) && Expression.Trim().Any(char.IsDigit))
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

    class Terminal
    {
        private string Command;
        public string Title;
        public string Prompt;
        public string Version = "AOs 2022 [Version 2.0]";
        public string VersionNum = "2.0";
        public Terminal(string _Title="", string _Prompt="")
        {
            Title = _Title;
            Prompt = _Prompt;
        }

        public string TakeInput(string _Command="")
        {
            Command = _Command.Trim();
            if (Collections.String.IsEmpty(Command))
            {
                Console.Write(Prompt);
                Command = Console.ReadLine().Trim();
            }

            // Set history.
            if (!Collections.String.IsEmpty(Command)) History.SetHistory(Command);

            // Parse input.
            if (!Collections.String.IsEmpty(Command) && Command[0] == '_') Command = Command.Substring(1).Trim();
            return Command.Trim();
        }

        public void Login(bool ChangeTitle=true, bool ClearConsole=true)
        {
            if (!Collections.String.IsEmpty(Title) && !Collections.String.IsEmpty(Prompt))
            {
                if (ChangeTitle) Console.Title = Title;
                if (ClearConsole) Console.Clear();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(Version);
                Console.ResetColor();
            }
        }

        public string ClearConsole()
        {
            Login(false);
            return "";
        }

        public void Credits()
        {
            string[] CreditCenter = {
                "_________ Team AOS ________",
                "Author     -> Srijan Srivastava",
                "Found on   -> 15 June 2020",
                "Github     -> github.com/Light-Lens/AOs",
                "Contact    -> QCoreNest@gmail.com",
                "",
                "____________________ Note (For Developers) ____________________",
                "|| AOs is a Command line utility designed to improve your Efficiency and Productivity.",
                "|| All code is licensed under an MIT license.",
                "|| This allows you to re-use the code freely, remixed in both commercial and non-commercial projects.",
                "|| The only requirement is to include the same license when distributing.",
                "|| If you built an Operating System on top of AOs Kernal,",
                "|| Then you must include 'Based on AOs Kernel 2.0' in your program.",
                "",
                "____________________ Note (For All) ____________________",
                "|| Warning - Do not Delete any File",
                "|| or it may Cause System Corruption",
                "|| and may lead to system instability.",
                "",
                "Type 'help -h' to get more information about help command."
            };

            Console.WriteLine(string.Join("\n", CreditCenter));
        }

        public void GetHelp(string _HelpFlag="-h")
        {
            string[] AOsHelpCenter = {
                "about           -> About AOs.",
                "shutdown        -> Shutdown AOs.",
                "restart         -> Restart AOs.",
                "admin           -> Administrator.",
                "clear           -> Clears the screen.",
                "history         -> Displays the history of Commands.",
                "version         -> Displays the AOs version.",
                "console         -> Starts a new instance of the terminal.",
                "title           -> Changes the title for AOs window.",
                "color           -> Changes the default AOs foreground and background colors.",
                "time            -> Displays current time and date.",
                "shout           -> Displays messages.",
                "get             -> Asks for input.",
                "getkey          -> Asks for key input.",
                "pause           -> Suspends processing of a command and displays the message.",
                "wait            -> Suspends processing of a command for the given number of seconds.",
                "run             -> Starts a specified program or command.",
                "ls              -> Displays a list of files and subdirectories in a directory.",
                "cd              -> Changes the current directory.",
                "touch           -> Creates a file or folder.",
                "del             -> Deletes one or more files or folders.",
                "ren             -> Renames one or more files from one directory to another directory.",
                "copy            -> Copies one or more files from one directory to another directory.",
                "move            -> Moves one or more files from one directory to another directory.",
                "alarm           -> Sets an alarm.",
                "lock            -> Locks the system temporarily.",
                "pixelate        -> Starts a website in a web browser.",
                "electron        -> Starts a text editor.",
                "pixstore        -> Manages all installed apps via command line.",
                "prompt          -> Specifies a new command prompt.",
                "apps            -> Lists all installed apps on your machine.",
                "calculator      -> Starts the calculator.",
                "music           -> Starts the music apps.",
                "paint           -> Starts microsoft paint.",
                "paint3d         -> Starts microsoft paint 3d.",
                "onenote         -> Starts onenote for windows 10.",
                "settings        -> Starts windows settings.",
                "screensnip      -> Starts snipping tool.",
                "snipandsketch   -> Starts snip & sketch.",
                "minecraft       -> Starts minecraft.",
                "candycrush      -> Starts candy crush soda saga.",
                "onedrive        -> Starts onedrive.",
                "commit          -> Edits a text file via command prompt.",
                "read            -> Reads a text file via command prompt."
            };

            string[] AdminHelpCenter = {
                "update          -> Check for Updates.",
                "ran             -> Displays operating system configuration information.",
                "tree            -> Graphically displays the directory structure of a drive or path.",
                "diagxt          -> Displays machine specific properties and configuration.",
                "exit            -> Exit Administrator.",
                "clear           -> Clears the screen.",
                "generate        -> Generates a random number between 0 and 1.",
                "terminate       -> Terminates current running process.",
                "reset           -> Reset this PC.",
                "lock            -> Locks the System at Start-up.",
                "scan            -> Scans the integrity of all protected system files.",
                "restore         -> Restores system files and folders."
            };

            string[] SafeModeHelpCenter = {
                "cd              -> Changes the current directory.",
                "update          -> Check for Updates.",
                "diagxt          -> Displays machine specific properties and configuration.",
                "exit            -> Exit SAFE MODE.",
                "clear           -> Clears the screen.",
                "scan            -> Scans the integrity of all protected system files.",
                "restore         -> Restores system files and folders.",
                "reset           -> Reset this PC.",
                "about           -> About AOs.",
                "shutdown        -> Shutdown AOs.",
                "restart         -> Restart AOs.",
                "credits         -> Credits the Developers.",
                "version         -> Displays the AOs version.",
                "console         -> Starts a new instance of the terminal.",
                "time            -> Displays current time and date.",
                "pause           -> Suspends processing of a command and displays the message.",
                "read            -> Reads a text file via command prompt.",
                "run             -> Starts a specified program or command.",
                "ls              -> Displays a list of files and subdirectories in a directory.",
                "history         -> Displays the history of Commands."
            };

            string[] HiddenFeaturesHelpCenter = {
                "#               -> Ignores the following text.",
                ">               -> Starts a specified program or command.",
                "@               -> Displays overload commands.",
                "!               -> Starts oxygen."
            };

            if (_HelpFlag == "-h" || _HelpFlag == "--help" || _HelpFlag == "/?")
            {
                string[] SYSHelpCenter = {
                    "AOs is a Command line utility designed to improve your Efficiency and Productivity.",
                    "Usage: help [Option]",
                    "",
                    "Options:",
                    "-h, --help       -> Displays all supported arguments for help command.",
                    "-g, --global     -> Displays all supported commands.",
                    "-a, --aos        -> Displays only AOs commands.",
                    "-d, --admin      -> Displays only Administrator commands.",
                    "-s, --safemode   -> Displays only SafeMode commands."
                };

                Console.WriteLine(string.Join("\n", SYSHelpCenter));
            }

            else if (_HelpFlag == "-g" || _HelpFlag == "--global")
            {
                Console.WriteLine("Type 'help -h' to get more information about help command.");
                List<string> Global = new List<string>();

                Global.AddRange(AOsHelpCenter);
                Global.AddRange(AdminHelpCenter);
                Global.AddRange(HiddenFeaturesHelpCenter);

                string[] AllCommands = Global.Distinct().ToArray();
                Array.Sort(AllCommands);
                Console.WriteLine(string.Join("\n", AllCommands));
            }

            else if (_HelpFlag == "-a" || _HelpFlag == "--aos")
            {
                Console.WriteLine("Type 'help -h' to get more information about help command.");
                Array.Sort(AOsHelpCenter);
                Console.WriteLine(string.Join("\n", AOsHelpCenter));
            }

            else if (_HelpFlag == "-d" || _HelpFlag == "--admin")
            {
                Console.WriteLine("Type 'help -h' to get more information about help command.");
                Array.Sort(AdminHelpCenter);
                Console.WriteLine(string.Join("\n", AdminHelpCenter));
            }

            else if (_HelpFlag == "-s" || _HelpFlag == "--safe")
            {
                Console.WriteLine("Type 'help -h' to get more information about help command.");
                Array.Sort(SafeModeHelpCenter);
                Console.WriteLine(string.Join("\n", SafeModeHelpCenter));
            }

            else
            {
                Error.Args(_HelpFlag);
                Console.WriteLine("Type 'help -h' to get more information about help command.");
            }
        }

        public class History
        {
            static public void SetHistory(string _Command)
            {
                File.AppendAllText($"{Shell.Root()}\\Files.x72\\Temp\\HISTORY", $"{DateTime.Now.ToString("[dd-MM-yyyy HH:mm:ss]")}, '{_Command}'\n");
            }

            static public void GetHistory()
            {
                Console.WriteLine(File.ReadAllText($"{Shell.Root()}\\Files.x72\\Temp\\HISTORY"));
            }

            static public void ClearHistory()
            {
                File.Delete($"{Shell.Root()}\\Files.x72\\Temp\\HISTORY");
            }
        }
    }

    class Shell
    {
        static public void RootPackages()
        {
            Directory.CreateDirectory($"{Shell.Root()}\\Files.x72\\etc");
            Directory.CreateDirectory($"{Shell.Root()}\\Files.x72\\StartUp");
            Directory.CreateDirectory($"{Shell.Root()}\\Files.x72\\Temp\\log");
            Directory.CreateDirectory($"{Shell.Root()}\\Files.x72\\Temp\\tmp");
            Directory.CreateDirectory($"{Shell.Root()}\\Files.x72\\Packages\\appdata");

            if (!File.Exists($"{Shell.Root()}\\Files.x72\\Temp\\HISTORY")) File.Create($"{Shell.Root()}\\Files.x72\\Temp\\HISTORY").Dispose();
            if (!File.Exists($"{Shell.Root()}\\Files.x72\\Temp\\log\\BOOT.log")) File.Create($"{Shell.Root()}\\Files.x72\\Temp\\log\\BOOT.log").Dispose();
            if (!File.Exists($"{Shell.Root()}\\Files.x72\\Temp\\log\\Crashreport.log")) File.Create($"{Shell.Root()}\\Files.x72\\Temp\\log\\Crashreport.log").Dispose();
        }

        static public string LocateEXE(string Filename)
        {
            string[] Path = Environment.GetEnvironmentVariable("path").Split(';');
            foreach (string Folder in Path)
            {
                if (File.Exists($"{Folder}{Filename}")) return $"{Folder}{Filename}";
                else if (File.Exists($"{Folder}\\{Filename}"))  return $"{Folder}\\{Filename}";
            }

            return string.Empty;
        }

        static public void CommandPrompt(string GetProcess="")
        {
            ProcessStartInfo StartProcess = new ProcessStartInfo("cmd.exe", $"/c {GetProcess}");
            var Execute = Process.Start(StartProcess);
            Execute.WaitForExit();
        }

        static public void StartApp(string AppName, string AppArgumens="", bool AppAdmin=false)
        {
            Process AppProcess = new Process();
            AppProcess.StartInfo.UseShellExecute = true;
            AppProcess.StartInfo.FileName = AppName;
            AppProcess.StartInfo.Arguments = AppArgumens;
            AppProcess.StartInfo.CreateNoWindow = false;
            if (AppAdmin) AppProcess.StartInfo.Verb = "runas";
            try
            {
                AppProcess.Start();
            }

            catch (Exception)
            {
                Console.WriteLine("Cannot open the app");
            }
        }

        static public string Strings(string Line)
        {
            if (Line.StartsWith("\"") && Line.EndsWith("\"")) return Line.Substring(1, Line.Length - 2);
            else if (Line.StartsWith("'") && Line.EndsWith("'")) return Line.Substring(1, Line.Length - 2);
            return Line;
        }

        static public void SYSRestore()
        {
            Console.WriteLine("Restoring.");
            Console.Write("Using 'Sysfail\\RECOVERY' to restore.");
            if (File.Exists($"{Shell.Root()}\\RESTORE.exe"))
            {
                CommandPrompt($"call \"{Shell.Root()}\\RESTORE.exe\"");
                Console.WriteLine("AOs restore successful.");
            }

            else
            {
                string NoteCurrentTime = DateTime.Now.ToString("[dd-MM-yyyy], [HH:mm:ss]");
                File.AppendAllText($"{Shell.Root()}\\Files.x72\\Temp\\log\\Crashreport.log", $"{NoteCurrentTime}, RECOVERY DIRECTORY is missing or corrupted.\n");

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nCannot restore this PC.");
                Console.WriteLine("RECOVERY DIRECTORY is missing or corrupted.");
                Console.ResetColor();

                Environment.Exit(0);
            }
        }

        static public void CheckForUpdates(string CurrentVersion)
        {
            if (File.Exists($"{Shell.Root()}\\SoftwareDistribution\\UpdatePackages\\UPR\\UPR.exe"))
                CommandPrompt($"call \"{Shell.Root()}\\SoftwareDistribution\\UpdatePackages\\UPR\\UPR.exe\" -v \"{CurrentVersion}\"");

            else
            {
                string NoteCurrentTime = DateTime.Now.ToString("[dd-MM-yyyy], [HH:mm:ss]");
                File.AppendAllText($"{Shell.Root()}\\Files.x72\\Temp\\log\\Crashreport.log", $"{NoteCurrentTime}, UPDATE DIRECTORY is missing or corrupted.\n");

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Update failed.");
                Console.WriteLine("UPDATE DIRECTORY is missing or corrupted.");
                Console.ResetColor();

                Environment.Exit(0);
            }
        }

        static public bool Scan()
        {
            string Errors = "";
            string[] CheckFor = {
                $"{Shell.Root()}\\Files.x72\\img\\src", $"{Shell.Root()}\\Files.x72\\img\\ain",
                $"{Shell.Root()}\\SoftwareDistribution\\UpdatePackages\\UPR\\UPR.exe",
                $"{Shell.Root()}\\Files.x72\\Packages\\data\\amd", $"{Shell.Root()}\\Files.x72\\Packages\\data\\oza",
                $"{Shell.Root()}\\RESTORE.exe", $"{Shell.Root()}\\Sysfail\\RECOVERY", $"{Shell.Root()}\\Files.x72\\Temp\\set\\Config.set"
            };

            // Scan the system.
            for (int i = 0; i < CheckFor.Length; i++)
            {
                if (Directory.Exists(CheckFor[i]) && !Directory.EnumerateFileSystemEntries(CheckFor[i], "*", SearchOption.AllDirectories).Any())
                    Errors += $"{CheckFor[i]} ";

                else if (!Directory.Exists(CheckFor[i]) && !File.Exists(CheckFor[i]))
                    Errors += $"{CheckFor[i]} ";
            }

            // Check for corrupted files.
            if (!string.IsNullOrEmpty(Errors.Trim()))
            {
                string[] Missing = Errors.Trim().Split();
                string NoteCurrentTime = DateTime.Now.ToString("[dd-MM-yyyy], [HH:mm:ss]");

                File.AppendAllText($"{Shell.Root()}\\Files.x72\\Temp\\log\\Crashreport.log", $"{NoteCurrentTime}, [{string.Join(", ", Missing)}]\n");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{Missing.Length} Errors were Found!");
                Console.WriteLine("Your PC ran into a problem :(");
                Console.ResetColor();
                Console.Write("Press any Key to Continue.");
                Console.ReadKey();
                Console.WriteLine();
                SYSRestore();
                Console.WriteLine("A restart is recommended.");
                return false;
            }

            return true;
        }

        static public void StartUpApps()
        {
            if (Directory.Exists($"{Shell.Root()}\\Files.x72\\StartUp"))
            {
                foreach (string App in Directory.GetFiles($"{Shell.Root()}\\Files.x72\\StartUp", "*.aos"))
                {
                    foreach (string Command in File.ReadLines(App)) Program.AOs(Command, false);
                }
            }
        }

        static public void AskPass()
        {
            if (File.Exists($"{Shell.Root()}\\Files.x72\\Temp\\set\\User.set"))
            {
                while (true)
                {
                    Console.Write("Enter password: ");
                    string Password = Console.ReadLine();
                    if (Password == File.ReadAllText($"{Shell.Root()}\\Files.x72\\Temp\\set\\User.set")) break;
                    else Console.WriteLine("Incorrect password.");
                }
            }
        }

        static public string Root()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        static public string SetTerminalPrompt(string[] Flags)
        {
            string PromptMessage = "";
            foreach (string i in Flags)
            {
                if (i.ToLower() == "-v") PromptMessage += new Terminal().Version;
                else if (i.ToLower() == "-s") PromptMessage += " ";
                else if (i.ToLower() == "-t") PromptMessage += DateTime.Now.ToString("HH:mm:ss");
                else if (i.ToLower() == "-d") PromptMessage += DateTime.Now.ToString("dd-MM-yyyy");
                else if (i.ToLower() == "-p") PromptMessage += Directory.GetCurrentDirectory();
                else if (i.ToLower() == "-n") PromptMessage += Path.GetPathRoot(Environment.SystemDirectory);
                else PromptMessage += i;
            }

            return PromptMessage;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // The BIOS or Bootloader.
            // First remove all repeated elemets from the args array then check for supported arguments.
            string[] Argv = Collections.Array.Filter(args);
            if (Argv.Contains("--help") || Argv.Contains("-h") || Argv.Contains("/?"))
            {
                string[] SYSHelpCenter = {
                    "AOs is a Command line utility designed to improve your Efficiency and Productivity.",
                    "Usage: AOs [Option]",
                    "",
                    "Options:",
                    "-h, --help            -> Displays all supported arguments.",
                    "-v, --version         -> Displays the version.",
                    "-ma, --modeaos        -> Launches in default mode.",
                    "-md, --modeadmin      -> Launches in administrator mode.",
                    "-ms, --modesafe       -> Launches in safe mode."
                };

                Console.WriteLine(string.Join("\n", SYSHelpCenter));
            }

            else if (Argv.Contains("--version") || Argv.Contains("-v")) Console.WriteLine(new Terminal().Version);
            else if (Argv.Contains("--modeaos") || Argv.Contains("-ma")) AOs();
            else if (Argv.Contains("--modeadmin") || Argv.Contains("-md")) Administrator();
            else if (Argv.Contains("--modesafe") || Argv.Contains("-ms")) SafeMode();
            else if (Argv.Length > 0)
            {
                Error.Args(Argv);
                Console.WriteLine("Type `AOs -h' for more information.");
            }

            else AOs();
            Environment.Exit(0);
        }

        static public void AOs(string input="", bool RunIndependent=true, bool RanOnBoot=true)
        {
            Terminal AOs = new Terminal("AOs", "$ ");
            if (RunIndependent)
            {
                if (RanOnBoot) BIOS();
                AOs.Login();
                if (RanOnBoot) BootLoader();
            }

            // Main loop.
            bool Loop = true;
            string Prompt = AOs.Prompt;
            while (Loop)
            {
                // Live update the prompt.
                if (Prompt == "$ ") AOs.Prompt = Prompt;
                else AOs.Prompt = Shell.SetTerminalPrompt(Prompt.Split());

                // Take input.
                if (!RunIndependent) Loop = false;
                string Command = AOs.TakeInput(input);
                string[] ListOfToks = new Lexer(Command).Tokens;

                // Split the ListOfToks into a Command and Args variable and array respectively.
                Command = Collections.Array.Trim(ListOfToks).FirstOrDefault();
                string[] Args = Collections.Array.Trim(Collections.Array.Reduce(ListOfToks));
                if (Args.FirstOrDefault() == Command && (Args.Length != 0 || Args != null)) Args = Collections.Array.Trim(Args.Skip(1).ToArray());

                // Parse input.
                if (Collections.String.IsEmpty(Command)) continue;
                else if (Command == "∞")
                {
                    if (Args.Length == 0 || Args == null) Console.WriteLine(Command);
                    else Error.Args(Args);
                }

                else if (double.TryParse(Command, out double _))
                {
                    if (Args.Length == 0 || Args == null) Console.WriteLine(Command);
                    else Error.Args(Args);
                }

                else if (Collections.String.IsString(Command))
                {
                    string NewString = "";
                    foreach (string i in ListOfToks)
                    {
                        if (Collections.String.IsString(i)) NewString += Shell.Strings(i);
                        else NewString += i;
                    }

                    Console.WriteLine(NewString);
                }

                // Parse commands.
                else if (Command.ToLower() == "about" || Command.ToLower() == "info")
                    Console.WriteLine("AOs is a Command line utility designed to improve your Efficiency and Productivity.");

                else if (Command.ToLower() == "shutdown" || Command.ToLower() == "quit" || Command.ToLower() == "exit")
                {
                    if (Args.Length == 0 || Args == null) Environment.Exit(0);
                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "restart" || Command.ToLower() == "reload" || Command.ToLower() == "refresh")
                {
                    if (Args.Length == 0 || Args == null)
                    {
                        string AOsBinaryFile = Process.GetCurrentProcess().MainModule.FileName;
                        Shell.CommandPrompt($"\"{AOsBinaryFile}\"");
                        Environment.Exit(0);
                    }

                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "cls" || Command.ToLower() == "clear")
                {
                    if (Args.Length == 0 || Args == null) AOs.ClearConsole();
                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "help" || Command.ToLower() == "-h" || Command.ToLower() == "/?")
                {
                    if (Args.Length == 0 || Args == null) AOs.GetHelp("-a");
                    else AOs.GetHelp(Args.FirstOrDefault().ToString());
                }

                else if (Command.ToLower() == "version" || Command.ToLower() == "ver" || Command.ToLower() == "-v")
                {
                    if (Args.Length == 0 || Args == null) Console.WriteLine(AOs.Version);
                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "credits")
                {
                    if (Args.Length == 0 || Args == null) AOs.Credits();
                    else Error.Args(Args);
                }

                else if (Command == "AOs1000")
                {
                    if (Args.Length == 0 || Args == null)
                    {
                        string[] AOs1000 = {
                            "AOs1000!",
                            "Congratulations for hitting 1000 Lines Of Code in AOs!",
                            "It was the first program to ever reach these many Lines Of Code!"
                        };

                        Console.WriteLine(string.Join("\n", AOs1000));
                    }

                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "history")
                {
                    if (Args.Length == 0 || Args == null) Terminal.History.GetHistory();
                    else if (Args.FirstOrDefault() == "-c" && Args.Length == 1) Terminal.History.ClearHistory();
                    else Error.Args(Args);
                }

                else if (Command == ">")
                {
                    if (Args.Length == 0 || Args == null) Shell.StartApp("cmd");
                    else Shell.CommandPrompt(string.Join(" ", Args));
                }

                else if (Command == "@")
                {
                    string[] PromptHelpCenter = {
                        "Displays overload commands.",
                        "Usage: @[args]",
                        "",
                        "Arguments:",
                        "alternate   -> Displays a list of alternate commands.",
                        "hackgod     -> Pretends to hack but actually is harmless.",
                        "regedit     -> Enables registry in administrator.",
                        "itsmagic    -> It's magic It's magic.",
                        "studybyte   -> Starts studybyte.",
                        "deeplock    -> Locks windows itself.",
                        "deepscan    -> Scans the host operating system.",
                        "log         -> Creates a file for logging purpose.",
                        "todo        -> Create and manages a todo list.",
                        "help        -> Displays a list of all overload commands."
                    };

                    if (Args.Length == 0 || Args == null) Console.WriteLine(string.Join("\n", PromptHelpCenter));
                    else if (Args.FirstOrDefault().ToLower() == "-h" || Args.FirstOrDefault().ToLower() == "/?" || Args.FirstOrDefault().ToLower() == "help")
                    {
                        Console.WriteLine(string.Join("\n", PromptHelpCenter));
                        break;
                    }

                    // Rickroll!!!
                    else if (Args.FirstOrDefault().ToLower() == "itsmagic") Shell.StartApp("https://youtu.be/dQw4w9WgXcQ");
                    else if (Args.FirstOrDefault().ToLower() == "hackgod") Shell.StartApp("https://hackertyper.net");
                    else if (Args.FirstOrDefault().ToLower() == "studybyte") Shell.StartApp("https://light-lens.github.io/Studybyte");
                    else if (Args.FirstOrDefault().ToLower() == "deeplock") Shell.StartApp(@"C:\WINDOWS\system32\rundll32.exe", "user32.dll,LockWorkStation");
                    else if (Args.FirstOrDefault().ToLower() == "deepscan") Shell.StartApp($"{Shell.Root()}\\Sysfail\\rp\\FixCorruptedSystemFiles.bat", AppAdmin: true);
                    else if (Args.FirstOrDefault().ToLower() == "regedit") Directory.CreateDirectory($"{Shell.Root()}\\Files.x72\\Packages\\appdata\\REGISTRY");
                    else if (Args.FirstOrDefault().ToLower() == "log")
                    {
                        // {4c4f4747494e4720544849532046494c45} -> LOGGING THIS FILE
                        string[] LogArgs = Collections.Array.Trim(Args.Skip(1).ToArray());
                        if (LogArgs.Length == 0 || LogArgs == null) Error.NoArgs();
                        else if (LogArgs.Length > 1) Error.TooManyArgs(LogArgs);
                        else if (LogArgs.Length < 1) Error.TooFewArgs(LogArgs);
                        else
                        {
                            LogArgs[0] = Shell.Strings(LogArgs[0]);
                            File.WriteAllText(LogArgs[0], "{4c4f4747494e4720544849532046494c45}\n\n");
                        }
                    }

                    else if (Args.FirstOrDefault().ToLower() == "alternate")
                    {
                        string[] AlternateCommands = {
                            "about, info",
                            "shutdown, quit, exit",
                            "restart, reload, refresh",
                            "cls, clear",
                            "version, ver",
                            "console, terminal, cmd",
                            "calendar, time, date, clock",
                            "alarm, timer, stopwatch",
                            "run, start, call",
                            "ls, dir",
                            "touch, create",
                            "del, rm, delete, remove",
                            "ren, rename",
                            "copy, xcopy, robocopy",
                            "pixstore, winget",
                            "pixelate, leaf, corner",
                            "electron, builder, amdik, notepad"
                        };

                        Console.WriteLine(string.Join("\n", AlternateCommands));
                    }

                    else if (Args.FirstOrDefault().ToLower() == "todo")
                    {
                        string[] TODOHelp = {
                            "Displays a list of all todo arguments",
                            "Usage: @todo [args] [taskname]",
                            "",
                            "Arguments:",
                            "add      -> Adds a task.",
                            "delete   -> Deletes a task.",
                            "cut      -> Marks the task as completed.",
                            "list     -> Shows all pending tasks.",
                            "done     -> Shows all completed tasks.",
                            "help     -> Displays a list of all todo arguments."
                        };

                        string[] TODOArgs = Collections.Array.Trim(Args.Skip(1).ToArray());
                        if (TODOArgs.Length == 0 || TODOArgs == null) Console.WriteLine(string.Join("\n", TODOHelp));
                        else if ((TODOArgs.FirstOrDefault() == "help" || TODOArgs.FirstOrDefault() == "-h" || TODOArgs.FirstOrDefault() == "/?") && TODOArgs.Length == 1) Console.WriteLine(string.Join("\n", TODOHelp));
                        else if (TODOArgs.FirstOrDefault() == "list" && TODOArgs.Length == 1)
                        {
                            string[] Tasks = File.ReadLines($"{Shell.Root()}\\Files.x72\\etc\\TODO.txt").ToArray();
                            for (int i = 0; i < Tasks.Length; i++) Console.WriteLine($"{i}. {Tasks[i]}");
                        }

                        else if (TODOArgs.FirstOrDefault() == "done" && TODOArgs.Length == 1)
                        {
                            string[] Tasks = File.ReadLines($"{Shell.Root()}\\Files.x72\\etc\\DONE.txt").ToArray();
                            for (int i = 0; i < Tasks.Length; i++) Console.WriteLine($"{i}. {Tasks[i]}");
                        }

                        else if (TODOArgs.Length > 2) Error.TooManyArgs(TODOArgs);
                        else if (TODOArgs.Length < 2) Error.TooFewArgs(TODOArgs);
                        else if (TODOArgs.FirstOrDefault() == "add")
                        {
                            if (!File.Exists($"{Shell.Root()}\\Files.x72\\etc\\TODO.txt")) File.Create($"{Shell.Root()}\\Files.x72\\etc\\TODO.txt").Dispose();
                            string TaskName = Shell.Strings(TODOArgs[1]);
                            string[] Tasks = File.ReadLines($"{Shell.Root()}\\Files.x72\\etc\\TODO.txt").ToArray();

                            if (!Tasks.Any(TaskName.Contains)) File.AppendAllText($"{Shell.Root()}\\Files.x72\\etc\\TODO.txt", $"{TaskName}\n");
                            else Console.WriteLine("This task already exists.");
                        }

                        else if (TODOArgs.FirstOrDefault() == "delete")
                        {
                            string TaskName = Shell.Strings(TODOArgs[1]);
                            if (File.Exists($"{Shell.Root()}\\Files.x72\\etc\\TODO.txt"))
                            {
                                string[] Tasks = File.ReadLines($"{Shell.Root()}\\Files.x72\\etc\\TODO.txt").ToArray();
                                if (Tasks.Any(TaskName.Contains))
                                {
                                    List<string> TaskList = new List<string>(Tasks);
                                    TaskList.Remove(TaskName);
                                    Tasks = TaskList.ToArray();
                                    File.WriteAllLines($"{Shell.Root()}\\Files.x72\\etc\\TODO.txt", Tasks);
                                }

                                else Console.WriteLine("No such task found.");
                            }

                            else Console.WriteLine("No todo list found.");
                        }

                        else if (TODOArgs.FirstOrDefault() == "cut")
                        {
                            string TaskName = Shell.Strings(TODOArgs[1]);
                            if (File.Exists($"{Shell.Root()}\\Files.x72\\etc\\TODO.txt"))
                            {
                                string[] Tasks = File.ReadLines($"{Shell.Root()}\\Files.x72\\etc\\TODO.txt").ToArray();
                                if (Tasks.Any(TaskName.Contains))
                                {
                                    List<string> TaskList = new List<string>(Tasks);
                                    TaskList.Remove(TaskName);
                                    Tasks = TaskList.ToArray();
                                    File.WriteAllLines($"{Shell.Root()}\\Files.x72\\etc\\TODO.txt", Tasks);
                                    File.AppendAllText($"{Shell.Root()}\\Files.x72\\etc\\DONE.txt", $"{TaskName}\n");
                                }

                                else Console.WriteLine("No such task found.");
                            }

                            else Console.WriteLine("No todo list found.");
                        }

                        else Error.Args(Args);
                    }

                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "wait")
                {
                    if (Args.Length == 0 || Args == null)
                    {
                        Console.Write("Enter the number of seconds to wait> ");
                        int Sec = Convert.ToInt32(Console.ReadLine());

                        Thread.Sleep(Sec);
                    }

                    else if (int.TryParse(Args.FirstOrDefault(), out int Num) && Args.Length == 1) Thread.Sleep(Num);
                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "prompt")
                {
                    Prompt = "";
                    if (Args.Length == 0 || Args == null) Prompt = "$ ";
                    else
                    {
                        foreach (string i in Args)
                        {
                            if (Collections.String.IsString(i)) Prompt += Shell.Strings(i);
                            else if (i.ToLower() == "-h" || i.ToLower() == "/?" || i.ToLower() == "--help")
                            {
                                string[] PromptHelpCenter = {
                                    "Specifies a new command prompt.",
                                    "Usage: prompt [Option]",
                                    "",
                                    "Options:",
                                    "-h, --help      -> Displays the help message.",
                                    "-r, --reset     -> $ (dollar sign, reset the prompt)",
                                    "-s, --space     -> (space)",
                                    "-v, --version   -> Current AOs version",
                                    "-t, --time      -> Current time",
                                    "-d, --date      -> Current date",
                                    "-p, --path      -> Current path",
                                    "-n, --drive     -> Current drive"
                                };

                                Console.WriteLine(string.Join("\n", PromptHelpCenter));
                                Prompt = AOs.Prompt;
                                break;
                            }

                            else if (i.ToLower() == "-r" || i.ToLower() == "--reset" || i.ToLower() == "--restore" || i.ToLower() == "--default")
                            {
                                Prompt = "$ ";
                                break;
                            }

                            else if (i.ToLower() == "-v" || i.ToLower() == "--ver" || i.ToLower() == "--version") Prompt += "-v ";
                            else if (i.ToLower() == "-s" || i.ToLower() == "--space") Prompt += "-s ";
                            else if (i.ToLower() == "-t" || i.ToLower() == "--time") Prompt += "-t ";
                            else if (i.ToLower() == "-d" || i.ToLower() == "--date") Prompt += "-d ";
                            else if (i.ToLower() == "-p" || i.ToLower() == "--path") Prompt += "-p ";
                            else if (i.ToLower() == "-n" || i.ToLower() == "--drive") Prompt += "-n ";
                            else if (i.StartsWith("-"))
                            {
                                Error.Args(i);
                                Prompt = AOs.Prompt;
                                break;
                            }

                            else Prompt += $"{i} ";
                        }
                    }
                }

                else if (Command.ToLower() == "color")
                {
                    if (Args.Length == 0 || Args == null) Error.NoArgs();
                    else Shell.CommandPrompt($"color {string.Join(" ", Args)}");
                }

                else if (Command.ToLower() == "title")
                {
                    if (Args.Length == 0 || Args == null) Error.NoArgs();
                    else Console.Title = string.Join(" ", Args);
                }

                else if (Command.ToLower() == "calendar" || Command.ToLower() == "time" || Command.ToLower() == "date" || Command.ToLower() == "clock")
                {
                    if (Args.Length == 0 || Args == null)
                    {
                        Console.WriteLine(DateTime.Now.ToString("h:mm:ss tt"));
                        Console.WriteLine(DateTime.Now.ToString("dddd, dd MMMM yyyy"));
                    }

                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "console" || Command.ToLower() == "terminal" || Command.ToLower() == "cmd")
                {
                    if (Args.Length == 0 || Args == null) Shell.StartApp("cmd");
                    else Shell.CommandPrompt(string.Join(" ", Args));
                }

                else if (Command.ToLower() == "shout")
                {
                    if (Args.Length == 0 || Args == null) Error.NoArgs();
                    else
                    {
                        string NewString = "";
                        foreach (string i in Args)
                        {
                            if (Collections.String.IsString(i)) NewString += Shell.Strings(i);
                            else NewString += i;
                        }

                        Console.WriteLine(NewString);
                    }
                }

                else if (Command.ToLower() == "getkey")
                {
                    if (Args.Length == 0 || Args == null) Error.NoArgs();
                    else
                    {
                        string NewString = "";
                        foreach (string i in Args)
                        {
                            if (Collections.String.IsString(i)) NewString += Shell.Strings(i);
                            else NewString += i;
                        }

                        Console.Write(NewString);
                        Console.ReadKey();
                        Console.WriteLine();
                    }
                }

                else if (Command.ToLower() == "get")
                {
                    if (Args.Length == 0 || Args == null) Error.NoArgs();
                    else
                    {
                        string NewString = "";
                        foreach (string i in Args)
                        {
                            if (Collections.String.IsString(i)) NewString += Shell.Strings(i);
                            else NewString += i;
                        }

                        Console.Write(NewString);
                        Console.ReadLine();
                    }
                }

                else if (Command.ToLower() == "pause")
                {
                    if (Args.Length == 0 || Args == null) Console.Write("Press any Key to Continue.");
                    else
                    {
                        string NewString = "";
                        foreach (string i in Args)
                        {
                            if (Collections.String.IsString(i)) NewString += Shell.Strings(i);
                            else NewString += i;
                        }

                        Console.Write(NewString);
                    }

                    Console.ReadKey();
                    Console.WriteLine();
                }

                else if (Command.ToLower() == "apps" || Command.ToLower() == "installedapps")
                {
                    if (Args.Length == 0 || Args == null) Shell.StartApp("shell:AppsFolder");
                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "alarm" || Command.ToLower() == "timer" || Command.ToLower() == "stopwatch")
                {
                    if (Args.Length == 0 || Args == null) Shell.StartApp("ms-clock:");
                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "calculator" || Command.ToLower() == "math")
                {
                    if (Args.Length == 0 || Args == null) Shell.StartApp("ms-calculator:");
                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "music" || Command.ToLower() == "video" || Command.ToLower() == "media")
                {
                    if (Args.Length == 0 || Args == null) Shell.StartApp("mswindowsmusic:");
                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "paint")
                {
                    if (Args.Length == 0 || Args == null) Shell.StartApp("mspaint");
                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "onenote")
                {
                    if (Args.Length == 0 || Args == null) Shell.StartApp("onenote:");
                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "onedrive")
                {
                    if (Args.Length == 0 || Args == null)
                    {
                        string OneDrivePath = Environment.GetEnvironmentVariable("onedrive").ToString();
                        if (!Collections.String.IsEmpty(OneDrivePath)) Shell.StartApp(OneDrivePath);
                    }

                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "electron" || Command.ToLower() == "builder" || Command.ToLower() == "amdik" || Command.ToLower() == "notepad")
                {
                    if (Args.Length == 0 || Args == null) Shell.StartApp("notepad");
                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "paint3d")
                {
                    if (Args.Length == 0 || Args == null) Shell.StartApp("ms-paint:");
                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "notepad")
                {
                    if (Args.Length == 0 || Args == null) Shell.StartApp("onenote:");
                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "settings")
                {
                    if (Args.Length == 0 || Args == null) Shell.StartApp("ms-settings:");
                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "screensnip")
                {
                    if (Args.Length == 0 || Args == null) Shell.StartApp("ms-screenclip:");
                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "snipandsketch")
                {
                    if (Args.Length == 0 || Args == null) Shell.StartApp("ms-ScreenSketch:");
                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "minecraft")
                {
                    if (Args.Length == 0 || Args == null) Shell.StartApp("minecraft:");
                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "candycrush")
                {
                    if (Args.Length == 0 || Args == null) Shell.StartApp("candycrushsodasaga:");
                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "run" || Command.ToLower() == "start" || Command.ToLower() == "call")
                {
                    if (Args.Length == 0 || Args == null) Shell.StartApp("cmd");
                    else Shell.StartApp(string.Join(" ", Args));
                }

                else if (Command.ToLower() == "cd")
                {
                    if (Args.Length == 0 || Args == null) Console.WriteLine(Directory.GetCurrentDirectory());
                    else Directory.SetCurrentDirectory(Shell.Strings(string.Join(" ", Args)));
                }

                else if (Command.ToLower() == "cd.")
                {
                    if (Args.Length == 0 || Args == null) Console.WriteLine(Directory.GetCurrentDirectory());
                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "cd..")
                {
                    if (Args.Length == 0 || Args == null) Directory.SetCurrentDirectory("..");
                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "ls" || Command.ToLower() == "dir")
                {
                    if (Args.Length == 0 || Args == null)
                    {
                        Console.WriteLine(Directory.GetCurrentDirectory());
                        string[] Entries = Directory.GetFileSystemEntries(".", "*");
                        foreach (string Entry in Entries) Console.WriteLine(Entry);
                    }

                    else
                    {
                        Console.WriteLine(Directory.GetCurrentDirectory());
                        foreach (string i in Args)
                        {
                            if (i.ToLower() == "-h" || i.ToLower() == "/?" || i.ToLower() == "--help")
                            {
                                string[] LSHelpCenter = {
                                    "Displays a list of files and subdirectories in a directory.",
                                    "Usage: ls [Option]",
                                    "",
                                    "Options:",
                                    "-f   -> Display only files.",
                                    "-d   -> Display only folders.",
                                    "-a   -> Display all files and folders.",
                                };

                                Console.WriteLine(string.Join("\n", LSHelpCenter));
                                break;
                            }

                            else if (i.ToLower() == "-a" || i.ToLower() == "--all")
                            {
                                string[] Entries = Directory.GetFileSystemEntries(".", "*");
                                foreach (string Entry in Entries) Console.WriteLine(Entry);
                                break;
                            }

                            else if (i.ToLower() == "-f" || i.ToLower() == "--files")
                            {
                                string[] Files = Directory.GetFiles(".");
                                foreach (string File in Files) Console.WriteLine(File);
                            }

                            else if (i.ToLower() == "-d" || i.ToLower() == "--folder" || i.ToLower() == "--directories")
                            {
                                string[] Directories = Directory.GetDirectories(".");
                                foreach (string Folder in Directories) Console.WriteLine(Folder);
                            }

                            else
                            {
                                Error.Args(i);
                                break;
                            }
                        }
                    }
                }

                else if (Command.ToLower() == "touch" || Command.ToLower() == "create")
                {
                    if (Args.Length == 0 || Args == null) Error.NoArgs();
                    else if (Args.Length > 1) Error.TooManyArgs(Args);
                    else
                    {
                        string FileOrFolderName = Shell.Strings(string.Join(" ", Args));
                        if (FileOrFolderName.ToString().ToLower() == "con") Console.WriteLine("Hello CON!");
                        else if ((FileOrFolderName.EndsWith("\\") || FileOrFolderName.EndsWith("/")) && !Directory.Exists(FileOrFolderName))
                            Directory.CreateDirectory(FileOrFolderName.Substring(0, FileOrFolderName.Length - 1));

                        else if (!File.Exists(FileOrFolderName) && !Directory.Exists(FileOrFolderName)) File.Create(FileOrFolderName).Dispose();
                        else Console.WriteLine("File or directory already exist.");
                    }
                }

                else if (Command.ToLower() == "del" || Command.ToLower() == "rm" || Command.ToLower() == "delete" || Command.ToLower() == "remove")
                {
                    if (Args.Length == 0 || Args == null) Error.NoArgs();
                    else if (Args.Length > 1) Error.TooManyArgs(Args);
                    else
                    {
                        string FileOrFolderName = Shell.Strings(string.Join(" ", Args));

                        if (FileOrFolderName.ToString().ToLower() == "con") Console.WriteLine("Don't Delete CON.");
                        else if (Directory.Exists(FileOrFolderName)) Directory.Delete(FileOrFolderName.Substring(0, FileOrFolderName.Length - 1), true);
                        else if (File.Exists(FileOrFolderName)) File.Delete(FileOrFolderName);
                        else Console.WriteLine("No such file or directory.");
                    }
                }

                else if (Command.ToLower() == "ren" || Command.ToLower() == "rename")
                {
                    if (Args.Length == 0 || Args == null) Error.NoArgs();
                    else if (Args.Length > 1) Error.TooManyArgs(Args);
                    else
                    {
                        string FileOrFolderName = Shell.Strings(string.Join(" ", Args));
                        if (FileOrFolderName.EndsWith("\\") || FileOrFolderName.EndsWith("/"))
                            FileOrFolderName = FileOrFolderName.Substring(0, FileOrFolderName.Length - 1);

                        if (FileOrFolderName.ToString().ToLower() == "con") Console.WriteLine("Hello CON!");
                        else if (Directory.Exists(FileOrFolderName) || File.Exists(FileOrFolderName)) Shell.CommandPrompt($"ren {FileOrFolderName}");
                        else Console.WriteLine("No such file or directory.");
                    }
                }

                else if (Command.ToLower() == "copy" || Command.ToLower() == "xcopy" || Command.ToLower() == "robocopy")
                {
                    if (Args.Length == 0 || Args == null) Error.NoArgs();
                    else if (Args.Length > 1) Error.TooManyArgs(Args);
                    else
                    {
                        string FileOrFolderName = Shell.Strings(string.Join(" ", Args));
                        if (FileOrFolderName.EndsWith("\\") || FileOrFolderName.EndsWith("/"))
                            FileOrFolderName = FileOrFolderName.Substring(0, FileOrFolderName.Length - 1);

                        if (Directory.Exists(FileOrFolderName) || File.Exists(FileOrFolderName)) Shell.CommandPrompt($"{Command} {FileOrFolderName}");
                        else Console.WriteLine("No such file or directory.");
                    }
                }

                else if (Command.ToLower() == "move")
                {
                    if (Args.Length == 0 || Args == null) Error.NoArgs();
                    else if (Args.Length > 1) Error.TooManyArgs(Args);
                    else
                    {
                        string FileOrFolderName = Shell.Strings(string.Join(" ", Args));
                        if (FileOrFolderName.EndsWith("\\") || FileOrFolderName.EndsWith("/"))
                            FileOrFolderName = FileOrFolderName.Substring(0, FileOrFolderName.Length - 1);

                        if (Directory.Exists(FileOrFolderName) || File.Exists(FileOrFolderName)) Shell.CommandPrompt($"move {FileOrFolderName}");
                        else Console.WriteLine("No such file or directory.");
                    }
                }

                else if (Command.ToLower() == "lock")
                {
                    string Password = "";
                    if (Args.Length == 0 || Args == null)
                    {
                        Console.WriteLine("Secure your System.");
                        Console.Write("Set Password: ");
                        Password = Console.ReadLine();
                    }

                    else Password = Shell.Strings(string.Join(" ", Args));

                    // Lock the System.
                    while(true)
                    {
                        AOs.ClearConsole();
                        Console.Write("Enter Password: ");
                        string askPin = Console.ReadLine();
                        if (askPin == Password)
                        {
                            Console.WriteLine("Correct password.");
                            Console.Write("Continue.");
                            Console.ReadKey();
                            Console.WriteLine();
                            break;
                        }

                        else
                        {
                            Console.WriteLine("Incorrect password.");
                            Console.Write("Continue.");
                            Console.ReadKey();
                            Console.WriteLine();
                        }
                    }

                    AOs.ClearConsole();
                }

                else if (Command.ToLower() == "pixstore" || Command.ToLower() == "winget")
                {
                    if (Args.Length == 0 || Args == null) Error.NoArgs();
                    else Shell.CommandPrompt($"winget {string.Join(" ", Args)}");
                }

                else if (Command.ToLower() == "pixelate" || Command.ToLower() == "leaf" || Command.ToLower() == "corner")
                {
                    if (Args.Length == 0 || Args == null) Shell.StartApp("https://www.youtube.com/c/OnestateCoding/");
                    else Shell.StartApp(Shell.Strings(string.Join(" ", Args)));
                }

                else if (Command.ToLower() == "commit")
                {
                    if (Args.Length == 0 || Args == null) Error.NoArgs();
                    else if (Args.Length > 2) Error.TooManyArgs(Args);
                    else if (Args.Length < 2) Error.TooFewArgs(Args);
                    else
                    {
                        for (int i = 0; i < Args.Length; i++)
                        {
                            if (Collections.String.IsString(Args[i])) Args[i] = Shell.Strings(Args[i]);
                        }


                        if (File.Exists(Args[0]))
                        {
                            if (File.ReadLines(Args[0]).FirstOrDefault() == "{4c4f4747494e4720544849532046494c45}")
                            {
                                File.AppendAllText(Args[0], $"{Args[1]}\n");
                                File.AppendAllText(Args[0], $"{DateTime.Now.ToString("[dd-MM-yyyy, HH:mm:ss]")}\n");
                            }

                            else File.AppendAllText(Args[0], $"{Args[1]}\n");
                        }

                        else Console.WriteLine("No file found.");
                    }
                }

                else if (Command.ToLower() == "read")
                {
                    if (Args.Length == 0 || Args == null) Error.NoArgs();
                    else if (Args.Length > 1) Error.TooManyArgs(Args);
                    else if (Args.Length < 1) Error.TooFewArgs(Args);
                    else
                    {
                        Args[0] = Shell.Strings(Args[0]);
                        if (File.Exists(Args[0])) Console.WriteLine(File.ReadAllText(Args[0]));
                        else Console.WriteLine("No file found.");
                    }
                }

                // Run the Administrator.
                else if (Command.ToLower() == "admin")
                {
                    if (Args.Length == 0 || Args == null) Administrator(RanOnBoot: false);
                    else Administrator(string.Join(" ", Args), false);
                }

                // Run '.aos' files.
                else if (Command.ToLower().EndsWith(".aos"))
                {
                    if (Args.Length == 0 || Args == null)
                    {
                        if (File.Exists(Command))
                        {
                            foreach (string Line in File.ReadLines(Command)) Program.AOs(Line, false);
                        }
                    }

                    else Error.Args(Args);
                }

                // Check for apps in environment variable.
                else if (File.Exists(Command.ToLower()))
                {
                    if (Args.Length == 0 || Args == null) Shell.CommandPrompt(Command.ToLower());
                    else Shell.CommandPrompt($"\"{Command.ToLower()}\" {string.Join(" ", Args)}");
                }

                else if (!Collections.String.IsEmpty(Environment.GetEnvironmentVariable(Command.ToLower())))
                {
                    if (Args.Length == 0 || Args == null) Console.WriteLine(Environment.GetEnvironmentVariable(Command.ToLower()));
                    else Error.Args(Args);
                }

                else if (!Collections.String.IsEmpty(Shell.LocateEXE(Command.ToLower())))
                {
                    if (Args.Length == 0 || Args == null) Shell.CommandPrompt($"{Command.ToLower()}");
                    else Shell.CommandPrompt($"\"{Command.ToLower()}\" {string.Join(" ", Args)}");
                }

                else if (!Collections.String.IsEmpty(Shell.LocateEXE($"{Command.ToLower()}.exe")))
                {
                    if (Args.Length == 0 || Args == null) Shell.CommandPrompt($"{Command.ToLower()}.exe");
                    else Shell.CommandPrompt($"\"{Command.ToLower()}.exe\" {string.Join(" ", Args)}");
                }

                else if (!Collections.String.IsEmpty(Shell.LocateEXE($"{Command.ToLower()}.bat")))
                {
                    if (Args.Length == 0 || Args == null) Shell.CommandPrompt($"{Command.ToLower()}.bat");
                    else Shell.CommandPrompt($"\"{Command.ToLower()}.bat\" {string.Join(" ", Args)}");
                }

                else if (!Collections.String.IsEmpty(Shell.LocateEXE($"{Command.ToLower()}.cmd")))
                {
                    if (Args.Length == 0 || Args == null) Shell.CommandPrompt($"{Command.ToLower()}.cmd");
                    else Shell.CommandPrompt($"\"{Command.ToLower()}.cmd\" {string.Join(" ", Args)}");
                }

                else Error.Command(Command);
            }
        }

        static public void Administrator(string input="", bool RunIndependent=true, bool RanOnBoot=true)
        {
            if (!RunIndependent) RanOnBoot = false;
            if (!RanOnBoot) Shell.AskPass();

            // Init terminal
            Terminal Admin = new Terminal("Administrator", "admin$ ");

            // Show the login screen.
            if (RunIndependent)
            {
                if (RanOnBoot) BIOS();
                Admin.Login();
                if (RanOnBoot) BootLoader();
            }

            // Main loop.
            bool Loop = true;
            while (Loop)
            {
                // Take input.
                if (!RunIndependent) Loop = false;
                string Command = Admin.TakeInput(input);
                string[] ListOfToks = new Lexer(Command).Tokens;

                // Split the ListOfToks into a Command and Args variable and array respectively.
                Command = Collections.Array.Trim(ListOfToks).FirstOrDefault();
                string[] Args = Collections.Array.Trim(ListOfToks);
                if (Args.FirstOrDefault() == Command && (Args.Length != 0 || Args != null)) Args = Collections.Array.Trim(Args.Skip(1).ToArray());

                // Parse input.
                if (Collections.String.IsEmpty(Command)) continue;
                else if (Command.ToLower() == "help" || Command.ToLower() == "-h" || Command.ToLower() == "/?")
                {
                    if (Args.Length == 0 || Args == null) Admin.GetHelp("-d");
                    else Admin.GetHelp(Args.FirstOrDefault().ToString());
                }

                else if (Command.ToLower() == "cls" || Command.ToLower() == "clear")
                {
                    if (Args.Length == 0 || Args == null) Admin.ClearConsole();
                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "update")
                {
                    if (Args.Length == 0 || Args == null) Shell.CheckForUpdates(Admin.VersionNum);
                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "scan")
                {
                    if (Args.Length == 0 || Args == null)
                    {
                        Console.WriteLine("Scanning.");
                        if (Shell.Scan()) Console.WriteLine("Your PC is working fine.");
                    }

                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "lock")
                {
                    string[] LOCKHelp = {
                        "Locks the System at Start-up.",
                        "Usage: lock [args][password]",
                        "",
                        "Arguments:",
                        "remove   -> Remove current password."
                    };

                    if (Args.Length == 0 || Args == null)
                    {
                        Console.Write("Set Password: ");
                        string Password = Console.ReadLine();

                        File.WriteAllText($"{Shell.Root()}\\Files.x72\\Temp\\set\\User.set", Password);
                        Console.WriteLine("Your password was set successfully.");
                    }

                    else if ((Args.FirstOrDefault() == "help" || Args.FirstOrDefault() == "-h" || Args.FirstOrDefault() == "/?") && Args.Length == 1) Console.WriteLine(string.Join("\n", LOCKHelp));
                    else if ((Args.FirstOrDefault() == "remove" || Args.FirstOrDefault() == "-rm") && Args.Length == 1)
                    {
                        if (File.Exists($"{Shell.Root()}\\Files.x72\\Temp\\set\\User.set"))
                        {
                            File.Delete($"{Shell.Root()}\\Files.x72\\Temp\\set\\User.set");
                            Console.WriteLine("Password removed successfully.");
                        }

                        Console.WriteLine("Your system isn't password protected.");
                    }

                    else if (Args.Length > 1) Error.TooManyArgs(Args);
                    else if (Args.Length < 1) Error.TooFewArgs(Args);
                    else
                    {
                        Args[0] = Shell.Strings(Args[0]);
                        File.WriteAllText($"{Shell.Root()}\\Files.x72\\Temp\\set\\User.set", Args[0]);
                        Console.WriteLine("Your password was set successfully.");
                    }
                }

                else if (Command.ToLower() == "terminate")
                {
                    if (Args.Length == 0 || Args == null) Shell.CommandPrompt("tasklist");
                    else
                    {
                        for (int i = 0; i < Args.Length; i++) Args[i] = Shell.Strings(Args[i]);
                        Shell.CommandPrompt($"taskkill /f /im {string.Join(" ", Args)}");
                    }
                }

                else if (Command.ToLower() == "generate")
                {
                    if (Args.Length == 0 || Args == null)
                    {
                        Random random = new Random();
                        double Generic = random.NextDouble();
                        Console.WriteLine(Generic);
                    }

                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "ran")
                {
                    if (Args.Length == 0 || Args == null) Shell.CommandPrompt("systeminfo");
                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "tree")
                {
                    if (Args.Length == 0 || Args == null) Shell.CommandPrompt("tree");
                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "diagxt")
                {
                    if (Args.Length == 0 || Args == null) Console.WriteLine(File.ReadAllText($"{Shell.Root()}\\Files.x72\\Temp\\set\\Config.set"));
                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "restore")
                {
                    if (Args.Length == 0 || Args == null) Shell.SYSRestore();
                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "reset")
                {
                    if (Args.Length == 0 || Args == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Resetting this PC will delete all your files, settings and apps.");
                        Console.ResetColor();

                        Console.WriteLine("Are you sure? Y/N");
                        Console.Write("> ");
                        ConsoleKeyInfo KeyInput = Console.ReadKey();
                        string GetKey = KeyInput.Key.ToString();
                        Console.WriteLine();
                        if (GetKey.ToLower() == "y")
                        {
                            try
                            {
                                string[] EntrySourcePath = {$"{Shell.Root()}\\Files.x72", $"{Shell.Root()}\\SoftwareDistribution"};

                                Console.WriteLine("Formatting.");
                                foreach (string Source in EntrySourcePath)
                                {
                                    string[] Entries = Directory.GetFileSystemEntries(Source, "*", SearchOption.AllDirectories);
                                    foreach (string Entry in Entries)
                                    {
                                        Console.WriteLine($"Locating {Entry}");
                                        if (Directory.Exists(Entry))
                                        {
                                            Console.WriteLine($"Deleting {Entry}");
                                            Directory.Delete(Entry, true);

                                            Console.WriteLine("Verifying.");
                                            if (!Directory.Exists(Entry)) Console.WriteLine($"Deleted {Entry}");
                                        }

                                        else if (File.Exists(Entry))
                                        {
                                            Console.WriteLine($"Deleting {Entry}");
                                            File.Delete(Entry);

                                            Console.WriteLine("Verifying.");
                                            if (!File.Exists(Entry)) Console.WriteLine($"Deleted {Entry}");
                                        }
                                    }
                                }

                                Console.WriteLine();
                                Shell.SYSRestore();
                                Console.WriteLine("System Reset Completed.");
                                Console.WriteLine("Restarting in 3 seconds.");

                                Thread.Sleep(3);

                                Shell.CommandPrompt($"\"{Process.GetCurrentProcess().MainModule.FileName}\"");
                                Environment.Exit(0);
                            }

                            catch (Exception)
                            {
                                Console.WriteLine("Cannot perform a System Reset.");
                            }
                        }

                        else if (GetKey.ToLower() == "n") Console.WriteLine("System Reset Cancelled!");
                        else
                        {
                            Console.WriteLine("Invalid Key Input.");
                            Console.WriteLine("Cannot perform a System Reset.");
                        }
                    }

                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "regedit" && Directory.Exists($"{Shell.Root()}\\Files.x72\\Packages\\appdata\\REGISTRY"))
                {
                    if (Args.Length == 0 || Args == null) REGEDIT();
                    else Error.Args(Args);
                }

                // Exit or Invalid Command.
                else if (Command.ToLower() == "exit" || Command.ToLower() == "quit")
                {
                    if (Args.Length == 0 || Args == null)
                    {
                        Console.Title = "AOs";
                        break;
                    }

                    else Error.Args(Args);
                }

                else AOs(Command, false);
            }
        }

        static void REGEDIT()
        {
            // Init terminal
            Terminal REGISTRY = new Terminal("REGISTRY", "registry$ ");
            REGISTRY.Login();

            // Main loop.
            bool Loop = true;
            while (Loop)
            {
                // Take input.
                string Command = REGISTRY.TakeInput("");
                string[] ListOfToks = new Lexer(Command).Tokens;

                // Split the ListOfToks into a Command and Args variable and array respectively.
                Command = Collections.Array.Trim(ListOfToks).FirstOrDefault();
                string[] Args = Collections.Array.Trim(ListOfToks);
                if (Args.FirstOrDefault() == Command && (Args.Length != 0 || Args != null)) Args = Collections.Array.Trim(Args.Skip(1).ToArray());

                // Parse input.
                if (Collections.String.IsEmpty(Command)) continue;
                else if (Command.ToLower() == "cls" || Command.ToLower() == "clear")
                {
                    if (Args.Length == 0 || Args == null) REGISTRY.ClearConsole();
                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "reset")
                {
                    if (Args.Length == 0 || Args == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Resetting REGISTRY will delete all your system preferences which may lead to system instability.");
                        Console.ResetColor();

                        Console.WriteLine("Are you sure? Y/N");
                        Console.Write("> ");
                        ConsoleKeyInfo KeyInput = Console.ReadKey();
                        string GetKey = KeyInput.Key.ToString();
                        Console.WriteLine();
                        if (GetKey.ToLower() == "y")
                        {
                            try
                            {
                                Directory.Delete($"{Shell.Root()}\\Files.x72\\Packages\\appdata\\REGISTRY");
                                Console.WriteLine("Deleting changes and Restarting in 3 seconds.");
                                Thread.Sleep(3);

                                string AOsBinaryFile = Process.GetCurrentProcess().MainModule.FileName;
                                Shell.CommandPrompt($"\"{AOsBinaryFile}\"");
                                Environment.Exit(0);
                            }

                            catch (Exception)
                            {
                                Console.WriteLine("Cannot perform a REGISTRY Reset.");
                            }
                        }

                        else if (GetKey.ToLower() == "n") Console.WriteLine("REGISTRY Reset Cancelled!");
                        else
                        {
                            Console.WriteLine("Invalid Key Input.");
                            Console.WriteLine("Cannot perform a REGISTRY Reset.");
                        }
                    }

                    else Error.Args(Args);
                }

                // Exit or Invalid Command.
                else if (Command.ToLower() == "exit" || Command.ToLower() == "quit" || Command.ToLower() == "restart" || Command.ToLower() == "reload" || Command.ToLower() == "refresh")
                {
                    if (Args.Length == 0 || Args == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Exiting will save current changes which may lead to system instability.");
                        Console.ResetColor();

                        Console.WriteLine("Are you sure? Y/N");
                        Console.Write("> ");
                        ConsoleKeyInfo KeyInput = Console.ReadKey();
                        string GetKey = KeyInput.Key.ToString();
                        Console.WriteLine();
                        if (GetKey.ToLower() == "y")
                        {
                            try
                            {
                                Console.WriteLine("Saving changes and Restarting in 3 seconds.");
                                Thread.Sleep(3);

                                string AOsBinaryFile = Process.GetCurrentProcess().MainModule.FileName;
                                Shell.CommandPrompt($"\"{AOsBinaryFile}\"");
                                Environment.Exit(0);
                            }

                            catch (Exception)
                            {
                                Console.WriteLine("Cannot exit REGISTRY.");
                            }
                        }

                        else if (GetKey.ToLower() == "n") Console.WriteLine("REGISTRY Exit Cancelled!");
                        else
                        {
                            Console.WriteLine("Invalid Key Input.");
                            Console.WriteLine("Cannot exit REGISTRY.");
                        }
                    }

                    else Error.Args(Args);
                }

                else Error.Command(Command);
            }
        }

        static void SafeMode()
        {
            // Init terminal
            Terminal SAFE = new Terminal("Safe mode", "safe$ ");

            // Show the login screen.
            BIOS();
            SAFE.Login();
            BootLoader();

            // Main loop.
            bool Loop = true;
            while (Loop)
            {
                // Take input.
                string Command = SAFE.TakeInput("");
                string[] ListOfToks = new Lexer(Command).Tokens;

                // Split the ListOfToks into a Command and Args variable and array respectively.
                Command = Collections.Array.Trim(ListOfToks).FirstOrDefault();
                string[] Args = Collections.Array.Trim(ListOfToks);
                if (Args.FirstOrDefault() == Command && (Args.Length != 0 || Args != null)) Args = Collections.Array.Trim(Args.Skip(1).ToArray());

                // Parse input.
                if (Collections.String.IsEmpty(Command)) continue;
                else if (Command.ToLower() == "help" || Command.ToLower() == "-h" || Command.ToLower() == "/?")
                {
                    if (Args.Length == 0 || Args == null) SAFE.GetHelp("-s");
                    else SAFE.GetHelp(Args.FirstOrDefault().ToString());
                }

                else if (Command.ToLower() == "cls" || Command.ToLower() == "clear")
                {
                    if (Args.Length == 0 || Args == null) SAFE.ClearConsole();
                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "regedit" && Directory.Exists($"{Shell.Root()}\\Files.x72\\Packages\\appdata\\REGISTRY"))
                {
                    if (Args.Length == 0 || Args == null) REGEDIT();
                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "update")
                {
                    if (Args.Length == 0 || Args == null) Shell.CheckForUpdates(SAFE.VersionNum);
                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "scan")
                {
                    Console.WriteLine("Scanning.");
                    if (Shell.Scan()) Console.WriteLine("Your PC is working fine.");
                }

                else if (Command.ToLower() == "diagxt")
                {
                    if (Args.Length == 0 || Args == null) Console.WriteLine(File.ReadAllText($"{Shell.Root()}\\Files.x72\\Temp\\set\\Config.set"));
                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "restore")
                {
                    if (Args.Length == 0 || Args == null) Shell.SYSRestore();
                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "reset")
                {
                    if (Args.Length == 0 || Args == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Resetting this PC will delete all your files, settings and apps.");
                        Console.ResetColor();

                        Console.WriteLine("Are you sure? Y/N");
                        Console.Write("> ");
                        ConsoleKeyInfo KeyInput = Console.ReadKey();
                        string GetKey = KeyInput.Key.ToString();
                        Console.WriteLine();
                        if (GetKey.ToLower() == "y")
                        {
                            try
                            {
                                string[] EntrySourcePath = {$"{Shell.Root()}\\Files.x72", $"{Shell.Root()}\\SoftwareDistribution"};

                                Console.WriteLine("Formatting.");
                                foreach (string Source in EntrySourcePath)
                                {
                                    string[] Entries = Directory.GetFileSystemEntries(Source, "*", SearchOption.AllDirectories);
                                    foreach (string Entry in Entries)
                                    {
                                        Console.WriteLine($"Locating {Entry}");
                                        if (Directory.Exists(Entry))
                                        {
                                            Console.WriteLine($"Deleting {Entry}");
                                            Directory.Delete(Entry, true);

                                            Console.WriteLine("Verifying.");
                                            if (!Directory.Exists(Entry)) Console.WriteLine($"Deleted {Entry}");
                                        }

                                        else if (File.Exists(Entry))
                                        {
                                            Console.WriteLine($"Deleting {Entry}");
                                            File.Delete(Entry);

                                            Console.WriteLine("Verifying.");
                                            if (!File.Exists(Entry)) Console.WriteLine($"Deleted {Entry}");
                                        }
                                    }
                                }

                                Console.WriteLine();
                                Shell.SYSRestore();
                                Console.WriteLine("System Reset Completed.");
                                Console.WriteLine("Restarting in 3 seconds.");

                                Thread.Sleep(3);

                                Shell.CommandPrompt($"\"{Process.GetCurrentProcess().MainModule.FileName}\"");
                                Environment.Exit(0);
                            }

                            catch (Exception)
                            {
                                Console.WriteLine("Cannot perform a System Reset.");
                            }
                        }

                        else if (GetKey.ToLower() == "n") Console.WriteLine("System Reset Cancelled!");
                        else
                        {
                            Console.WriteLine("Invalid Key Input.");
                            Console.WriteLine("Cannot perform a System Reset.");
                        }
                    }

                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "read")
                {
                    if (Args.Length == 0 || Args == null) Error.NoArgs();
                    else if (Args.Length > 1) Error.TooManyArgs(Args);
                    else if (Args.Length < 1) Error.TooFewArgs(Args);
                    else
                    {
                        Args[0] = Shell.Strings(Args[0]);
                        if (File.Exists(Args[0])) Console.WriteLine(File.ReadAllText(Args[0]));
                        else Console.WriteLine("No file found.");
                    }
                }

                else if (Command.ToLower() == "about" || Command.ToLower() == "info")
                    Console.WriteLine("AOs is a Command line utility designed to improve your Efficiency and Productivity.");

                else if (Command.ToLower() == "shutdown" || Command.ToLower() == "quit" || Command.ToLower() == "exit")
                {
                    if (Args.Length == 0 || Args == null) Environment.Exit(0);
                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "restart" || Command.ToLower() == "reload" || Command.ToLower() == "refresh")
                {
                    if (Args.Length == 0 || Args == null)
                    {
                        string AOsBinaryFile = Process.GetCurrentProcess().MainModule.FileName;
                        Shell.CommandPrompt($"\"{AOsBinaryFile}\"");
                        Environment.Exit(0);
                    }

                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "version" || Command.ToLower() == "ver" || Command.ToLower() == "-v")
                {
                    if (Args.Length == 0 || Args == null) Console.WriteLine(SAFE.Version);
                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "credits")
                {
                    if (Args.Length == 0 || Args == null) SAFE.Credits();
                    else Error.Args(Args);
                }

                else if (Command == "AOs1000")
                {
                    if (Args.Length == 0 || Args == null)
                    {
                        string[] AOs1000 = {
                            "AOs1000!",
                            "Congratulations for hitting 1000 Lines Of Code in AOs!",
                            "It was the first program to ever reach these many Lines Of Code!"
                        };

                        Console.WriteLine(string.Join("\n", AOs1000));
                    }

                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "history")
                {
                    if (Args.Length == 0 || Args == null) Terminal.History.GetHistory();
                    else if (Args.FirstOrDefault() == "-c" && Args.Length == 1) Terminal.History.ClearHistory();
                    else Error.Args(Args);
                }

                else if (Command == ">")
                {
                    if (Args.Length == 0 || Args == null) Shell.StartApp("cmd");
                    else Shell.CommandPrompt(string.Join(" ", Args));
                }

                else if (Command.ToLower() == "calendar" || Command.ToLower() == "time" || Command.ToLower() == "date" || Command.ToLower() == "clock")
                {
                    if (Args.Length == 0 || Args == null)
                    {
                        Console.WriteLine(DateTime.Now.ToString("h:mm:ss tt"));
                        Console.WriteLine(DateTime.Now.ToString("dddd, dd MMMM yyyy"));
                    }

                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "console" || Command.ToLower() == "terminal" || Command.ToLower() == "cmd")
                {
                    if (Args.Length == 0 || Args == null) Shell.StartApp("cmd");
                    else Shell.CommandPrompt(string.Join(" ", Args));
                }

                else if (Command.ToLower() == "pause")
                {
                    if (Args.Length == 0 || Args == null) Console.Write("Press any Key to Continue.");
                    else
                    {
                        string NewString = "";
                        foreach (string i in Args)
                        {
                            if (Collections.String.IsString(i)) NewString += Shell.Strings(i);
                            else NewString += i;
                        }

                        Console.Write(NewString);
                    }

                    Console.ReadKey();
                    Console.WriteLine();
                }

                else if (Command.ToLower() == "run" || Command.ToLower() == "start" || Command.ToLower() == "call")
                {
                    if (Args.Length == 0 || Args == null) Shell.StartApp("cmd");
                    else Shell.StartApp(string.Join(" ", Args));
                }

                else if (Command.ToLower() == "cd")
                {
                    if (Args.Length == 0 || Args == null) Console.WriteLine(Directory.GetCurrentDirectory());
                    else Directory.SetCurrentDirectory(Shell.Strings(string.Join(" ", Args)));
                }

                else if (Command.ToLower() == "cd.")
                {
                    if (Args.Length == 0 || Args == null) Console.WriteLine(Directory.GetCurrentDirectory());
                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "cd..")
                {
                    if (Args.Length == 0 || Args == null) Directory.SetCurrentDirectory("..");
                    else Error.Args(Args);
                }

                else if (Command.ToLower() == "ls" || Command.ToLower() == "dir")
                {
                    if (Args.Length == 0 || Args == null)
                    {
                        Console.WriteLine(Directory.GetCurrentDirectory());
                        string[] Entries = Directory.GetFileSystemEntries(".", "*");
                        foreach (string Entry in Entries) Console.WriteLine(Entry);
                    }

                    else
                    {
                        Console.WriteLine(Directory.GetCurrentDirectory());
                        foreach (string i in Args)
                        {
                            if (i.ToLower() == "-h" || i.ToLower() == "/?" || i.ToLower() == "--help")
                            {
                                string[] LSHelpCenter = {
                                    "Displays a list of files and subdirectories in a directory.",
                                    "Usage: ls [Option]",
                                    "",
                                    "Options:",
                                    "-f   -> Display only files.",
                                    "-d   -> Display only folders.",
                                    "-a   -> Display all files and folders.",
                                };

                                Console.WriteLine(string.Join("\n", LSHelpCenter));
                                break;
                            }

                            else if (i.ToLower() == "-a" || i.ToLower() == "--all")
                            {
                                string[] Entries = Directory.GetFileSystemEntries(".", "*");
                                foreach (string Entry in Entries) Console.WriteLine(Entry);
                                break;
                            }

                            else if (i.ToLower() == "-f" || i.ToLower() == "--files")
                            {
                                string[] Files = Directory.GetFiles(".");
                                foreach (string File in Files) Console.WriteLine(File);
                            }

                            else if (i.ToLower() == "-d" || i.ToLower() == "--folder" || i.ToLower() == "--directories")
                            {
                                string[] Directories = Directory.GetDirectories(".");
                                foreach (string Folder in Directories) Console.WriteLine(Folder);
                            }

                            else
                            {
                                Error.Args(i);
                                break;
                            }
                        }
                    }
                }

                else Error.Command(Command);
            }
        }

        static void BIOS()
        {
            Console.Clear();
            Console.Title = "Entrypoint";

            // Important process before booting.
            Shell.RootPackages();
            Shell.Scan();
            Shell.AskPass();
        }

        static void BootLoader()
        {
            // Run apps on startup.
            Shell.StartUpApps();

            // Log current time in boot file.
            string NoteCurrentTime = DateTime.Now.ToString("[dd-MM-yyyy], [HH:mm:ss]");
            File.AppendAllText($"{Shell.Root()}\\Files.x72\\Temp\\log\\BOOT.log", $"{NoteCurrentTime}\n");
        }
    }
}
