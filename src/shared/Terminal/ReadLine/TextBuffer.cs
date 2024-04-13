partial class Terminal
{
    partial class ReadLine
    {
        // Clear current text buffer and re-render the updated input with syntax highlighting.
        private void UpdateTextBuffer(bool render_suggestion=true)
        {
            /*
            ------------------------------------------------------------
            -------------------- Render text buffer --------------------
            ------------------------------------------------------------
            */

            // Tokenize the updated input text
            Lexer.Tokenizer tokenizer = new("") { disable_error = true, line = Text };
            tokenizer.Tokenize();

            // Clear current text buffer and re-render the updated input
            Console.SetCursorPosition(CursorStartPos, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth - CursorStartPos));
            Console.SetCursorPosition(CursorStartPos, Console.CursorTop);

            // Loop through each token and check if the token is to be highlighted or not.
            // If yes, highlight, otherwise update text after cursor normally.
            foreach (Lexer.Tokenizer.Token token in tokenizer.tokens)
            {
                if (SyntaxHighlightCodes.TryGetValue(token.Type, out ConsoleColor color))
                    Print(token.Name, color, false);

                else
                    Console.Write(token.Name);
            }

            /*
            ------------------------------------------------------------
            -------------------- Render suggestions --------------------
            ------------------------------------------------------------
            */

            // Get all suggestions and render them
            GetAutocompleteSuggestions(Text);

            // Don't render the suggestions because the user doesn't want to
            if (!render_suggestion || Utils.Array.IsEmpty([.. Suggestions]))
                return;

            if (SuggestionIdx < 0 || SuggestionIdx > Suggestions.Count)
                SuggestionIdx = 0;

            // Get the current suggestion
            Suggestion = Suggestions[SuggestionIdx];

            // If suggestion is not empty then render it
            if (!Utils.String.IsEmpty(Suggestion))
            {
                // Remove the last token from the tokenizer which is 'EOL', then get the last token's name
                string buffer = tokenizer.tokens.SkipLast(1).LastOrDefault().Name;

                // Get only the uncommon part of suggestion
                Suggestion = Suggestion[buffer.Length..];

                // Render the suggestion
                Print(Suggestion, ConsoleColor.DarkGray, false);
            }
        }

        private void GetAutocompleteSuggestions(string str)
        {
            // If the string is empty then return an empty array
            if (Utils.String.IsEmpty(str))
                Suggestions = [];

            // Get the directory name from the current string and if the directory exists then,
            // set it as the search path otherwise set the current dir as search path
            string dirname = Path.GetDirectoryName(str);
            string path = Path.Exists(dirname) ? dirname : Directory.GetCurrentDirectory();

            // Find all the matching files, folders and commands if they start with 'str'
            List<string> entries = FileIO.FolderSystem.Read(path).Where(dir => dir.StartsWith(str)).ToList();
            List<string> matching_commands = EntryPoint.Settings.cmds.SelectMany(cmd => cmd.cmd_names).Where(cmd => cmd.StartsWith(str)).ToList();

            // Add all of them to suggestions list
            Suggestions = [];
            Suggestions.AddRange(entries);
            Suggestions.AddRange(matching_commands);
        }
    }
}
