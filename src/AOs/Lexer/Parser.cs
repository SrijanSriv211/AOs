partial class Lexer
{
    private void Parse(string[] toks)
    {
        List<string> current_list = new();

        for (int i = 0; i < toks.Length; i++)
        {
            string tok = toks[i];

            if (tok == ";")
            {
                this.Tokens.Add(current_list.ToArray());
                current_list = new List<string>();

                if (i-1 < toks.Length && Utils.String.IsWhiteSpace(toks[i+1]))
                    i++;
            }

            else if (Is_expr(tok))
            {
                string expr = tok;

                i++;
                while (i < toks.Length && (Is_expr(toks[i]) || Utils.String.IsWhiteSpace(toks[i])))
                {
                    if (i < toks.Length-1 && Utils.String.IsWhiteSpace(toks[i]) && !Is_expr(toks[i+1]))
                        break;

                    expr += toks[i];
                    i++;
                }

                i--;

                current_list.Add(Evaluate(expr));
            }

            else
                current_list.Add(tok);
        }

        // Add the last sublist to the result list
        this.Tokens.Add(current_list.ToArray());
    }
}
