partial class Terminal
{
    public static void Print(string message, ConsoleColor color=ConsoleColor.Gray, bool is_newline=true)
    {
        var default_color = Console.ForegroundColor;
        Console.ForegroundColor = color;

        if (is_newline)
            Console.WriteLine(message);

        else
            Console.Write(message);

        Console.ForegroundColor = default_color;
    }
}
