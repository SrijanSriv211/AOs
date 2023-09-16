partial class Parser
{
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
}
