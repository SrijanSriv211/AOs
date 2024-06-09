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
                // '_' can be used to seperate the numbers to make it more readable.
                // I've taken this feature from python. It's a cool feature to have.
                // For eg,
                // 100_000_000 -> 100000000
                result = dt.Compute(expr.Replace(" ", "").Replace("_", ""), "").ToString();
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
