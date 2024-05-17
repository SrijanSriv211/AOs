partial class ReadLine
{
    private string RenderedTextBuffer = "";
    private Tokenizer tokenizer;
    private (int, int) DiffTokenIdx = (0, 0);

    private void UpdateBuffer(bool RenderSuggestions=true)
    {
        ClearTextBuffer();
        RenderTextBuffer();
    }

    // Render the updated input with syntax highlighting
    private void RenderTextBuffer()
    {
        // Get difference between TextBuffer and RenderedTextBuffer
        DiffTokenIdx = GetTokenDiff(TextBuffer, RenderedTextBuffer);

        // Loop through each token starting from first different token
        RenderToken(DiffTokenIdx.Item1, DiffTokenIdx.Item2);

        if (CursorVec3.I < TextBuffer.Length)
        {
            for (int i = DiffTokenIdx.Item1 + 1; i < tokenizer.tokens.Count; i++)
                RenderToken(i, 0);
        }

        RenderedTextBuffer = TextBuffer;
    }

    // Clear changed text buffer
    private void ClearTextBuffer()
    {
        Console.SetCursorPosition(CursorVec3.X, CursorVec3.Y);
        Console.Write(new string(' ', Console.WindowWidth - RenderedTextBuffer.Length));
        Console.SetCursorPosition(CursorVec3.X, CursorVec3.Y);
    }

    private void RenderToken(int token_idx, int char_idx)
    {
        // Do text wrapping across the terminal window if the text is too long.
        if (CursorVec3.X == Console.WindowWidth)
        {
            Console.WriteLine();

            CursorVec3.X = 0;
            CursorVec3.Y++;
            Console.SetCursorPosition(CursorVec3.X, CursorVec3.Y);
            CursorVec3.X++;
        }

        ReadLine.Tokenizer.Token token = tokenizer.tokens[token_idx];

        // Check if the token is to be highlighted or not. If yes, then highlight.
        if (Config.SyntaxHighlightCodes.TryGetValue(token.Type, out ConsoleColor color) && Config.Toggle_color_coding)
            Terminal.Print(token.Name[char_idx..], color, false);

        // Otherwise update text after cursor normally.
        else
            Console.Write(token.Name[char_idx..]);
    }
}
