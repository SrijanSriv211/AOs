partial class Terminal
{
    partial class ReadLine
    {
        // Clear current text buffer and re-render the updated input with syntax highlighting.
        private void UpdateTextBuffer(int overwrite_len)
        {
            if (Text == PreviousText)
                return;

            // Tokenize the updated input text
            Lexer.Tokenizer tokenizer = new("") { disable_error = true, line = Text };
            tokenizer.Tokenize();

            // Clear current text buffer and re-render the updated input
            Console.SetCursorPosition(CursorStartPos, Console.CursorTop);
            Console.Write(new string(' ', overwrite_len));
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

            // Update the previous text with the new one
            PreviousText = Text;
        }
    }
}
