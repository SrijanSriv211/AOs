partial class Terminal
{
    partial class ReadLine
    {
        private string RenderedText = "";

        private string RenderedSuggestions = "";
        private string Suggestion = "";
        private List<string> Suggestions = [];
        private ReadLine.Tokenizer tokenizer;
        private (int, int) Diff_token_idx = (0, 0);

        private void UpdateBuffer(bool Render_suggestions=true)
        {
            ClearTextBuffer();
            RenderTextBuffer();

            if (Toggle_autocomplate)
            {
                ClearSuggestionBuffer();

                if (Render_suggestions)
                    RenderSuggestionBuffer();
            }
        }

        // Clear changed text buffer
        private void ClearTextBuffer()
        {
            Diff_token_idx = GetTokenDiff(Text, RenderedText);
            RenderedText = Text;

            int diff_start = string.Join("", tokenizer.tokens[..Diff_token_idx.Item1].SelectMany(x => x.Name)).Length + Diff_token_idx.Item2;

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
                    if (Toggle_color_coding)
                        Print(token.Name[char_idx..], color, false);

                    else
                        Console.Write(token.Name[char_idx..]);
                }

                // Otherwise update text after cursor normally.
                else if (token.Type != Lexer.Tokenizer.TokenType.EOL)
                    Console.Write(token.Name[char_idx..]);
            }

            // Loop through each token starting from first different token
            RenderText(Diff_token_idx.Item1, Diff_token_idx.Item2);
            for (int i = Diff_token_idx.Item1 + 1; i < tokenizer.tokens.Count; i++)
                RenderText(i, 0);
        }

        private void ClearSuggestionBuffer()
        {
            if (Utils.String.IsEmpty(RenderedSuggestions))
                return;

            Console.SetCursorPosition(0, TopCursorPos + 1);
            Console.Write(new string(' ', RenderedSuggestions.Length));
            Console.SetCursorPosition(LeftCursorPos, TopCursorPos);

            RenderedSuggestions = "";
        }

        private void RenderSuggestionBuffer()
        {
            // Get all suggestions
            List<List<string>> Suggested_commands = [SuggestCommands(Text), SuggestHistoricalCommands(Text)];
            foreach (List<string> item in Suggested_commands)
            {
                if (!Utils.Array.IsEmpty(item.ToArray()))
                {
                    Suggestions = item;
                    break;
                }
            }

            if (Utils.Array.IsEmpty(Suggestions.ToArray()))
                return;

            else if (SuggestionIdx < 0 || SuggestionIdx > Suggestions.Count-1)
                SuggestionIdx = 0;

            // Get the current suggestion
            Suggestion = Suggestions[SuggestionIdx];

            // Move to new line to render suggestions
            Console.WriteLine();

            // Render the suggestions
            string Suggestions_buffer = "";
            for (int i = 0; i < Suggestions.Count; i++)
            {
                Suggestions_buffer += Suggestions[i] + "    ";
                Print(Suggestions[i] + "    ", Suggestion == Suggestions[i] ? ConsoleColor.Blue : ConsoleColor.DarkGray, false);

                if ((i+1) % 12 == 0)
                {
                    string whitespace = new(' ', Console.WindowWidth - (Suggestions_buffer.Length % Console.WindowWidth));
                    Suggestions_buffer += whitespace;
                    Console.Write(whitespace);
                }
            }

            // Get only the uncommon part of suggestion
            Suggestion = Text.Length <= Suggestion.Length ? Suggestion[Text.Length..] : "";
            RenderedSuggestions = Suggestions_buffer;
        }
    }
}
