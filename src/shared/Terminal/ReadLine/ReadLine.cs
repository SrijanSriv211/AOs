partial class Terminal
{
    private partial class ReadLine
    {
        private string Text = "";
        private bool Loop = true;
        private int CursorPos;

        private readonly Lexer.Tokenizer tokenizer;
        private readonly int CursorStartPos;
        private readonly Dictionary<(ConsoleKey, ConsoleModifiers), Action> KeyBindings = [];
        private readonly Dictionary<Lexer.Tokenizer.TokenType, ConsoleColor> SyntaxHighlightCodes = [];

        public ReadLine()
        {
            CursorStartPos = Console.CursorLeft;
            CursorPos = CursorStartPos;
            tokenizer = new("") { disable_error = true };

            InitKeyBindings();
            InitSyntaxHighlightCodes();
        }

        public string Readf()
        {
            while (Loop)
            {
                ConsoleKeyInfo KeyInfo = Console.ReadKey(true);
                (ConsoleKey, ConsoleModifiers) Key = (KeyInfo.Key, KeyInfo.Modifiers);

                if (KeyBindings.TryGetValue(Key, out Action func))
                    func();

                else
                {
                    // Ignore control characters other than the handled keybindings
                    if (char.IsControl(KeyInfo.KeyChar))
                        continue;

                    // Insert the character at the cursor position
                    Text = Text.Insert(CursorPos - CursorStartPos, KeyInfo.KeyChar.ToString());
                    CursorPos++;
                }

                if (Loop)
                {
                    // Tokenize the updated input text
                    tokenizer.line = Text;
                    tokenizer.Tokenize();

                    // Clear current text buffer and re-render the updated input
                    // Console.SetCursorPosition(CursorStartPos, Console.CursorTop);
                    // Console.Write(new string(' ', Text.Length));
                    Console.SetCursorPosition(CursorStartPos, Console.CursorTop);

                    // Loop through each token and check if the token is to be highlighted or not.
                    // If yes, highlight, otherwise update text after cursor normally.
                    foreach (Lexer.Tokenizer.Token token in tokenizer.tokens)
                    {
                        if (SyntaxHighlightCodes.TryGetValue(token.Type, out ConsoleColor color))
                            Print(
                                token.Type == Lexer.Tokenizer.TokenType.STRING ? $"\"{token.Name}\"" : token.Name,
                                color, false
                            );

                        else if (token.Type != Lexer.Tokenizer.TokenType.EOL)
                            Console.Write(token.Name);
                    }

                    Console.SetCursorPosition(CursorPos, Console.CursorTop);
                }
            }

            return Text;
        }

        private void InitKeyBindings()
        {
            KeyBindings[(ConsoleKey.Enter, ConsoleModifiers.None)] = HandleEnter;

            KeyBindings[(ConsoleKey.Home, ConsoleModifiers.None)] = HandleHome;
            KeyBindings[(ConsoleKey.End, ConsoleModifiers.None)] = HandleEnd;

            KeyBindings[(ConsoleKey.Delete, ConsoleModifiers.None)] = HandleDelete;
            KeyBindings[(ConsoleKey.Delete, ConsoleModifiers.Control)] = HandleCtrlDelete;

            KeyBindings[(ConsoleKey.Backspace, ConsoleModifiers.None)] = HandleBackspace;
            KeyBindings[(ConsoleKey.Backspace, ConsoleModifiers.Control)] = HandleCtrlBackspace;

            KeyBindings[(ConsoleKey.LeftArrow, ConsoleModifiers.None)] = HandleLeftArrow;
            KeyBindings[(ConsoleKey.LeftArrow, ConsoleModifiers.Control)] = HandleCtrlLeftArrow;

            KeyBindings[(ConsoleKey.RightArrow, ConsoleModifiers.None)] = HandleRightArrow;
            KeyBindings[(ConsoleKey.RightArrow, ConsoleModifiers.Control)] = HandleCtrlRightArrow;
        }

        private void InitSyntaxHighlightCodes()
        {
            SyntaxHighlightCodes[Lexer.Tokenizer.TokenType.STRING] = ConsoleColor.Yellow;
            SyntaxHighlightCodes[Lexer.Tokenizer.TokenType.EXPR] = ConsoleColor.Cyan;
            SyntaxHighlightCodes[Lexer.Tokenizer.TokenType.SYMBOL] = ConsoleColor.Blue;
        }
    }
}
