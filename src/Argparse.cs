using System;
using System.Linq;
using System.Collections.Generic;

class Argparse
{
    private string programName;
    private string programDescription;
    private List<Argument> arguments = new List<Argument>();

    public Argparse(string name, string description)
    {
        programName = name;
        programDescription = description;
    }

    public void AddArgument(string name, string help = "", bool required = false, string defaultValue = null)
    {
        arguments.Add(new Argument(name, help, required, defaultValue));
    }

    public Dictionary<string, string> Parse(string[] args)
    {
        var parsedArgs = new Dictionary<string, string>();

        foreach (var argument in arguments)
        {
            parsedArgs[argument.Name] = argument.DefaultValue;
        }

        List<string> freeArgs = new List<string>();

        for (int i = 0; i < args.Length; i++)
        {
            string arg = args[i];

            if (arg.StartsWith("-"))
            {
                string argName = arg.TrimStart('-');
                var argument = arguments.FirstOrDefault(a => a.MatchesName(argName));

                if (argument != null)
                {
                    if (argument.IsFlag)
                    {
                        parsedArgs[argument.Name] = "true";
                    }
                    else
                    {
                        if (i + 1 < args.Length)
                        {
                            parsedArgs[argument.Name] = args[++i];
                        }
                        else
                        {
                            throw new ArgumentException($"Missing value for argument: {arg}");
                        }
                    }
                }
                else
                {
                    throw new ArgumentException($"Invalid argument: {arg}");
                }
            }
            else
            {
                freeArgs.Add(arg);
            }
        }

        foreach (var argument in arguments)
        {
            if (argument.Required && !parsedArgs.ContainsKey(argument.Name))
            {
                throw new ArgumentException($"Missing required argument: {argument.Name}");
            }
        }

        parsedArgs["__free_args__"] = string.Join(" ", freeArgs);

        return parsedArgs;
    }

    public void PrintHelp()
    {
        Console.WriteLine($"Description:\n{programName} -> {programDescription}\n");
        Console.WriteLine($"Usage: {programName} [OPTIONS]");
        Console.WriteLine();
        Console.WriteLine("Options:");

        foreach (var argument in arguments)
        {
            string defaultValue = argument.DefaultValue != null ? $" (default: {argument.DefaultValue})" : "";
            Console.WriteLine($"-{argument.Name}: {argument.Help}{defaultValue}");
        }
    }

    private class Argument
    {
        public string Name { get; }
        public string Help { get; }
        public bool Required { get; }
        public string DefaultValue { get; }
        public bool IsFlag { get; }

        public Argument(string name, string help, bool required, string defaultValue)
        {
            Name = name;
            Help = help;
            Required = required;
            DefaultValue = defaultValue;
            IsFlag = string.IsNullOrEmpty(defaultValue);
        }

        public bool MatchesName(string argName)
        {
            return Name.Equals(argName, StringComparison.OrdinalIgnoreCase);
        }
    }
}
