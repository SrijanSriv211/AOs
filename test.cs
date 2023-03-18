using System;
using System.Collections.Generic;

class ArgumentParser
{
    private string _programName;
    private List<Argument> _arguments = new List<Argument>();

    public ArgumentParser(string programName)
    {
        _programName = programName;
    }

    public void AddArgument(string name, string help = "", bool required = false, string defaultValue = null)
    {
        _arguments.Add(new Argument(name, help, required, defaultValue));
    }

    public Dictionary<string, string> Parse(string[] args)
    {
        var parsedArgs = new Dictionary<string, string>();
        var i = 0;

        while (i < args.Length)
        {
            var arg = args[i];
            var argName = arg.StartsWith("--") ? arg.Substring(2) : arg.Substring(1);
            var argValue = "";

            if (arg.StartsWith("--"))
            {
                i++;

                if (i < args.Length && !args[i].StartsWith("-"))
                {
                    argValue = args[i];
                }
                else
                {
                    if (_arguments.Find(a => a.Name == argName && a.IsFlag) != null)
                    {
                        argValue = "true";
                    }
                    else
                    {
                        throw new ArgumentException($"Missing value for argument: {arg}");
                    }
                }
            }
            else
            {
                if (_arguments.Find(a => a.Name == argName && a.IsFlag) != null)
                {
                    argValue = "true";
                }
                else if (arg.Length > 2)
                {
                    argValue = arg.Substring(2);
                }
                else
                {
                    i++;

                    if (i < args.Length && !args[i].StartsWith("-"))
                    {
                        argValue = args[i];
                    }
                    else
                    {
                        throw new ArgumentException($"Missing value for argument: {arg}");
                    }
                }
            }

            var argument = _arguments.Find(a => a.Name == argName);
            if (argument != null)
            {
                parsedArgs[argName] = argValue;
            }
            else
            {
                throw new ArgumentException($"Unknown argument: {arg}");
            }

            i++;
        }

        foreach (var argument in _arguments)
        {
            if (argument.Required && !parsedArgs.ContainsKey(argument.Name))
            {
                throw new ArgumentException($"Missing required argument: {argument.Name}");
            }

            if (!parsedArgs.ContainsKey(argument.Name))
            {
                parsedArgs[argument.Name] = argument.DefaultValue;
            }
        }

        return parsedArgs;
    }

    public void PrintHelp()
    {
        Console.WriteLine($"Usage: {_programName} [OPTIONS]");
        Console.WriteLine();
        Console.WriteLine("Options:");
        foreach (var argument in _arguments)
        {
            var argName = argument.IsFlag ? $"--{argument.Name}" : $"-{argument.Name}";
            var defaultValue = argument.DefaultValue != null ? $" (default: {argument.DefaultValue})" : "";
            Console.WriteLine($"  {argName}: {argument.Help}{defaultValue}");
        }
    }

    private class Argument
    {
        public string Name { get; set; }
        public string Help { get; set; }
        public bool Required { get; set; }
        public string DefaultValue { get; set; }
        public bool IsFlag { get; set; }

        public Argument(string name, string help, bool required, string defaultValue)
        {
            Name = name;
            Help = help;
            Required = required;
            DefaultValue = defaultValue;
            IsFlag = (defaultValue == null);
        }
    }
}
