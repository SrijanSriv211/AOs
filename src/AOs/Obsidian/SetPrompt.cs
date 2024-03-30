partial class Obsidian
{
    public void SetPrompt(string[] flags)
    {
        if (Utils.Array.IsEmpty(flags))
        {
            this.prompt = "$ ";
            return;
        }

        var parser = new Argparse("prompt", "Specifies a new command prompt.", Error.UnrecognizedArgs);

        parser.Add(["-h", "--help"], "Display all supported arguments", is_flag: true);
        parser.Add(["-r", "--reset", "--restore", "--default"], "$ (dollar sign, reset the prompt)", is_flag: true);

        parser.Add(["-u", "--username"], "%username%", is_flag: true);
        parser.Add(["-s", "--space"], "(space)", is_flag: true);
        parser.Add(["-b", "--backspace"], "(backspace)", is_flag: true);
        parser.Add(["-v", "--version"], "Current AOs version", is_flag: true);

        parser.Add(["-t", "--time"], "Current time", is_flag: true);
        parser.Add(["-d", "--date"], "Current date", is_flag: true);
        parser.Add(["-p", "--path"], "Current path", is_flag: true);
        parser.Add(["-n", "--drive"], "Current drive", is_flag: true);

        var parsed_args = parser.Parse(flags);
        string new_prompt = "";

        foreach (var arg in parsed_args)
        {
            if (Argparse.IsAskingForHelp(arg.Names))
            {
                parser.PrintHelp();
                return;
            }

            else if (arg.Names.Contains("-r"))
            {
                this.prompt = "$ ";
                this.prompt_preset = [];
                break;
            }

            else if (arg.Names.Contains("-v"))
                new_prompt += version;

            else if (arg.Names.Contains("-s"))
                new_prompt += " ";

            else if (arg.Names.Contains("-b"))
                new_prompt += "\b \b";

            else if (arg.Names.Contains("-t"))
                new_prompt += DateTime.Now.ToString("HH:mm:ss");

            else if (arg.Names.Contains("-d"))
                new_prompt += DateTime.Now.ToString("dd-MM-yyyy");

            else if (arg.Names.Contains("-p"))
                new_prompt += Directory.GetCurrentDirectory();

            else if (arg.Names.Contains("-n"))
                new_prompt += Path.GetPathRoot(Environment.SystemDirectory);

            else if (arg.Names.Contains("-u"))
                new_prompt += Environment.GetEnvironmentVariable("username");

            else if (arg.Names.Any(name => name.StartsWith("-")))
            {
                this.prompt = "$ ";
                this.prompt_preset = [];
                break;
            }

            else
                new_prompt += arg.Names.First();
        }

        if (parsed_args.Count > 0)
            this.prompt = new_prompt;
    }
}
