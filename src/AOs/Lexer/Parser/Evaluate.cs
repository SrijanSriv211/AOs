namespace Lexer
{
    partial class Parser
    {
        private string Evaluate(string expr)
        {
            string result;

            // Try to evaluate the expr
            try
            {
                result = dt.Compute(expr.Replace(" ", ""), "").ToString();
                // Division be zero error if the answer is infinity
                if (result == "∞")
                    Error.ZeroDivision();
            }

            // Error found, catch it and print the details.
            catch (Exception e)
            {
                string error_detail = e.Message;
                int colon_index = error_detail.IndexOf(':');

                if (colon_index >= 0)
                    error_detail = error_detail[(colon_index + 1)..].Trim();

                // Print the error and save it to crashreport.
                Error.Syntax(this.line, expr, error_detail);
                EntryPoint.CrashreportLog(e.ToString());
                // Return 'NaN' showing that the result is not a number and there must be some error caused.
                return "NaN";
            }

            return result;
        }
    }
}
