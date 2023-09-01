class Error
{
    public Error(string details)
    {
        new TerminalColor(details, ConsoleColor.Red);
    }

    // No arguments.
    public static void NoArgs()
    {
        new Error("NoArgumentError: No arguments were passed");
    }

    // Unrecognized arguments.
    public static void UnrecognizedArgs(string[] _Flag)
    {
        new Error($"Unrecognized arguments: {string.Join(", ", Utils.Array.Reduce(_Flag))}");
    }

    public static void UnrecognizedArgs(string _Flag)
    {
        new Error($"Unrecognized arguments: {_Flag}");
    }

    // Not appropriate number of arguments.
    public static void TooFewArgs(string[] _Flag)
    {
        new Error($"Too few arguments: {string.Join(", ", Utils.Array.Reduce(_Flag))}");
    }

    public static void TooFewArgs(string _Flag)
    {
        new Error($"Too few arguments: {_Flag}");
    }

    public static void TooManyArgs(string[] _Flag)
    {
        new Error($"Too many arguments: {string.Join(", ", Utils.Array.Reduce(_Flag))}");
    }

    public static void TooManyArgs(string _Flag)
    {
        new Error($"Too many arguments: {_Flag}");
    }

    // Invalid Command.
    public static void Command(string _Command, string _Details = "Command does not exist")
    {
        new Error($"'{_Command}', {_Details}");
    }

    // Invalid syntax.
    public static void Syntax(string _Details)
    {
        new Error($"SyntaxError: {_Details}");
    }

    // Division by 0.
    public static void ZeroDivision()
    {
        new Error("ZeroDivisionError: Division by 0");
    }
}
