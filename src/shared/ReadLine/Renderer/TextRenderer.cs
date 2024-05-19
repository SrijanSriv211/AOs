partial class ReadLine
{
    private string RenderedTextBuffer = "";
    private Tokenizer tokenizer;

    //TODO: Fix the word wrap thing. It's broken. Fix it.

    // Render the updated input with syntax highlighting
    private void RenderTextBuffer()
    {
        // Get difference between TextBuffer and RenderedTextBuffer
        (int, int) DiffTokenIdx = GetTokenDiff(TextBuffer, RenderedTextBuffer);

        // Loop through each token starting from first different token
        RenderToken(DiffTokenIdx.Item1, DiffTokenIdx.Item2);
        for (int i = DiffTokenIdx.Item1 + 1; i < tokenizer.tokens.Count; i++)
            RenderToken(i, 0);

        RenderedTextBuffer = TextBuffer;
    }

    // Clear changed text buffer
    private void ClearTextBuffer()
    {
        // Find the position where text buffer and rendered text buffer differ at.
        int DiffStart = GetTextDiff(TextBuffer, RenderedTextBuffer);
        int TotalDist = Config.LeftCursorStartPos + DiffStart;

        // Calculate the exact x and y positions to put the cursor at.
        int y = TotalDist / Console.WindowWidth;
        int x = TotalDist - (y * Console.WindowWidth);
        y += CursorVec3.Y;

        // Clear the screen.
        Console.SetCursorPosition(x, y);
        Console.Write(new string(' ', RenderedTextBuffer[DiffStart..].Length));
        Console.SetCursorPosition(x, y);
    }

    private void RenderToken(int token_idx, int char_idx)
    {
        ReadLine.Tokenizer.Token token = tokenizer.tokens[token_idx];

        // EOL is useless so don't render it.
        if (token.Type == Tokenizer.TokenType.EOL)
            return;

        // Do text wrapping across the terminal window if the text is too long.
        if (CursorVec3.X >= Console.WindowWidth)
        {
            Console.WriteLine();

            CursorVec3.X = 0;
            CursorVec3.Y++;
            Console.SetCursorPosition(CursorVec3.X, CursorVec3.Y);
            CursorVec3.X = 1;
        }

        // Check if the token is to be highlighted or not. If yes, then highlight.
        string Token = token.Name[char_idx..];
        if (Config.Toggle_color_coding && Config.SyntaxHighlightCodes.TryGetValue(token.Type, out ConsoleColor color))
            Terminal.Print(Token, color, false);

        // Otherwise update text after cursor normally.
        else
            Console.Write(Token);
    }
}
