class TerminalColor
{
    public TerminalColor(string message, ConsoleColor Color, bool isNewLine=true)
    {
        var ForegroundColor = Console.ForegroundColor;
        Console.ForegroundColor = Color;

        if (isNewLine)
            Console.WriteLine(message);

        else
            Console.Write(message);

        Console.ForegroundColor = ForegroundColor;
    }
}
