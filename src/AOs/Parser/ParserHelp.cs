partial class Parser
{
    public void PrintHelp(Command details)
    {
        string names = string.Join(", ", details.Cmd_names);
        string desc = details.Help_message;
        string default_value = (details.Default_values != null && !Utils.Array.IsEmpty(details.Default_values)) ? $" (default: {string.Join(", ", details.Default_values)})" : "";
        string is_flag = details.Is_flag == true ? $" (is flag: true)" : "";
        string max_args_len = details.Max_args_length == 0 ? "Maximum arguments: ∞" : $"Maximum arguments: {details.Max_args_length}";
        string min_args_len = details.Min_args_length == -1 ? "Minimum arguments: ∞" : $"Minimum arguments: {details.Max_args_length}";

        Terminal.Print("Name:", ConsoleColor.Cyan);
        Terminal.Print(string.Format("{0," + -Utils.Maths.CalculatePadding(1) + "}", names), is_newline: false);
        Terminal.Print(desc + "\n", ConsoleColor.DarkGray);

        Terminal.Print("Details:", ConsoleColor.Blue);
        Terminal.Print($"{names} {(details.Is_flag ? "" : "[OPTIONS]")} {default_value}{is_flag}");
        Console.WriteLine();

        if (!details.Is_flag)
        {
            Terminal.Print(max_args_len);
            Terminal.Print(min_args_len);
            Console.WriteLine();

            if (details.Supported_args != null)
            {
                int i = 1;
                Terminal.Print("Options:", ConsoleColor.Magenta);
                foreach (var supported_args in details.Supported_args)
                {
                    string arg_names = string.Join(", ", supported_args.Key);
                    string arg_desc = supported_args.Value;

                    Terminal.Print(string.Format("{0," + -Utils.Maths.CalculatePadding(i) + "}", arg_names), is_newline: false);
                    Terminal.Print(arg_desc, ConsoleColor.DarkGray);
                    i++;
                }

                Console.WriteLine();
            }

            if (details.Usage != null)
            {
                Terminal.Print("Usage:", ConsoleColor.Green);
                Console.WriteLine(string.Join("\n", details.Usage));
                Console.WriteLine();
            }
        }
    }

    public void GetHelp(string[] cmd_names)
    {
        cmd_names = Utils.Array.Reduce(cmd_names);

        if (Utils.Array.IsEmpty(cmd_names))
        {
            Console.WriteLine("Type `help <command-name>` for more information on a specific command");

            int count = 1;
            for (int i = 0; i < commands.Count; i++)
            {
                var detail = commands[i];

                if (detail.Do_index)
                {
                    string command_names = string.Join(", ", detail.Cmd_names);
                    string description = detail.Help_message;

                    Terminal.Print($"{count}. ", ConsoleColor.DarkGray, false);
                    Terminal.Print(string.Format("{0," + -Utils.Maths.CalculatePadding(count) + "}", command_names), is_newline: false);
                    Terminal.Print(description, ConsoleColor.DarkGray);

                    count++;
                }
            }
        }

        else
        {
            foreach (string name in cmd_names)
            {
                Command matching_cmd = FindMatchingCommand(name, only_indexed: true);
                if (matching_cmd.Cmd_names == null)
                {
                    new Error($"No information for command '{name}'", "parser error");
                    continue;
                }

                PrintHelp(matching_cmd);
            }
        }
    }
}
