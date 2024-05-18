partial class ReadLine
{
    private void HandleEnter()
    {
        // if (Config.Toggle_autocomplete)
        //     ClearSuggestionBuffer();

        int TotalDist = Config.LeftCursorStartPos + TextBuffer.Length;
        int y = TotalDist / Console.WindowWidth;

        CursorVec3.Y++;
        y += CursorVec3.Y;

        // Move the cursor to the end of the text then write a new line to output the text.
        Console.WriteLine();
        Console.SetCursorPosition(0, y);
        Loop = false;
    }

    // Set the current suggesion in the text.
    private void HandleCtrlEnter()
    {
        // TextBuffer += Suggestion;
        // LeftCursorPos += Suggestion.Length;
        // CurrentSuggestionIdx = 0;
        // UpdateBuffer(false);
    }

    // Render the next suggestion
    private void HandleTab()
    {
        // if (Utils.Array.IsEmpty([.. Suggestions]))
        //     return;

        // CurrentSuggestionIdx = (CurrentSuggestionIdx + 1) % Suggestions.Count;
        // if (CurrentSuggestionIdx < 0 || CurrentSuggestionIdx > Suggestions.Count)
        //     CurrentSuggestionIdx = 0;

        // UpdateBuffer();
    }

    // Render the suggestions without typing anything
    private void HandleCtrlSpacebar()
    {
        // UpdateBuffer();
    }

    // Clear all the suggestions
    private void HandleEscape()
    {
        // if (!Config.Toggle_autocomplete)
        //     return;

        // CurrentSuggestionIdx = 0;
        // ClearSuggestionBuffer();
    }

    // Clear all the text
    private void HandleShiftEscape()
    {
        TextBuffer = "";
        CursorVec3.X = Config.LeftCursorStartPos;
        CursorVec3.Y = Config.TopCursorStartPos;
        CursorVec3.I = 0;

        UpdateBuffer(false);
    }

    private void HandleBackspace()
    {
        if (CursorVec3.I > 0)
        {
            CursorVec3.I--;
            CursorVec3.X--;
            TextBuffer = TextBuffer.Remove(CursorVec3.I, 1);

            if (CursorVec3.X < 0)
            {
                CursorVec3.X--;
                CursorVec3.X += Console.WindowWidth;
                CursorVec3.Y--;
            }

            UpdateBuffer();
        }
    }

    private void HandleCtrlBackspace()
    {
        if (CursorVec3.I > 0)
        {
            if (TextBuffer.LastIndexOf(' ', CursorVec3.X - Config.LeftCursorStartPos - 1) == CursorVec3.X - Config.LeftCursorStartPos - 1)
                HandleBackspace();

            int PreviousWordIdx = TextBuffer.LastIndexOf(' ', CursorVec3.X - Config.LeftCursorStartPos - 1);
            int length = CursorVec3.X - Config.LeftCursorStartPos - PreviousWordIdx - 1;

            CursorVec3.X -= length;
            CursorVec3.I -= length;

            TextBuffer = TextBuffer.Remove(CursorVec3.X - Config.LeftCursorStartPos, length);
            UpdateBuffer();
        }
    }

    private void HandleDelete()
    {
        if (CursorVec3.I < TextBuffer.Length)
        {
            TextBuffer = TextBuffer.Remove(CursorVec3.X - Config.LeftCursorStartPos, 1);
            UpdateBuffer();
        }
    }

    private void HandleCtrlDelete()
    {
        if (CursorVec3.I < TextBuffer.Length)
        {
            if (TextBuffer.IndexOf(' ', CursorVec3.X - Config.LeftCursorStartPos) == CursorVec3.X - Config.LeftCursorStartPos)
                HandleDelete();

            int NextWordIdx = TextBuffer.IndexOf(' ', CursorVec3.X - Config.LeftCursorStartPos);
            int length = NextWordIdx == -1 ? TextBuffer.Length - (CursorVec3.X - Config.LeftCursorStartPos) : NextWordIdx - (CursorVec3.X - Config.LeftCursorStartPos);

            TextBuffer = TextBuffer.Remove(CursorVec3.X - Config.LeftCursorStartPos, length);
            UpdateBuffer();
        }
    }

    private void HandleHome()
    {
        CursorVec3.X = Config.LeftCursorStartPos;
        CursorVec3.Y = Config.TopCursorStartPos;
        CursorVec3.I = 0;
    }

    private void HandleEnd()
    {
        // CursorVec3.X = Config.LeftCursorStartPos + TextBuffer.Length;
        // CursorVec3.I = TextBuffer.Length;
    }

    private void HandleLeftArrow()
    {
        if (CursorVec3.I > 0)
        {
            CursorVec3.I--;
            CursorVec3.X--;

            if (CursorVec3.X < 0)
            {
                CursorVec3.X--;
                CursorVec3.X += Console.WindowWidth;
                CursorVec3.Y--;
                Console.SetCursorPosition(CursorVec3.X, CursorVec3.Y);
            }
        }
    }

    private void HandleCtrlLeftArrow()
    {
        if (CursorVec3.I > 0)
        {
            if (TextBuffer.LastIndexOf(' ', CursorVec3.I - 1) == CursorVec3.I - 1)
            {
                CursorVec3.X--;
                CursorVec3.I--;
            }

            int PreviousWordIdx = TextBuffer.LastIndexOf(' ', CursorVec3.I - 1);
            int length = CursorVec3.I - PreviousWordIdx - 1;

            CursorVec3.X -= length;
            CursorVec3.I -= length;

            if (CursorVec3.X < 0)
            {
                CursorVec3.X--;
                CursorVec3.X += Console.WindowWidth;
                CursorVec3.Y--;
                Console.SetCursorPosition(CursorVec3.X, CursorVec3.Y);
            }
        }
    }

    private void HandleRightArrow()
    {
        if (CursorVec3.I < TextBuffer.Length)
        {
            CursorVec3.I++;
            CursorVec3.X++;

            if (CursorVec3.X >= Console.WindowWidth)
            {
                CursorVec3.X = 1;
                CursorVec3.Y++;
                Console.SetCursorPosition(CursorVec3.X, CursorVec3.Y);
            }
        }
    }

    private void HandleCtrlRightArrow()
    {
        if (CursorVec3.I < TextBuffer.Length)
        {
            if (TextBuffer.IndexOf(' ', CursorVec3.X - Config.LeftCursorStartPos) == CursorVec3.X - Config.LeftCursorStartPos)
            {
                CursorVec3.X++;
                CursorVec3.I++;
            }

            int NextWordIdx = TextBuffer.IndexOf(' ', CursorVec3.X - Config.LeftCursorStartPos);
            int length = NextWordIdx == -1 ? TextBuffer.Length - (CursorVec3.X - Config.LeftCursorStartPos) : NextWordIdx - (CursorVec3.X - Config.LeftCursorStartPos);

            CursorVec3.X += length;
            CursorVec3.I += length;
        }
    }
}
