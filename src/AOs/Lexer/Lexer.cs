namespace Lexer
{
    class Lexer
    {
        public readonly List<string[]> Tokens = [];

        public Lexer(string line)
        {
            Parser parser = new(line);
            Tokens = parser.Parse(new Tokenizer(line).tokens);
        }
    }
}
