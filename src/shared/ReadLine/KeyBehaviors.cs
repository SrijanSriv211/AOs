partial class ReadLine
{
    private void HandleEnter()
    {
        if (Config.Toggle_autocomplete)
            ClearSuggestionBuffer();

        Console.WriteLine();
        Loop = false;
    }

    // Set the current suggesion in the text.
    private void HandleCtrlEnter()
    {
        TextBuffer += Suggestion;
        LeftCursorPos += Suggestion.Length;
        CurrentSuggestionIdx = 0;
        UpdateBuffer(false);
    }

    // Render the next suggestion
    private void HandleTab()
    {
        if (Utils.Array.IsEmpty([.. Suggestions]))
            return;

        CurrentSuggestionIdx = (CurrentSuggestionIdx + 1) % Suggestions.Count;
        if (CurrentSuggestionIdx < 0 || CurrentSuggestionIdx > Suggestions.Count)
            CurrentSuggestionIdx = 0;

        UpdateBuffer();
    }

    // Render the suggestions without typing anything
    private void HandleCtrlSpacebar()
    {
        UpdateBuffer();
    }

    // Clear all the suggestions
    private void HandleEscape()
    {
        if (!Config.Toggle_autocomplete)
            return;

        CurrentSuggestionIdx = 0;
        ClearSuggestionBuffer();
    }

    // Clear all the text
    private void HandleShiftEscape()
    {
        TextBuffer = "";
        LeftCursorPos = Config.LeftCursorStartPos;
        UpdateBuffer(false);
    }

    private void HandleBackspace()
    {
        if (LeftCursorPos > Config.LeftCursorStartPos)
        {
            LeftCursorPos--;
            TextBuffer = TextBuffer.Remove(LeftCursorPos - Config.LeftCursorStartPos, 1);
            UpdateBuffer();
        }
    }

    private void HandleCtrlBackspace()
    {
        if (LeftCursorPos > Config.LeftCursorStartPos)
        {
            if (TextBuffer.LastIndexOf(' ', LeftCursorPos - Config.LeftCursorStartPos - 1) == LeftCursorPos - Config.LeftCursorStartPos - 1)
                HandleBackspace();

            int PreviousWordIdx = TextBuffer.LastIndexOf(' ', LeftCursorPos - Config.LeftCursorStartPos - 1);
            int length = LeftCursorPos - Config.LeftCursorStartPos - PreviousWordIdx - 1;

            LeftCursorPos -= length;
            TextBuffer = TextBuffer.Remove(LeftCursorPos - Config.LeftCursorStartPos, length);
            UpdateBuffer();
        }
    }

    private void HandleDelete()
    {
        if (LeftCursorPos - Config.LeftCursorStartPos < TextBuffer.Length)
        {
            TextBuffer = TextBuffer.Remove(LeftCursorPos - Config.LeftCursorStartPos, 1);
            UpdateBuffer();
        }
    }

    private void HandleCtrlDelete()
    {
        if (LeftCursorPos - Config.LeftCursorStartPos < TextBuffer.Length)
        {
            if (TextBuffer.IndexOf(' ', LeftCursorPos - Config.LeftCursorStartPos) == LeftCursorPos - Config.LeftCursorStartPos)
                HandleDelete();

            int NextWordIdx = TextBuffer.IndexOf(' ', LeftCursorPos - Config.LeftCursorStartPos);
            int length = NextWordIdx == -1 ? TextBuffer.Length - (LeftCursorPos - Config.LeftCursorStartPos) : NextWordIdx - (LeftCursorPos - Config.LeftCursorStartPos);

            TextBuffer = TextBuffer.Remove(LeftCursorPos - Config.LeftCursorStartPos, length);
            UpdateBuffer();
        }
    }

    private void HandleHome()
    {
        LeftCursorPos = Config.LeftCursorStartPos;
    }

    private void HandleEnd()
    {
        LeftCursorPos = TextBuffer.Length;
    }

    private void HandleLeftArrow()
    {
        if (LeftCursorPos > Config.LeftCursorStartPos)
            LeftCursorPos--;
    }

    private void HandleCtrlLeftArrow()
    {
        if (LeftCursorPos > Config.LeftCursorStartPos)
        {
            if (TextBuffer.LastIndexOf(' ', LeftCursorPos - Config.LeftCursorStartPos - 1) == LeftCursorPos - Config.LeftCursorStartPos - 1)
                LeftCursorPos--;

            int PreviousWordIdx = TextBuffer.LastIndexOf(' ', LeftCursorPos - Config.LeftCursorStartPos - 1);
            int length = LeftCursorPos - Config.LeftCursorStartPos - PreviousWordIdx - 1;

            LeftCursorPos -= length;
        }
    }

    private void HandleRightArrow()
    {
        if (LeftCursorPos - Config.LeftCursorStartPos < TextBuffer.Length)
            LeftCursorPos++;
    }

    private void HandleCtrlRightArrow()
    {
        if (LeftCursorPos - Config.LeftCursorStartPos < TextBuffer.Length)
        {
            if (TextBuffer.IndexOf(' ', LeftCursorPos - Config.LeftCursorStartPos) == LeftCursorPos - Config.LeftCursorStartPos)
                LeftCursorPos++;

            int NextWordIdx = TextBuffer.IndexOf(' ', LeftCursorPos - Config.LeftCursorStartPos);
            int length = NextWordIdx == -1 ? TextBuffer.Length - (LeftCursorPos - Config.LeftCursorStartPos) : NextWordIdx - (LeftCursorPos - Config.LeftCursorStartPos);

            LeftCursorPos += length;
        }
    }
}
