partial class Terminal
{
    private partial class ReadLine
    {
        private int SuggestionIdx = 0;
        private string Text = "";
        private bool Loop = true;
        private int LeftCursorPos;
        private int TopCursorPos;

        private readonly int LeftCursorStartPos;
        private readonly Dictionary<(ConsoleKey, ConsoleModifiers), Action> KeyBindings = [];
        private readonly Dictionary<Lexer.Tokenizer.TokenType, ConsoleColor> SyntaxHighlightCodes = [];

        public ReadLine()
        {
            LeftCursorStartPos = Console.CursorLeft;
            LeftCursorPos = LeftCursorStartPos;
            TopCursorPos = Console.CursorTop;

            /*
            -------------------------------------------------
            ------------------ Keybindings ------------------
            -------------------------------------------------
            */

            KeyBindings[(ConsoleKey.Enter, ConsoleModifiers.None)] = HandleEnter;
            KeyBindings[(ConsoleKey.Enter, ConsoleModifiers.Control)] = HandleCtrlEnter;

            KeyBindings[(ConsoleKey.Tab, ConsoleModifiers.None)] = HandleTab;
            KeyBindings[(ConsoleKey.Spacebar, ConsoleModifiers.Control)] = HandleCtrlSpacebar;

            KeyBindings[(ConsoleKey.Escape, ConsoleModifiers.None)] = HandleEscape;
            KeyBindings[(ConsoleKey.Escape, ConsoleModifiers.Shift)] = HandleShiftEscape;

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

            /*
            ------------------------------------------------------------
            ------------------ Syntax highlight codes ------------------
            ------------------------------------------------------------
            */

            SyntaxHighlightCodes[Lexer.Tokenizer.TokenType.STRING] = ConsoleColor.Yellow;
            SyntaxHighlightCodes[Lexer.Tokenizer.TokenType.EXPR] = ConsoleColor.Cyan;
            SyntaxHighlightCodes[Lexer.Tokenizer.TokenType.SYMBOL] = ConsoleColor.White;
            SyntaxHighlightCodes[Lexer.Tokenizer.TokenType.COMMENT] = ConsoleColor.DarkGray;
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
                    Text = Text.Insert(LeftCursorPos - LeftCursorStartPos, KeyInfo.KeyChar.ToString());

                    // Set the SuggestionIdx to 0
                    SuggestionIdx = 0;

                    // Update the text buffer
                    UpdateBuffer();
                    LeftCursorPos++;
                }

                // Set the cursor pos to where it should be
                if (Loop) Console.SetCursorPosition(LeftCursorPos, TopCursorPos);
            }

            return Text;
        }
    }
}
