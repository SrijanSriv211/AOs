partial class Terminal
{
    private partial class ReadLine
    {
        private string Text = "";
        private bool Loop = true;
        private int CursorPos;

        private readonly int CursorStartPos;
        private readonly Dictionary<(ConsoleKey, ConsoleModifiers), Action> KeyBindings = [];

        public ReadLine()
        {
            CursorStartPos = Console.CursorLeft;
            CursorPos = CursorStartPos;

            KeyBindings[(ConsoleKey.Enter, ConsoleModifiers.None)] = HandleEnter;
            KeyBindings[(ConsoleKey.Backspace, ConsoleModifiers.None)] = HandleBackspace;
            KeyBindings[(ConsoleKey.Backspace, ConsoleModifiers.Control)] = HandleCtrlBackspace;
            KeyBindings[(ConsoleKey.LeftArrow, ConsoleModifiers.Control)] = HandleCtrlLeftArrow;
            KeyBindings[(ConsoleKey.LeftArrow, ConsoleModifiers.None)] = HandleLeftArrow;
            KeyBindings[(ConsoleKey.RightArrow, ConsoleModifiers.None)] = HandleRightArrow;
        }

        public string Readf()
        {
            while (Loop)
            {
                ConsoleKeyInfo KeyInfo = Console.ReadKey(true);
                (ConsoleKey, ConsoleModifiers) Key = (KeyInfo.Key, KeyInfo.Modifiers);

                if (KeyBindings.TryGetValue(Key, out Action value))
                    value();

                else
                {
                    // Ignore control characters other than the handled keybindings
                    if (char.IsControl(KeyInfo.KeyChar))
                        continue;

                    // Insert the character at the cursor position
                    else
                    {
                        Text = Text.Insert(CursorPos - CursorStartPos, KeyInfo.KeyChar.ToString());
                        Console.Write(Text[(CursorPos - CursorStartPos)..]); // Update text after cursor
                    }

                    CursorPos++;
                }

                if (Loop)
                    Console.SetCursorPosition(CursorPos, Console.CursorTop);
            }

            return Text;
        }
    }
}
