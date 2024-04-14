partial class Terminal
{
    //TODO: Continue on working and improving the ReadBuffer code.
    partial class ReadLine
    {
        private string RenderedText = "";
        private List<string> RenderedSuggestions = [];
        private int RenderedSuggestionIdx = -1;

        private string Suggestion = "";
        private List<string> Suggestions = [];
        private ReadLine.Tokenizer tokenizer;

        private void UpdateBuffer(bool render_suggestions=true)
        {
            ClearTextBuffer();
            RenderTextBuffer();
            // ClearSuggestionBuffer();
            // RenderSuggestionBuffer(render_suggestions);
        }

        // Clear changed text buffer
        private void ClearTextBuffer()
        {
            int diff_start = GetTextDiff(Text, RenderedText);
            Console.WriteLine(diff_start);
            RenderedText = Text;

            Console.SetCursorPosition(LeftCursorStartPos + diff_start, TopCursorPos);
            Console.Write(new string(' ', Console.WindowWidth - LeftCursorStartPos - diff_start));
            Console.SetCursorPosition(LeftCursorStartPos + diff_start, TopCursorPos);
        }

        // Clear current text buffer and re-render the updated input with syntax highlighting.
        private void RenderTextBuffer()
        {
            // Loop through each token and check if the token is to be highlighted or not.
            // If yes, highlight, otherwise update text after cursor normally.
            Console.Write(Text[RenderedText.Length..]);
            // foreach (ReadLine.Tokenizer.Token token in tokenizer.tokens)
            // {
            //     if (SyntaxHighlightCodes.TryGetValue(token.Type, out ConsoleColor color))
            //         Print(token.Name, color, false);

            //     else
            //         Console.Write(token.Name);
            // }
        }

        private void ClearSuggestionBuffer()
        {
        }

        private void RenderSuggestionBuffer(bool render_suggestions)
        {
            // Get all suggestions and render them
            GetAutocompleteSuggestions(Text);

            // Don't render the suggestions because the user doesn't want to
            if (!render_suggestions || Utils.Array.IsEmpty(Suggestions.ToArray()))
                return;

            if (SuggestionIdx < 0 || SuggestionIdx > Suggestions.Count-1)
                SuggestionIdx = 0;

            // Get the current suggestion
            Suggestion = Suggestions[SuggestionIdx];

            // If suggestion is not empty then render it
            if (!Utils.String.IsEmpty(Suggestion))
            {
                // Remove the last token from the tokenizer which is 'EOL', then get the last token's name
                string buffer = tokenizer.tokens.SkipLast(1).LastOrDefault().Name;

                // Render the suggestion
                // Print(Suggestion, ConsoleColor.DarkGray, false);
                Console.WriteLine();
                for (int i = 0; i < Suggestions.Count; i++)
                    Print(Suggestions[i] + "    ", Suggestions[i] == Suggestion ? ConsoleColor.Blue : ConsoleColor.DarkGray, false);

                // Get only the uncommon part of suggestion
                Suggestion = Suggestion[buffer.Length..];
            }
        }
    }
}
