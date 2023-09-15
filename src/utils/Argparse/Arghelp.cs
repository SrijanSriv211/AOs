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

    public void GetHelp(string name="")
    {
        help_list.Sort();
        name = name.StartsWith('_') ? name.Substring(1) : name;

        if (Utils.String.IsEmpty(name))
        {
            Console.WriteLine("Type `help <command-name>` for more information on a specific command");
            foreach (string item in help_list)
                Console.WriteLine(item);
        }

        else
        {
            if (name == ",")
            {
                Error.Syntax("Invalid syntax");
                return;
            }

            string match = "";
            foreach (string item in help_list)
            {
                if (!Utils.String.IsEmpty(match))
                {
                    Console.WriteLine(match);
                    break;
                }

                string[] parts = item.Split("->");
                for (int i = 0; i < parts.Length; i++)
                {
                    if (parts[i].Contains(name))
                    {
                        match = $"{Utils.String.Reduce(parts[i])} -> {Utils.String.Reduce(parts[i+1])}";
                        break;
                    }
                }
            }

            if (Utils.String.IsEmpty(match))
                new Error($"No information for command '{name}'");
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
