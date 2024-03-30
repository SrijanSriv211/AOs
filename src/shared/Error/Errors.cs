partial class Error
{
    public Error(string err_msg, string err_name="runtime error")
    {
        // Print the error_name and print the error message.
        TerminalColor.Print(err_name, ConsoleColor.Red, is_newline: false);
        Console.WriteLine($": {err_msg}");
    }

    public Error(string err_msg, string err_name="runtime error", string err_cmd_name="", char err_highlight='^', int space_before_highlight=0, int repeat_err_highlight=1)
    {
        // Store the space space_in_highlight times. It is similar to Python's:
        // err_highlight_space = " " * space_in_highlight
        // print(err_highlight_space)
        string err_highlight_space = new(' ', space_before_highlight);
        string err_highlight_repeat = new(err_highlight, repeat_err_highlight);

        // Print the error and highlight it.
        Console.Write(err_highlight_space);
        TerminalColor.Print(err_highlight_repeat, ConsoleColor.Red);
        Console.WriteLine();

        TerminalColor.Print(err_cmd_name, ConsoleColor.White, is_newline: false);
        Console.Write(": ");

        // Print the error_name and print the error message.
        TerminalColor.Print(err_name, ConsoleColor.Red, is_newline: false);
        Console.WriteLine($": {err_msg}");
    }
}
