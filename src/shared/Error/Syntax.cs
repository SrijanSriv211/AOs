partial class Error
{
    // Invalid syntax.
    public static void Syntax(string err_line, string err_tok, string err_msg)
    {
        Console.WriteLine(err_line);

        _ = new Error(
            err_msg: err_msg,
            err_cmd_name: err_tok,
            err_name: "syntax error",
            repeat_err_highlight: err_tok.Length,
            space_before_highlight: err_line.Length - err_tok.Length
        );
    }

    // Invalid syntax.
    public static void Syntax(string err_line, string err_tok, string err_msg, int err_whitespace)
    {
        Console.WriteLine(err_line);

        _ = new Error(
            err_msg: err_msg,
            err_cmd_name: err_tok,
            err_name: "syntax error",
            repeat_err_highlight: err_tok.Length,
            space_before_highlight: err_whitespace
        );
    }
}
