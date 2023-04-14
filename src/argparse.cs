using System;
using System.Linq;
using System.Collections.Generic;

class argparse
{
    public string[] free_args = new string[0];

    private string program_name = string.Empty;
    private string program_description = string.Empty;

    private List<Argument> arguments = new List<Argument>();
    private List<string> help_list = new List<string>();

    public argparse(string name, string description)
    {
        program_name = name;
        program_description = description;
        help_list.Add($"{program_name} -> {description}");
    }

    public void AddArgument(string name, string help = "", bool required = false, string default_value = null)
    {
        arguments.Add(new Argument(name, help, required, default_value));
    }

    // Method to parse the command line arguments and return a dictionary of parsed arguments
    public Dictionary<string, string> Parse(string[] args)
    {
        var parsed_args = new Dictionary<string, string>();

        string arg_name = string.Empty;
        string arg_val = string.Empty;

        for (int i = 0; i < args.Length; i++)
        {
            var arg = args[i];
            if (arguments.Find(a => a.Name == arg) != null)
            {
                arg_name = arg;
                if (arguments.Find(a => a.Name == arg && a.IsFlag) != null)
                    arg_val = "true";

                else if (arguments.Find(a => a.Name == arg && !a.IsFlag) != null)
                {
                    List<string> list = args.ToList();
                    int index = list.IndexOf(arg);
                    if (index != -1 && index < args.Length-1)
                        arg_val = args[index + 1];

                    else
                        new Error($"Missing value for argument: {arg}");
                }

                else
                    arg_val = "false";
            }

            else
            {
                List<string> list = free_args.ToList();
                list.Add(arg);
                free_args = list.ToArray();
            }

            // Store the parsed argument to the dict.
            if (arguments.Find(a => a.Name == arg_name) != null)
                parsed_args[arg_name] = arg_val;
        }

        foreach (var argument in arguments)
        {
            if (argument.Required && !parsed_args.ContainsKey(argument.Name))
                new Error($"Missing required argument: {argument.Name}");

            if (!parsed_args.ContainsKey(argument.Name))
                parsed_args[argument.Name] = argument.DefaultValue;
        }

        return parsed_args;
    }

    public void PrintHelp()
    {
        System.Console.WriteLine($"Description:\n{program_name} -> {program_description}\n");
        System.Console.WriteLine($"Usage: {program_name} [OPTIONS]");
        System.Console.WriteLine();
        System.Console.WriteLine("Options:");
        foreach (var argument in arguments)
        {
            var argName = $"{argument.Name}";
            var defaultValue = argument.DefaultValue != null ? $" (default: {argument.DefaultValue})" : "";
            System.Console.WriteLine($"{argName}: {argument.Help}{defaultValue}");
        }
    }

    public void GetHelp(string name="")
    {
        help_list.Sort();
        if (Collection.String.IsEmpty(name))
        {
            Console.WriteLine("Type `help <command-name>` for more information on a specific command");
            foreach (string i in help_list)
                Console.WriteLine("{0,-15} -> {1}", i.Split(' ')[0], string.Join(" ", i.Split(' ').Skip(2)));
        }

        else
        {
            var matches = help_list.Where(s => s.StartsWith(name));
            if (matches.Any())
                Console.WriteLine(string.Join("\n", matches));

            else
                new Error($"No information for command '{name}'");
        }
    }

    public static bool IsAskingForHelp(string input)
    {
        if (input == "/?" || input == "-h" || input == "--help" || input == "??") return true;
        return false;
    }

    public static bool IsAskingForHelp(string[] input)
    {
        if (input.Contains("/?") || input.Contains("-h") || input.Contains("--help") || input.Contains("??")) return true;
        return false;
    }

    private class Argument
    {
        public string Name { get; set; }
        public string Help { get; set; }
        public bool Required { get; set; }
        public string DefaultValue { get; set; }
        public bool IsFlag { get; set; }

        public Argument(string name, string help, bool required, string default_value)
        {
            Name = name;
            Help = help;
            Required = required;
            DefaultValue = default_value;
            IsFlag = (default_value == null);
        }
    }
}
