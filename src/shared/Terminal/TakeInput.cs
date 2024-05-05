partial class Terminal
{
    public static string TakeInput(string Prompt="", ConsoleColor PromptColor=ConsoleColor.Gray, ConsoleColor InputColor=ConsoleColor.Gray, ReadLineConfig Config = null)
    {
        ConsoleColor DefaultColor = Console.ForegroundColor;
        Print(Prompt, PromptColor, false);

        // Change the foreground color to what the user wants.
        Console.ForegroundColor = InputColor;

        ReadLine readline = new(Config);
        readline.InitDefaultKeyBindings();
        string Output = Config == null ? Console.ReadLine() : readline.Readf();

        // Reset the foreground color to the default color and return the output.
        Console.ForegroundColor = DefaultColor;
        return Output;
    }
}
