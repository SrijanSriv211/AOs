partial class Terminal
{
    private partial class ReadLine
    {
        private void HandleEnter()
        {
            if (Toggle_autocomplate)
                ClearSuggestionBuffer();

            TopCursorPos++;
            Console.WriteLine();
            Loop = false;
        }

        // Set the current suggesion in the text.
        private void HandleCtrlEnter()
        {
            Text += Suggestion;
            LeftCursorPos += Suggestion.Length;
            SuggestionIdx = 0;
            UpdateBuffer(false);
        }

        // Render the next suggestion
        private void HandleTab()
        {
            if (Utils.Array.IsEmpty([.. Suggestions]))
                return;

            SuggestionIdx = (SuggestionIdx + 1) % Suggestions.Count;
            if (SuggestionIdx < 0 || SuggestionIdx > Suggestions.Count)
                SuggestionIdx = 0;

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
            if (Utils.String.IsEmpty(RenderedSuggestions))
                return;

            SuggestionIdx = 0;
            ClearSuggestionBuffer();
        }

        // Clear all the text
        private void HandleShiftEscape()
        {
            Text = "";
            LeftCursorPos = LeftCursorStartPos;
            UpdateBuffer(false);
        }

        private void HandleBackspace()
        {
            if (LeftCursorPos > LeftCursorStartPos)
            {
                LeftCursorPos--;
                Text = Text.Remove(LeftCursorPos - LeftCursorStartPos, 1);
                UpdateBuffer();
            }
        }

        private void HandleCtrlBackspace()
        {
            if (LeftCursorPos > LeftCursorStartPos)
            {
                if (Text.LastIndexOf(' ', LeftCursorPos - LeftCursorStartPos - 1) == LeftCursorPos - LeftCursorStartPos - 1)
                    HandleBackspace();

                int PreviousWordIdx = Text.LastIndexOf(' ', LeftCursorPos - LeftCursorStartPos - 1);
                int length = LeftCursorPos - LeftCursorStartPos - PreviousWordIdx - 1;

                LeftCursorPos -= length;
                Text = Text.Remove(LeftCursorPos - LeftCursorStartPos, length);
                UpdateBuffer();
            }
        }

        private void HandleDelete()
        {
            if (LeftCursorPos - LeftCursorStartPos < Text.Length)
            {
                Text = Text.Remove(LeftCursorPos - LeftCursorStartPos, 1);
                UpdateBuffer();
            }
        }

        private void HandleCtrlDelete()
        {
            if (LeftCursorPos - LeftCursorStartPos < Text.Length)
            {
                if (Text.IndexOf(' ', LeftCursorPos - LeftCursorStartPos) == LeftCursorPos - LeftCursorStartPos)
                    HandleDelete();

                int NextWordIdx = Text.IndexOf(' ', LeftCursorPos - LeftCursorStartPos);
                int length = NextWordIdx == -1 ? Text.Length - (LeftCursorPos - LeftCursorStartPos) : NextWordIdx - (LeftCursorPos - LeftCursorStartPos);

                Text = Text.Remove(LeftCursorPos - LeftCursorStartPos, length);
                UpdateBuffer();
            }
        }

        private void HandleHome()
        {
            LeftCursorPos = LeftCursorStartPos;
        }

        private void HandleEnd()
        {
            LeftCursorPos = Text.Length;
        }

        private void HandleLeftArrow()
        {
            if (LeftCursorPos > LeftCursorStartPos)
                LeftCursorPos--;
        }

        private void HandleCtrlLeftArrow()
        {
            if (LeftCursorPos > LeftCursorStartPos)
            {
                if (Text.LastIndexOf(' ', LeftCursorPos - LeftCursorStartPos - 1) == LeftCursorPos - LeftCursorStartPos - 1)
                    LeftCursorPos--;

                int PreviousWordIdx = Text.LastIndexOf(' ', LeftCursorPos - LeftCursorStartPos - 1);
                int length = LeftCursorPos - LeftCursorStartPos - PreviousWordIdx - 1;

                LeftCursorPos -= length;
            }
        }

        private void HandleRightArrow()
        {
            if (LeftCursorPos - LeftCursorStartPos < Text.Length)
                LeftCursorPos++;
        }

        private void HandleCtrlRightArrow()
        {
            if (LeftCursorPos - LeftCursorStartPos < Text.Length)
            {
                if (Text.IndexOf(' ', LeftCursorPos - LeftCursorStartPos) == LeftCursorPos - LeftCursorStartPos)
                    LeftCursorPos++;

                int NextWordIdx = Text.IndexOf(' ', LeftCursorPos - LeftCursorStartPos);
                int length = NextWordIdx == -1 ? Text.Length - (LeftCursorPos - LeftCursorStartPos) : NextWordIdx - (LeftCursorPos - LeftCursorStartPos);

                LeftCursorPos += length;
            }
        }
    }
}
