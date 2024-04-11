partial class Terminal
{
    public static string TakeInput(ConsoleColor Color=ConsoleColor.Gray)
    {
        ConsoleColor default_color = Console.ForegroundColor;

        // Change the foreground color to what the user wants.
        Console.ForegroundColor = Color;
        // string output = new ReadLine().Readf();
        string output = "";

        // output = ReadLine.ConsoleReadLine.ReadLine();

        // Reset the foreground color to the default color and return the output.
        Console.ForegroundColor = default_color;
        return output;
    }
}
