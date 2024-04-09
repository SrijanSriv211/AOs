partial class Error
{
    // Division by 0.
    public static void ZeroDivision()
    {
        new Error(
            err_msg: "Division by 0",
            err_name: "parser error"
        );
    }
}
