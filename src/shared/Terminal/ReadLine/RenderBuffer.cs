partial class Terminal
{
    partial class ReadLine
    {
        private string RenderedText = "";

        private string RenderedSuggestions = "";
        private string Suggestion = "";
        private List<string> Suggestions = [];
        private ReadLine.Tokenizer tokenizer;
        private (int, int) diff_token_idx = (0, 0);

        private void UpdateBuffer(bool render_suggestions=true)
        {
            ClearTextBuffer();
            RenderTextBuffer();

            if (toggle_autocomplate)
            {
                ClearSuggestionBuffer();
                RenderSuggestionBuffer(render_suggestions);
            }
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
                {
                    if (toggle_color_coding)
                        Print(token.Name[char_idx..], color, false);

                    else
                        Console.Write(token.Name[char_idx..]);
                }

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
            Console.SetCursorPosition(0, Console.CursorTop + 1);
            Console.Write(new string(' ', RenderedSuggestions.Length));
            Console.SetCursorPosition(LeftCursorPos, TopCursorPos);
            RenderedSuggestions = "";
        }

        private void RenderSuggestionBuffer(bool render_suggestions)
        {
            // Remove the last token from the tokenizer which is 'EOL', then get the last token's name
            string buffer = tokenizer.tokens.SkipLast(1).LastOrDefault().Name;

            // Get all suggestions
            GetAutocompleteSuggestions(buffer);

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
                // Move to new line to render suggestions
                Console.WriteLine();

                // Render the suggestions
                string str_suggestions = "";
                for (int i = 0; i < Suggestions.Count; i++)
                {
                    string suggestion = Suggestions[i];
                    str_suggestions += suggestion + "    ";
                    RenderedSuggestions += suggestion + "    ";

                    Print(suggestion + "    ", suggestion == Suggestion ? ConsoleColor.Blue : ConsoleColor.DarkGray, false);

                    if ((i+1) % 12 == 0)
                    {
                        string whitespaces = new(' ', Console.WindowWidth - str_suggestions.Length);
                        Console.Write(whitespaces);

                        RenderedSuggestions += whitespaces;
                        str_suggestions = "";
                    }
                }

                // Get only the uncommon part of suggestion
                Suggestion = Suggestion[buffer.Length..];
            }
        }
    }
}
