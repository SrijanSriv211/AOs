#include "aospch.h"
#include "argparse.h"

#include "strings/strings.h"
#include "console/console.h"
#include "array/array.h"
#include "math/math.h"

argparse::argparse(const std::string& name, const std::string& description, const std::function<void(std::string)>& error_func)
{
    this->name = name;
    this->description = description;
    this->error_func = error_func;
}

void argparse::add(const std::vector<std::string>& cmd_names, const std::string& help_message, const std::string& default_value, const bool& is_flag, const bool& required)
{
    this->arguments.emplace_back(cmd_names, help_message, default_value, is_flag, required);
}

argparse::argument argparse::find_matching_argument(const std::string& arg)
{
    for (const argument& argu : arguments)
    {
        if (std::find(argu.names.begin(), argu.names.end(), arg) != argu.names.end())
            return argu;
    }

    return argument({}, "", "", false, false);
}

std::vector<argparse::parsed_argument> argparse::parse(const std::vector<std::string>& args)
{
    std::vector<argparse::parsed_argument> parsed_args;
    const std::vector<std::string> arg_flags = { "--", "-", "/" };

    for (int i = 0; i < args.size(); i++)
    {
        std::string lowercase_arg = strings::lowercase(args[i]);
        argument matching_argument = find_matching_argument(lowercase_arg);

        // Return if no matching command was found.
        if (matching_argument.names.empty())
        {
            if (std::any_of(arg_flags.begin(), arg_flags.end(), [&](const std::string& flag) { return lowercase_arg.find(flag) == 0; }))
            {
                if (error_func)
                    error_func(args[i]);

                return {};
            }

            else
            {
                parsed_args.emplace_back(std::vector<std::string>{args[i]}, "", true, false, "unknown");
                continue;
            }
        }

        if (matching_argument.is_flag)
            parsed_args.emplace_back(matching_argument.names, "true", matching_argument.is_flag, matching_argument.required);

        else
        {
            std::string out;
            if (i == args.size() - 1)
            {
                if (matching_argument.default_value.empty())
                {
                    console::throw_error(args[i], "No argument");
                    return {};
                }

                else
                    out = matching_argument.default_value;
            }

            else
            {
                out = args[i + 1];
                ++i;
            }

            parsed_args.emplace_back(matching_argument.names, out, matching_argument.is_flag, matching_argument.required);
        }

        std::vector<std::string> missing_arg_list;
        for (const auto& argument : arguments)
        {
            if (argument.required && std::none_of(parsed_args.begin(), parsed_args.end(), [&](const parsed_argument& arg) { return arg.names == argument.names; }))
                missing_arg_list.push_back(argument.names.front());
        }

        if (!missing_arg_list.empty())
        {
            std::string str_missing_arg_list = "Missing required argument(s): " + strings::join(", ", missing_arg_list);
            console::throw_error(str_missing_arg_list, "Too few arguments");
            return {};
        }
    }

    return parsed_args;
}

void argparse::print_help()
{
    console::print("Name:", console::color::LIGHT_YELLOW);
    std::cout << this->name << "\n\n";

    console::print("Description:", console::color::LIGHT_CYAN);
    std::cout << this->description << "\n\n";

    console::print("Usage:", console::color::WHITE);
    std::cout << this->name << " [OPTIONS]" << "\n\n";

    console::print("Options:", console::color::MAGENTA);
    for (int i = 0; i < arguments.size(); i++)
    {
        argparse::argument arg = arguments[i];

        std::string arg_name = strings::join(", ", array::reduce(arg.names));
        std::string default_value = (!arg.default_value.empty()) ? " (default: " + arg.default_value + ")" : "";
        std::string is_required = (arg.required) ? " (required: true)" : " (required: false)";
        std::string is_flag = (arg.is_flag) ? " (is flag: true)" : " (is flag: false)";

        int padding = math::calc_padding(1);
        std::cout << std::setw(padding) << std::left << arg_name << "  ";
        console::print(arg.help + default_value + is_required + is_flag, console::color::GRAY);
    }

    std::cout << std::endl;
}

void argparse::print_help(const argument& details)
{
    std::string names = strings::join(", ", details.names);
    std::string description = details.help;
    std::string default_value = (!details.default_value.empty() && !strings::is_empty(details.default_value)) ? " (default: " + details.default_value + ")" : "";
    std::string is_flag = details.is_flag ? " (is flag: true)" : " (is flag: false)";
    std::string is_required = details.required ? " (required: true)" : " (required: false)";

    console::print("Name:", console::color::LIGHT_CYAN);
    int padding = math::calc_padding(1);
    std::cout << std::setw(padding) << std::left << names << "  ";
    console::print(description + "\n", console::color::GRAY);

    console::print("Details:", console::color::LIGHT_BLUE);
    std::cout << names << " [OPTIONS] " << default_value << is_flag << is_required << "\n\n";
}

void argparse::get_help(const std::vector<std::string>& cmd_names)
{
    const std::vector<std::string> names = array::reduce(cmd_names);

    if (array::is_empty(names))
    {
        std::cout << "Type `help <command-name>` for more information on a specific command" << "\n\n";

        for (int i = 0; i < arguments.size(); i++)
        {
            argparse::argument detail = arguments[i];

            std::vector<std::string> command_names = detail.names;
            std::string description = detail.help;
            int padding = math::calc_padding(i+1);

            console::print(i+1 + ". ", console::color::GRAY, false);
            std::cout << std::setw(padding) << std::left << strings::join(", ", command_names) << "  ";
            console::print(description, console::color::GRAY);
        }
    }

    else
    {
        for (const std::string name : names)
        {
            argument matching_cmd = find_matching_argument(name);
            if (matching_cmd.names.empty())
            {
                console::print("No information for command '" + name + "'", console::color::LIGHT_RED);
                continue;
            }

            print_help(matching_cmd);
        }
    }
}
