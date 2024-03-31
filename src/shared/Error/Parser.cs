partial class Error
{
    // Division by 0.
    public static void ZeroDivision()
    {
        _ = new Error(
            err_msg: "Division by 0",
            err_name: "parser error"
        );
    }
}
