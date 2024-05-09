struct ReadLineConfig(int LeftCursorStartPos, int TopCursorStartPos, Dictionary<ReadLine.Tokenizer.TokenType, ConsoleColor> SyntaxHighlightCodes, bool Toggle_autocomplete = true, bool Toggle_color_coding = true, List<string> Suggestions = null)
{
    public int LeftCursorStartPos { get; set; } = LeftCursorStartPos;
    public int TopCursorStartPos { get; set; } = TopCursorStartPos;
    public bool Toggle_autocomplete { get; set; } = Toggle_autocomplete;
    public bool Toggle_color_coding { get; set; } = Toggle_color_coding;
    public List<string> Suggestions { get; set; } = Suggestions ?? ([]);
    public Dictionary<ReadLine.Tokenizer.TokenType, ConsoleColor> SyntaxHighlightCodes { get; set; } = SyntaxHighlightCodes;
}

partial class ReadLine(ReadLineConfig Config)
{
    public bool Loop = true;
    public Dictionary<(ConsoleKey, ConsoleModifiers), Action> KeyBindings = [];

    private string TextBuffer = "";
    private int LeftCursorPos = Config.LeftCursorStartPos;
    private int TopCursorPos = Config.TopCursorStartPos;
    private int CurrentSuggestionIdx = 0;

    private readonly ReadLineConfig Config = Config;

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
                TextBuffer = TextBuffer.Insert(LeftCursorPos - Config.LeftCursorStartPos, KeyInfo.KeyChar.ToString());

                // Set current suggestion index to 0
                CurrentSuggestionIdx = 0;

                // Update the text buffer
                UpdateBuffer();
                LeftCursorPos++;
            }

            // Set the cursor pos to where it should be
            if (Loop)
                Console.SetCursorPosition(LeftCursorPos, TopCursorPos);
        }

        return TextBuffer;
    }
}
