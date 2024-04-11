partial class Terminal
{
    public static string TakeInput(ConsoleColor Color=ConsoleColor.Gray)
    {
        ConsoleColor default_color = Console.ForegroundColor;

        // Change the foreground color to what the user wants.
        Console.ForegroundColor = Color;
        string Out = new ReadLine().Readf();

        // Reset the foreground color to the default color and return the output.
        Console.ForegroundColor = default_color;
        return Out;
    }
}
