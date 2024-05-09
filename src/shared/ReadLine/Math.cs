partial class ReadLine
{
    private (int, int) GetTokenDiff(string text, string text2)
    {
        // Tokenize the updated input text
        tokenizer = new("") { line = text };
        tokenizer.Tokenize();

        // Tokenize the updated rendered input text
        ReadLine.Tokenizer _tokenizer = new("") { line = text2 };
        _tokenizer.Tokenize();

        for (int token_idx = 0; token_idx < Math.Min(tokenizer.tokens.Count, _tokenizer.tokens.Count); token_idx++)
        {
            string text_token = tokenizer.tokens[token_idx].Name;
            string rendered_text_token = _tokenizer.tokens[token_idx].Name;

            if (text_token != rendered_text_token)
                return (token_idx, GetTextDiff(text_token, rendered_text_token));
        }

        // (token_idx, char_idx)
        return (0, 0);
    }

    private int GetTextDiff(string text, string text2)
    {
        for (int char_idx = 0; char_idx < Math.Min(text.Length, text2.Length); char_idx++)
        {
            if (text[char_idx] != text2[char_idx])
                return char_idx;
        }

        if (text.Length != text2.Length)
            return Math.Min(text.Length, text2.Length);

        return 0;
    }
}
