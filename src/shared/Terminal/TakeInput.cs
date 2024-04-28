partial class Terminal
{
    public static string TakeInput(string Prompt="", ConsoleColor PromptColor=ConsoleColor.Gray, ConsoleColor InputColor=ConsoleColor.Gray, bool color_coding=true, bool autocomplete=true)
    {
        ConsoleColor default_color = Console.ForegroundColor;
        Print(Prompt, PromptColor, false);

        // Change the foreground color to what the user wants.
        Console.ForegroundColor = InputColor;
        string output = (color_coding || autocomplete) ? new ReadLine(color_coding, autocomplete).Readf() : Console.ReadLine();

        // Reset the foreground color to the default color and return the output.
        Console.ForegroundColor = default_color;
        return output;
    }
}
