namespace Lexer
{
    class Lexer
    {
        public List<string[]> Tokens = [];

        public Lexer(string line)
        {
            List<Tokenizer.Token> tokens = new Tokenizer(line).tokens;

            Console.WriteLine(line);
            foreach (var i in tokens)
            {
                Console.Write(i.Name);
                Console.Write(" : ");
                Console.WriteLine(i.Type);
            }

            // Parse(Tokenizer());

            // foreach (string[] tokens in Tokens)
            // {
            //     for (int i = 0; i < tokens.Length; i++)
            //     {
            //         string token = tokens[i];
            //         if (token.StartsWith("%") && token.EndsWith("%"))
            //             tokens[i] = SystemUtils.CheckForSysOrEnvApps(token);
            //     }
            // }
        }
    }
}
