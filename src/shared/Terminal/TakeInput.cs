partial class Terminal
{
    public static string TakeInput(string Prompt="", ConsoleColor PromptColor=ConsoleColor.Gray, ConsoleColor InputColor=ConsoleColor.Gray)
    {
        ConsoleColor default_color = Console.ForegroundColor;
        Print(Prompt, PromptColor, false);

        // Change the foreground color to what the user wants.
        Console.ForegroundColor = InputColor;
        string output = new ReadLine().Readf();

        // Reset the foreground color to the default color and return the output.
        Console.ForegroundColor = default_color;
        return output;
    }
}
