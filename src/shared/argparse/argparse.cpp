#include "aospch.h"
#include "argparse.h"
#include "strings/strings.h"
#include "console/console.h"

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
                missing_arg_list.push_back(argument.names[0]);
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
