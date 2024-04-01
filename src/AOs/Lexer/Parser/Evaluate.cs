namespace Lexer
{
    partial class Parser
    {
        private string Evaluate(string expr)
        {
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
                    error_detail = error_detail[(colon_index + 1)..].Trim();

                Error.Syntax(this.line, expr, error_detail);
                EntryPoint.CrashreportLog(e.ToString());
                return "";
            }

            return result;
        }
    }
}
