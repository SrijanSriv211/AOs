partial class Error
{
    // The given command cannot be found.
    public static void Command(string err_line, string err_cmd_name, string err_msg = "Command not found")
    {
        // Check for spelling mistakes in the command which maybe done by the user.
        Utils.SpellCheck checker = new(EntryPoint.Settings.cmd_schema.SelectMany(cmd => cmd.cmd_names).ToList());
        var suggestions = checker.Check(err_cmd_name, suggestions_len: 3);
        string[] suggested_spell_checks = suggestions.Select(pair => pair.Item1).ToArray();

        // Highlight the command which is not found.
        Console.WriteLine(err_line);
        new Error(
            err_msg: err_msg,
            err_cmd_name: err_cmd_name,
            repeat_err_highlight: err_cmd_name.Length
        );

        // Suggest some possible corrections if there is a spelling mistake.
        Terminal.Print($"Did you mean: {string.Join(", ", suggested_spell_checks)}?", ConsoleColor.DarkGray);
    }

    // Throw error when no arguments are passed for a command.
    public static void NoArgs(string err_cmd_name)
    {
        Console.WriteLine(err_cmd_name);

        new Error(
            err_msg: "No arguments were passed",
            err_cmd_name: err_cmd_name,
            space_before_highlight: err_cmd_name.Length+1
        );
    }

    // Unrecognized arguments.
    public static void UnrecognizedArgs(string[] args)
    {
        string err_args = string.Join(", ", Utils.Array.Reduce(args));

        Console.WriteLine(err_args);
        new Error(
            err_msg: "Unrecognized arguments",
            err_cmd_name: err_args,
            repeat_err_highlight: err_args.Length
        );
    }

    public static void UnrecognizedArgs(string err_arg)
    {
        Console.WriteLine(err_arg);
        new Error(
            err_msg: "Unrecognized arguments",
            err_cmd_name: err_arg,
            repeat_err_highlight: err_arg.Length
        );
    }

    // Not appropriate number of arguments.
    public static void TooFewArgs(string[] args)
    {
        string err_args = string.Join(", ", Utils.Array.Reduce(args));

        Console.WriteLine(err_args);
        new Error(
            err_msg: "Too few arguments",
            err_cmd_name: err_args,
            repeat_err_highlight: err_args.Length
        );
    }

    public static void TooFewArgs(string err_arg)
    {
        Console.WriteLine(err_arg);
        new Error(
            err_msg: "Too few arguments",
            err_cmd_name: err_arg,
            repeat_err_highlight: err_arg.Length
        );
    }

    public static void TooManyArgs(string[] args)
    {
        string err_args = string.Join(", ", Utils.Array.Reduce(args));

        Console.WriteLine(err_args);
        new Error(
            err_msg: "Too few arguments",
            err_cmd_name: err_args,
            repeat_err_highlight: err_args.Length
        );
    }

    public static void TooManyArgs(string err_arg)
    {
        Console.WriteLine(err_arg);
        new Error(
            err_msg: "Too few arguments",
            err_cmd_name: err_arg,
            repeat_err_highlight: err_arg.Length
        );
    }
}
