partial class Lexer
{
    private string Evaluate(string expr)
    {
        System.Data.DataTable dt = new();

        expr = expr.Replace(" ", "");

        string result;
        try
        {
            result = dt.Compute(expr, "").ToString();
            if (result == "∞")
                Error.ZeroDivision();
        }

        catch (Exception e)
        {
            string error_detail = e.Message;
            int colon_index = error_detail.IndexOf(':');

            if (colon_index >= 0)
                error_detail = error_detail.Substring(colon_index + 1).Trim();

            Error.Syntax(error_detail);
            EntryPoint.CrashreportLogging(e.ToString());
            result = "";
        }

        return result;
    }
}
