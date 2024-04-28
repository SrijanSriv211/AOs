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
        private (int, int) diff_token_idx = (0, 0);

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
            diff_token_idx = GetTokenDiff(Text, RenderedText);
            RenderedText = Text;

            int diff_start = string.Join("", tokenizer.tokens[..diff_token_idx.Item1].SelectMany(x => x.Name)).Length + diff_token_idx.Item2;

            Console.SetCursorPosition(LeftCursorStartPos + diff_start, TopCursorPos);
            Console.Write(new string(' ', Console.WindowWidth - LeftCursorStartPos - diff_start));
            Console.SetCursorPosition(LeftCursorStartPos + diff_start, TopCursorPos);
        }

        // Clear current text buffer and re-render the updated input with syntax highlighting.
        private void RenderTextBuffer()
        {
            void RenderText(int token_idx, int char_idx)
            {
                ReadLine.Tokenizer.Token token = tokenizer.tokens[token_idx];

                // Check if the token is to be highlighted or not. If yes, then highlight.
                if (SyntaxHighlightCodes.TryGetValue(token.Type, out ConsoleColor color))
                    Print(token.Name[char_idx..], color, false);

                // Otherwise update text after cursor normally.
                else if (token.Type != Lexer.Tokenizer.TokenType.EOL)
                    Console.Write(token.Name[char_idx..]);
            }

            // Loop through each token starting from first different token
            RenderText(diff_token_idx.Item1, diff_token_idx.Item2);
            for (int i = diff_token_idx.Item1 + 1; i < tokenizer.tokens.Count; i++)
                RenderText(i, 0);
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
