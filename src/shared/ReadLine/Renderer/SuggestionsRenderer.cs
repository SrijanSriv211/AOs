partial class ReadLine
{
    private string CurrentSuggestionBuffer = "";
    private string RenderedSuggestionsBuffer = "";
    private List<string> AllSuggestions = [];
    private int CurrentSuggestionIdx = 0;

    private void ClearSuggestionBuffer()
    {
        if (Utils.String.IsEmpty(RenderedSuggestionsBuffer))
            return;

        // Console.SetCursorPosition(0, TopCursorPos + 1);
        // Console.Write(new string(' ', RenderedSuggestions.Length));
        // Console.SetCursorPosition(LeftCursorPos, TopCursorPos);

        // RenderedSuggestions = "";
    }

    private void RenderSuggestionBuffer()
    {
        // // Get all suggestions
        // List<List<string>> Suggested_commands = [SuggestCommands(TextBuffer), SuggestHistoricalCommands(TextBuffer)];
        // foreach (List<string> item in Suggested_commands)
        // {
        //     if (!Utils.Array.IsEmpty(item.ToArray()))
        //     {
        //         Suggestions = item;
        //         break;
        //     }
        // }

        // if (Utils.Array.IsEmpty(Suggestions.ToArray()))
        //     return;

        // else if (CurrentSuggestionIdx < 0 || CurrentSuggestionIdx > Suggestions.Count-1)
        //     CurrentSuggestionIdx = 0;

        // // Get the current suggestion
        // Suggestion = Suggestions[CurrentSuggestionIdx];

        // // Move to new line to render suggestions
        // Console.WriteLine();

        // // Render the suggestions
        // string Suggestions_buffer = "";
        // for (int i = 0; i < Suggestions.Count; i++)
        // {
        //     Suggestions_buffer += Suggestions[i] + "    ";
        //     Terminal.Print(Suggestions[i] + "    ", Suggestion == Suggestions[i] ? ConsoleColor.Blue : ConsoleColor.DarkGray, false);

        //     if ((i+1) % 12 == 0)
        //     {
        //         string whitespace = new(' ', Console.WindowWidth - (Suggestions_buffer.Length % Console.WindowWidth));
        //         Suggestions_buffer += whitespace;
        //         Console.Write(whitespace);
        //     }
        // }

        // // Get only the uncommon part of suggestion
        // Suggestion = TextBuffer.Length <= Suggestion.Length ? Suggestion[TextBuffer.Length..] : "";
        // RenderedSuggestions = Suggestions_buffer;
    }
}
