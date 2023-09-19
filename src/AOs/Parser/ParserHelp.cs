partial class Parser
{
    public void PrintHelp(Command details)
    {
        string names = string.Join(", ", details.Cmd_names);
        string desc = details.Help_message;
        string default_value = (details.Default_values != null && !Utils.Array.IsEmpty(details.Default_values)) ? $" (default: {string.Join(", ", details.Default_values)})" : "";
        string is_flag = details.Is_flag == true ? $" (is flag: true)" : "";

        new TerminalColor("Name:", ConsoleColor.Cyan);
        Console.Write("{0," + -Utils.Maths.CalcPadding(1) + "}", names);
        new TerminalColor(desc + "\n", ConsoleColor.DarkGray);

        new TerminalColor("Details:", ConsoleColor.Blue);
        Console.WriteLine($"{names} [OPTIONS] {default_value}{is_flag}");
        Console.WriteLine($"Maximum arguments: {details.Max_args_length}");
        Console.WriteLine($"Minimum arguments: {details.Min_args_length}\n");

        if (!details.Is_flag && details.Supported_args != null)
        {
            int i = 1;
            new TerminalColor("Options:", ConsoleColor.Magenta);
            foreach (var supported_args in details.Supported_args)
            {
                int padding = Utils.Maths.CalcPadding(i);
                string arg_names = string.Join(", ", supported_args.Key);
                string arg_desc = supported_args.Value;

                Console.Write("{0," + -padding + "}", arg_names);
                new TerminalColor(arg_desc, ConsoleColor.DarkGray);
                i++;
            }

            Console.WriteLine();
        }
    }

    public void GetHelp(string[] cmd_names)
    {
        cmd_names = Utils.Array.Reduce(cmd_names);
        // int max_padding_len = 60;
        // int count = 1;

        if (Utils.Array.IsEmpty(cmd_names))
        {
            Console.WriteLine("Type `help <command-name>` for more information on a specific command");

            for (int i = 0; i < command_details.Count; i++)
            {
                var detail = command_details[i];

                string[] command_names = detail.Cmd_names;
                string description = detail.Help_message;
                int padding = Utils.Maths.CalcPadding(i+1);

                new TerminalColor($"{i+1}. ", ConsoleColor.DarkGray, false);
                Console.Write("{0," + -padding + "}", string.Join(", ", command_names));
                new TerminalColor(description, ConsoleColor.DarkGray);

                // count++;
            }

            // foreach (var item in help_list)
            // {
            //     string[] command_names = item.Key;
            //     string description = item.Value;
            //     int padding = Math.Max(max_padding_len - (int)Math.Log10(count), 0);

            //     new TerminalColor($"{count}. ", ConsoleColor.DarkGray, false);
            //     Console.Write("{0," + -padding + "}", string.Join(", ", command_names));
            //     new TerminalColor(description, ConsoleColor.DarkGray);

            //     count++;
            // }
        }

        else
        {
            foreach (string name in cmd_names)
            {
                Command matching_cmd = FindMatchingCommand(name);
                if (matching_cmd.Cmd_names == null)
                {
                    new Error($"No information for command '{name}'");
                    continue;
                }

                PrintHelp(matching_cmd);

                // bool match_found = false;
                // foreach (var item in help_list)
                // {
                // }
                //     string[] command_names = item.Key;
                //     string description = item.Value;

                //     if (command_names.Contains(name))
                //     {
                //         int padding = Math.Max(max_padding_len - (int)Math.Log10(count), 0);

                //         new TerminalColor($"{count}. ", ConsoleColor.DarkGray, false);
                //         Console.Write("{0," + -padding + "}", $"{string.Join(", ", command_names)}");
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
}
