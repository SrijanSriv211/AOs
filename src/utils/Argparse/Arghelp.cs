partial class Argparse
{
    public void PrintHelp()
    {
        new TerminalColor("Name:", ConsoleColor.Yellow);
        Console.WriteLine($"{this.name}\n");
        new TerminalColor("Description:", ConsoleColor.Cyan);
        Console.WriteLine($"{this.desc}\n");
        new TerminalColor("Usage:", ConsoleColor.Blue);
        Console.WriteLine($"{this.name} [OPTIONS]\n");
        new TerminalColor("Options:", ConsoleColor.Magenta);

        foreach (var argument in arguments)
        {
            string argName = string.Join(", ", Utils.Array.Reduce(argument.Names));
            string defaultValue = argument.Default_value != null ? $" (default: {argument.Default_value})" : "";
            string isRequired = argument.Required != false ? $" (required: true)" : "";
            string isFlag = argument.Is_flag != false ? $" (is flag: true)" : "";

            Console.WriteLine($"{argName}: {argument.Help}{defaultValue}{isRequired}{isFlag}");
        }
    }

    public void GetHelp(string[] cmd_names)
    {
        cmd_names = Utils.Array.Reduce(cmd_names);
        int max_padding_len = 60;
        int count = 1;

        if (Utils.Array.IsEmpty(cmd_names))
        {
            Console.WriteLine("Type `help <command-name>` for more information on a specific command");

            foreach (var item in help_list)
            {
                string[] command_names = item.Key;
                string description = item.Value;
                int padding = Math.Max(max_padding_len - (int)Math.Log10(count), 0);

                new TerminalColor($"{count}. ", ConsoleColor.DarkGray, false);
                Console.Write("{0," + -padding + "}", string.Join(", ", command_names));
                new TerminalColor(description, ConsoleColor.DarkGray);

                count++;
            }
        }

        else
        {
            foreach (var name in cmd_names)
            {
                bool match_found = false;
                foreach (var item in help_list)
                {
                    string[] command_names = item.Key;
                    string description = item.Value;

                    if (command_names.Contains(name))
                    {
                        int padding = Math.Max(max_padding_len - (int)Math.Log10(count), 0);

                        Console.Write("{0}. {1," + -padding + "}", count, $"{string.Join(", ", command_names)}");
                        new TerminalColor(description, ConsoleColor.DarkGray);

                        count++;
                        match_found = true;
                        break;
                    }

                }

                if (match_found == false)
                    new Error($"No information for command '{name}'");
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
