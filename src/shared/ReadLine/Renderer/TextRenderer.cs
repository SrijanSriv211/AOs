partial class ReadLine
{
    private string RenderedTextBuffer = "";
    private ReadLine.Tokenizer tokenizer;
    private (int, int) DiffTokenIdx = (0, 0);

    private void UpdateBuffer(bool RenderSuggestions=true)
    {
        ClearTextBuffer();
        RenderTextBuffer();
    }

    // Render the updated input with syntax highlighting.
    private void RenderTextBuffer()
    {
        // Loop through each token starting from first different token
        RenderToken(DiffTokenIdx.Item1, DiffTokenIdx.Item2);
        for (int i = DiffTokenIdx.Item1 + 1; i < tokenizer.tokens.Count; i++)
            RenderToken(i, 0);
    }

    // Clear changed text buffer
    private void ClearTextBuffer()
    {
        // Get difference between TextBuffer and RenderedTextBuffer
        DiffTokenIdx = GetTokenDiff(TextBuffer, RenderedTextBuffer);

        // No updating is needed.
        if (CursorVec3.I == TextBuffer.Length)
            return;

        // int TextDiffStart = string.Join("", tokenizer.tokens[..DiffTokenIdx.Item1].SelectMany(x => x.Name)).Length + DiffTokenIdx.Item2;

        // // Calculate the (x, y) pos where to put the cursor
        // (int y, double _) = Utils.Maths.SplitNumber((float)TextDiffStart / Console.WindowWidth);
        // int Row = CursorVec3.Y + y;

        // Clear the screen.
        // Console.SetCursorPosition(Config.LeftCursorStartPos + TextDiffStart, CursorVec3.Y + y);
        // Console.Write(new string(' ', Console.WindowWidth - Config.LeftCursorStartPos - TextDiffStart));
        // Console.SetCursorPosition(Config.LeftCursorStartPos + TextDiffStart, CursorVec3.Y + y);


        // int x = CursorVec3.X;
        // int y = CursorVec3.Y;

        // Console.Write(new string('-', RenderedTextBuffer.Length));
        // Console.SetCursorPosition(CursorVec3.X, CursorVec3.Y);

        RenderedTextBuffer = TextBuffer;
    }

    private void RenderToken(int token_idx, int char_idx)
    {
        ReadLine.Tokenizer.Token token = tokenizer.tokens[token_idx];

        // Do text wrapping across the terminal window if the text is too long.
        if (CursorVec3.X == Console.WindowWidth)
        {
            Console.WriteLine();

            CursorVec3.X = 0;
            CursorVec3.Y++;
            Console.SetCursorPosition(CursorVec3.X, CursorVec3.Y);
            CursorVec3.X++;
        }

        // Check if the token is to be highlighted or not. If yes, then highlight.
        if (Config.SyntaxHighlightCodes.TryGetValue(token.Type, out ConsoleColor color))
        {
            if (Config.Toggle_color_coding)
                Terminal.Print(token.Name[char_idx..], color, false);

            else
                Console.Write(token.Name[char_idx..]);
        }

        // Otherwise update text after cursor normally.
        else if (token.Type != Lexer.Tokenizer.TokenType.EOL)
            Console.Write(token.Name[char_idx..]);
    }
}
