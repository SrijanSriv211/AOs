partial class Argparse
{
    public void PrintHelp(Argument details)
    {
        string names = string.Join(", ", details.Names);
        string desc = details.Help;
        string default_value = (details.Default_value != null && !Utils.String.IsEmpty(details.Default_value)) ? $" (default: {details.Default_value})" : "";
        string is_flag = details.Is_flag == true ? $" (is flag: true)" : "";
        string is_required = details.Required != false ? $" (required: true)" : "";

        new TerminalColor("Name:", ConsoleColor.Cyan);
        Console.Write("{0," + -Utils.Maths.CalcPadding(1) + "}", names);
        new TerminalColor(desc + "\n", ConsoleColor.DarkGray);

        new TerminalColor("Details:", ConsoleColor.Blue);
        Console.WriteLine($"{names} [OPTIONS] {default_value}{is_flag}{is_required}");
    }

    public void GetHelp(string[] cmd_names)
    {
        cmd_names = Utils.Array.Reduce(cmd_names);

        if (Utils.Array.IsEmpty(cmd_names))
        {
            Console.WriteLine("Type `help <command-name>` for more information on a specific command");

            for (int i = 0; i < arguments.Count; i++)
            {
                var detail = arguments[i];

                string[] command_names = detail.Names;
                string description = detail.Help;
                int padding = Utils.Maths.CalcPadding(i+1);

                new TerminalColor($"{i+1}. ", ConsoleColor.DarkGray, false);
                Console.Write("{0," + -padding + "}", string.Join(", ", command_names));
                new TerminalColor(description, ConsoleColor.DarkGray);
            }
        }

        else
        {
            foreach (var name in cmd_names)
            {
                Argument matching_cmd = FindMatchingArgument(name);
                if (matching_cmd.Names == null)
                {
                    new Error($"No information for command '{name}'");
                    continue;
                }

                PrintHelp(matching_cmd);
            }
        }
    }

    public static bool IsAskingForHelp(string arg)
    {
        string[] help_flags = { "/?", "-h", "--help", "??" };
        return help_flags.Contains(arg, StringComparer.OrdinalIgnoreCase);
    }

    public static bool IsAskingForHelp(string[] args)
    {
        string[] help_flags = { "/?", "-h", "--help", "??" };
        foreach (string arg in args)
        {
            if (help_flags.Contains(arg, StringComparer.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }
}
