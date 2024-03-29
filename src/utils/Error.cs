//TODO: Improve the Error class and make it more robust.
class Error
{
    public Error(string details)
    {
        TerminalColor.Print(details, ConsoleColor.Red);
    }

    // No arguments.
    public static void NoArgs(string cmd_name)
    {
        _ = new Error($"NoArgumentError: No arguments were passed for '{cmd_name}'");
    }

    // Unrecognized arguments.
    public static void UnrecognizedArgs(string[] args)
    {
        _ = new Error($"Unrecognized arguments: {string.Join(", ", Utils.Array.Reduce(args))}");
    }

    public static void UnrecognizedArgs(string args)
    {
        _ = new Error($"Unrecognized arguments: {args}");
    }

    // Not appropriate number of arguments.
    public static void TooFewArgs(string[] args)
    {
        _ = new Error($"Too few arguments: {string.Join(", ", Utils.Array.Reduce(args))}");
    }

    public static void TooFewArgs(string args)
    {
        _ = new Error($"Too few arguments: {args}");
    }

    public static void TooManyArgs(string[] args)
    {
        _ = new Error($"Too many arguments: {string.Join(", ", Utils.Array.Reduce(args))}");
    }

    public static void TooManyArgs(string args)
    {
        _ = new Error($"Too many arguments: {args}");
    }

    // Invalid Command.
    public static void Command(string cmd_name, string err_msg = "Command does not exist")
    {
        _ = new Error($"'{cmd_name}', {err_msg}");
    }

    // Invalid syntax.
    public static void Syntax(string err_msg)
    {
        _ = new Error($"SyntaxError: {err_msg}");
    }

    // Division by 0.
    public static void ZeroDivision()
    {
        _ = new Error("ZeroDivisionError: Division by 0");
    }
}
