partial class Terminal
{
    private partial class ReadLine
    {
        private void HandleEnter()
        {
            Console.WriteLine();
            Loop = false;
        }

        // Set the current suggesion in the text.
        private void HandleCtrlEnter()
        {
            Text += Suggestion;
            CursorPos += Suggestion.Length;
            SuggestionIdx = 0;
            UpdateTextBuffer(false);
        }

        // Render the next suggestion
        private void HandleTab()
        {
            if (Utils.Array.IsEmpty([.. Suggestions]))
                return;

            SuggestionIdx = (SuggestionIdx + 1) % Suggestions.Count;
            if (SuggestionIdx < 0 || SuggestionIdx > Suggestions.Count)
                SuggestionIdx = 0;

            UpdateTextBuffer();
        }

        // Render the suggestions without typing anything
        private void HandleCtrlSpacebar()
        {
            UpdateTextBuffer();
        }

        // Clear all the suggestions
        private void HandleEscape()
        {
            UpdateTextBuffer(false);
        }

        // Clear all the text
        private void HandleShiftEscape()
        {
            Text = "";
            CursorPos = CursorStartPos;
            UpdateTextBuffer(false);
        }

        private void HandleBackspace()
        {
            if (CursorPos > CursorStartPos)
            {
                CursorPos--;
                Text = Text.Remove(CursorPos - CursorStartPos, 1);
                UpdateTextBuffer();
            }
        }

        private void HandleCtrlBackspace()
        {
            if (CursorPos > CursorStartPos)
            {
                if (Text.LastIndexOf(' ', CursorPos - CursorStartPos - 1) == CursorPos - CursorStartPos - 1)
                    HandleBackspace();

                int PreviousWordIdx = Text.LastIndexOf(' ', CursorPos - CursorStartPos - 1);
                int length = CursorPos - CursorStartPos - PreviousWordIdx - 1;

                CursorPos -= length;
                Text = Text.Remove(CursorPos - CursorStartPos, length);
                UpdateTextBuffer();
            }
        }

        private void HandleDelete()
        {
            if (CursorPos - CursorStartPos < Text.Length)
            {
                Text = Text.Remove(CursorPos - CursorStartPos, 1);
                UpdateTextBuffer();
            }
        }

        private void HandleCtrlDelete()
        {
            if (CursorPos - CursorStartPos < Text.Length)
            {
                if (Text.IndexOf(' ', CursorPos - CursorStartPos) == CursorPos - CursorStartPos)
                    HandleDelete();

                int NextWordIdx = Text.IndexOf(' ', CursorPos - CursorStartPos);
                int length = NextWordIdx == -1 ? Text.Length - (CursorPos - CursorStartPos) : NextWordIdx - (CursorPos - CursorStartPos);

                Text = Text.Remove(CursorPos - CursorStartPos, length);
                UpdateTextBuffer();
            }
        }

        private void HandleHome()
        {
            CursorPos = CursorStartPos;
        }

        private void HandleEnd()
        {
            CursorPos = Text.Length;
        }

        private void HandleLeftArrow()
        {
            if (CursorPos > CursorStartPos)
                CursorPos--;
        }

        private void HandleCtrlLeftArrow()
        {
            if (CursorPos > CursorStartPos)
            {
                if (Text.LastIndexOf(' ', CursorPos - CursorStartPos - 1) == CursorPos - CursorStartPos - 1)
                    CursorPos--;

                int PreviousWordIdx = Text.LastIndexOf(' ', CursorPos - CursorStartPos - 1);
                int length = CursorPos - CursorStartPos - PreviousWordIdx - 1;

                CursorPos -= length;
            }
        }

        private void HandleRightArrow()
        {
            if (CursorPos - CursorStartPos < Text.Length)
                CursorPos++;
        }

        private void HandleCtrlRightArrow()
        {
            if (CursorPos - CursorStartPos < Text.Length)
            {
                if (Text.IndexOf(' ', CursorPos - CursorStartPos) == CursorPos - CursorStartPos)
                    CursorPos++;

                int NextWordIdx = Text.IndexOf(' ', CursorPos - CursorStartPos);
                int length = NextWordIdx == -1 ? Text.Length - (CursorPos - CursorStartPos) : NextWordIdx - (CursorPos - CursorStartPos);

                CursorPos += length;
            }
        }
    }
}
