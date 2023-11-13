partial class Argparse
{
    public void PrintHelp()
    {
        TerminalColor.Print("Name:", ConsoleColor.Yellow);
        Console.WriteLine($"{this.name}\n");
        TerminalColor.Print("Description:", ConsoleColor.Cyan);
        Console.WriteLine($"{this.desc}\n");
        TerminalColor.Print("Usage:", ConsoleColor.Blue);
        Console.WriteLine($"{this.name} [OPTIONS]\n");
        TerminalColor.Print("Options:", ConsoleColor.Magenta);

        foreach (var argument in arguments)
        {
            string argName = string.Join(", ", Utils.Array.Reduce(argument.Names));
            string defaultValue = argument.Default_value != null ? $" (default: {argument.Default_value})" : "";
            string isRequired = argument.Required != false ? $" (required: true)" : "";
            string isFlag = argument.Is_flag != false ? $" (is flag: true)" : "";

            Console.WriteLine($"{argName}: {argument.Help}{defaultValue}{isRequired}{isFlag}");
        }

        Console.WriteLine();
    }

    public void PrintHelp(Argument details)
    {
        string names = string.Join(", ", details.Names);
        string desc = details.Help;
        string default_value = (details.Default_value != null && !Utils.String.IsEmpty(details.Default_value)) ? $" (default: {details.Default_value})" : "";
        string is_flag = details.Is_flag == true ? $" (is flag: true)" : "";
        string is_required = details.Required != false ? $" (required: true)" : "";

        TerminalColor.Print("Name:", ConsoleColor.Cyan);
        Console.Write("{0," + -Utils.Maths.CalculatePadding(1) + "}", names);
        TerminalColor.Print(desc + "\n", ConsoleColor.DarkGray);

        TerminalColor.Print("Details:", ConsoleColor.Blue);
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
                int padding = Utils.Maths.CalculatePadding(i+1);

                TerminalColor.Print($"{i+1}. ", ConsoleColor.DarkGray, false);
                Console.Write("{0," + -padding + "}", string.Join(", ", command_names));
                TerminalColor.Print(description, ConsoleColor.DarkGray);
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
