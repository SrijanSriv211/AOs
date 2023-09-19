partial class Argparse
{
    // public void PrintHelp()
    // {
    //     new TerminalColor("Name:", ConsoleColor.Yellow);
    //     Console.WriteLine($"{this.name}\n");
    //     new TerminalColor("Description:", ConsoleColor.Cyan);
    //     Console.WriteLine($"{this.desc}\n");
    //     new TerminalColor("Usage:", ConsoleColor.Blue);
    //     Console.WriteLine($"{this.name} [OPTIONS]\n");
    //     new TerminalColor("Options:", ConsoleColor.Magenta);

    //     foreach (var argument in arguments)
    //     {
    //         string argName = string.Join(", ", Utils.Array.Reduce(argument.Names));
    //         string defaultValue = argument.Default_value != null ? $" (default: {argument.Default_value})" : "";
    //         string isRequired = argument.Required != false ? $" (required: true)" : "";
    //         string isFlag = argument.Is_flag != false ? $" (is flag: true)" : "";

    //         Console.WriteLine($"{argName}: {argument.Help}{defaultValue}{isRequired}{isFlag}");
    //     }
    // }

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
        int max_padding_len = 60;
        int count = 1;

        // if (Utils.Array.IsEmpty(cmd_names))
        // {
        //     Console.WriteLine("Type `help <command-name>` for more information on a specific command");

        //     foreach (var item in help_list)
        //     {
        //         string[] command_names = item.Key;
        //         string description = item.Value;

        //         int padding = Utils.Maths.CalcPadding(count);
        //         // int padding = Math.Max(max_padding_len - (int)Math.Log10(count), 0);

        //         new TerminalColor($"{count}. ", ConsoleColor.DarkGray, false);
        //         Console.Write("{0," + -padding + "}", string.Join(", ", command_names));
        //         new TerminalColor(description, ConsoleColor.DarkGray);

        //         count++;
        //     }
        // }

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

                // bool match_found = false;
                // foreach (var item in help_list)
                // {
                //     string[] command_names = item.Key;
                //     string description = item.Value;

                //     if (command_names.Contains(name))
                //     {
                //         int padding = Math.Max(max_padding_len - (int)Math.Log10(count), 0);

                //         Console.Write("{0}. {1," + -padding + "}", count, $"{string.Join(", ", command_names)}");
                //         new TerminalColor(description, ConsoleColor.DarkGray);

                //         count++;
                //         match_found = true;
                //         break;
                //     }

                // }

                // if (match_found == false)
                //     new Error($"No information for command '{name}'");
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
