struct ReadLineConfig(int LeftCursorStartPos, int TopCursorStartPos, Dictionary<ReadLine.Tokenizer.TokenType, ConsoleColor> SyntaxHighlightCodes, bool Toggle_autocomplete = true, bool Toggle_color_coding = true, List<string> Suggestions = null)
{
    public int LeftCursorStartPos { get; set; } = LeftCursorStartPos;
    public int TopCursorStartPos { get; set; } = TopCursorStartPos;
    public bool Toggle_autocomplete { get; set; } = Toggle_autocomplete;
    public bool Toggle_color_coding { get; set; } = Toggle_color_coding;
    public List<string> Suggestions { get; set; } = Suggestions ?? ([]);
    public Dictionary<ReadLine.Tokenizer.TokenType, ConsoleColor> SyntaxHighlightCodes { get; set; } = SyntaxHighlightCodes;
}

partial class ReadLine
{
    public bool Loop = true;
    public Dictionary<(ConsoleKey, ConsoleModifiers), Action> KeyBindings = [];

    private string TextBuffer = "";
    private readonly ReadLineConfig Config;

    private class CursorVec3
    {
        public static int X { get; set; } = 0; // Cursor left
        public static int Y { get; set; } = 0; // Cursor top
        public static int I { get; set; } = 0; // Cursor pos on text buffer

        public static void Reset()
        {
            X = 0; Y = 0; I = 0;
        }
    }

    public ReadLine(ReadLineConfig Config)
    {
        this.Config = Config;
        CursorVec3.X = this.Config.LeftCursorStartPos;
        CursorVec3.Y = this.Config.TopCursorStartPos;
    }

    public string Readf()
    {
        while (Loop)
        {
            ConsoleKeyInfo KeyInfo = Console.ReadKey(true);
            (ConsoleKey, ConsoleModifiers) Key = (KeyInfo.Key, KeyInfo.Modifiers);

            if (KeyBindings.TryGetValue(Key, out Action func))
                func();

            // Ignore control characters other than the handled keybindings
            else if (char.IsControl(KeyInfo.KeyChar))
                continue;

            else
            {
                // Insert the character at the cursor position
                TextBuffer = TextBuffer.Insert(CursorVec3.I, KeyInfo.KeyChar.ToString());

                // Update the positions
                CursorVec3.I++;
                CursorVec3.X++;

                // Update the text buffer
                UpdateBuffer();
            }

            // Set the cursor pos to where it should be
            Console.SetCursorPosition(CursorVec3.X, CursorVec3.Y);
        }

        CursorVec3.Reset();
        return TextBuffer;
    }

    private void UpdateBuffer(bool RenderSuggestions=true)
    {
        ClearTextBuffer();
        RenderTextBuffer();

        if (!RenderSuggestions)
            return;

        ClearSuggestionBuffer();
        RenderSuggestionBuffer();
    }

    public void AddKeyBindings(ConsoleKey key, ConsoleModifiers modifier, Action action)
    {
        KeyBindings.Add((key, modifier), action);
    }

    public void InitDefaultKeyBindings()
    {
        AddKeyBindings(ConsoleKey.Enter, ConsoleModifiers.None, HandleEnter);
        AddKeyBindings(ConsoleKey.Enter, ConsoleModifiers.Control, HandleCtrlEnter);

        AddKeyBindings(ConsoleKey.Tab, ConsoleModifiers.None, HandleTab);
        AddKeyBindings(ConsoleKey.Spacebar, ConsoleModifiers.Control, HandleCtrlSpacebar);

        AddKeyBindings(ConsoleKey.Escape, ConsoleModifiers.None, HandleEscape);
        AddKeyBindings(ConsoleKey.Escape, ConsoleModifiers.Shift, HandleShiftEscape);

        AddKeyBindings(ConsoleKey.Home, ConsoleModifiers.None, HandleHome);
        AddKeyBindings(ConsoleKey.End, ConsoleModifiers.None, HandleEnd);

        AddKeyBindings(ConsoleKey.Delete, ConsoleModifiers.None, HandleDelete);
        AddKeyBindings(ConsoleKey.Delete, ConsoleModifiers.Control, HandleCtrlDelete);

        AddKeyBindings(ConsoleKey.Backspace, ConsoleModifiers.None, HandleBackspace);
        AddKeyBindings(ConsoleKey.Backspace, ConsoleModifiers.Control, HandleCtrlBackspace);

        AddKeyBindings(ConsoleKey.LeftArrow, ConsoleModifiers.None, HandleLeftArrow);
        AddKeyBindings(ConsoleKey.LeftArrow, ConsoleModifiers.Control, HandleCtrlLeftArrow);

        AddKeyBindings(ConsoleKey.RightArrow, ConsoleModifiers.None, HandleRightArrow);
        AddKeyBindings(ConsoleKey.RightArrow, ConsoleModifiers.Control, HandleCtrlRightArrow);
    }
}
