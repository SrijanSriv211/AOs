class TerminalColor
{
    public static void Print(string message, ConsoleColor color, bool is_newline=true)
    {
        var default_color = Console.ForegroundColor;
        Console.ForegroundColor = color;

        if (is_newline)
            Console.WriteLine(message);

        else
            Console.Write(message);

        Console.ForegroundColor = default_color;
    }

    public static string TakeInput(ConsoleColor color)
    {
        var default_color = Console.ForegroundColor;
        Console.ForegroundColor = color;

        string output = Console.ReadLine();

        Console.ForegroundColor = default_color;
        return output;
    }
}
