partial class Terminal
{
    partial class ReadLine
    {
        // (token_idx, char_idx)
        private (int, int) GetDiff()
        {
            // Tokenize the updated input text
            tokenizer = new("") { line = Text };
            tokenizer.Tokenize();

            // Tokenize the updated rendered input text
            ReadLine.Tokenizer _tokenizer = new("") { line = RenderedText };
            _tokenizer.Tokenize();

            for (int token_idx = 0; token_idx < Math.Min(tokenizer.tokens.Count, _tokenizer.tokens.Count); token_idx++)
            {
                string text = tokenizer.tokens[token_idx].Name;
                string rendered_text = _tokenizer.tokens[token_idx].Name;

                if (text != rendered_text)
                    return (token_idx, GetTextDiff(text, rendered_text));
            }

            return (-1, -1);
        }

        private int GetTextDiff(string text, string text2)
        {
            for (int char_idx = 0; char_idx < Math.Min(text.Length, text2.Length); char_idx++)
            {
                if (text[char_idx] != text2[char_idx])
                    return char_idx;
            }

            return -1;
        }
    }
}
