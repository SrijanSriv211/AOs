#include "aopch.h"
#include "execute.h"
#include "ao.h"

#include "core/lexer/lex.h"
#include "core/settings/settings.h"

#include "strings/strings.h"
#include "console/console.h"
#include "array/array.h"

int exec_command(const lex::token& cmd, const std::vector<std::string>& args)
{
    if (get_command_func(cmd.name) != nullptr)
        get_command_func(cmd.name)();

    else if (get_cmd_args_func(cmd.name) != nullptr)
        get_cmd_args_func(cmd.name)(args);

    else if (cmd.name == "exit" || cmd.name == "_exit" || cmd.name == "-c")
        return 0; // 0 means that the user wants to exit AO

    else if (cmd.name == "reload" || cmd.name == "-r" || cmd.name == "_refresh")
        return 2; // 2 means that the user wants to reload AO

    else if (settings::get_command_by_name(cmd.name) != -1)
        settings::run_command_by_id(cmd.name, args);

    else if (std::filesystem::exists(cmd.name) && cmd.type == lex::IDENTIFIER)
        std::system((cmd.name + " " + strings::trim(strings::join("", args))).c_str());

    else if (cmd.name == ">" && cmd.type == lex::GREATER)
        std::system(strings::trim(strings::join("", args)).c_str());

    else if (cmd.type == lex::EXPR)
        std::cout << cmd.name << std::endl;

    else if (cmd.type == lex::STRING)
    {
        // trim string literals from start and end
        std::string trimmed_str = strings::trim(cmd.name, 1, 2);
        std::cout << trimmed_str << std::endl;
    }

    else
        console::errors::runtime(cmd.name, "Command not found");

    return 1;
}

int execute_tokens(const std::vector<lex::token>& tokens)
{
    const std::vector<std::vector<lex::token>> preprocessed_tokens = preprocess_tokens(tokens);
    int return_code = 1;

    for (std::vector<std::vector<lex::token>>::size_type i = 0; i < preprocessed_tokens.size(); i++)
    {
        // break tokens into `cmd` and `args` for the AO parser
        lex::token cmd = preprocessed_tokens[i].front();
        std::vector<lex::token> args = std::vector<lex::token>(preprocessed_tokens[i].begin() + 1, preprocessed_tokens[i].end());

        // _s means converted to strings
        std::vector<std::string> args_s(args.size());
        std::transform(args.begin(), args.end(), args_s.begin(), [](const lex::token& token) { return token.name; });

        return_code = exec_command(cmd, array::trim(args_s));

        if (return_code != 1)
            return return_code;
    }

    return 1; // 1 means that the user continue using AO
}

std::vector<std::vector<lex::token>> preprocess_tokens(const std::vector<lex::token>& tokens)
{
    std::vector<std::vector<lex::token>> all_tokens;
    std::vector<lex::token> subtokens;

    for (std::vector<lex::token>::size_type i = 0; i < tokens.size(); i++)
    {
        if (tokens[i].type == lex::COMMENT || tokens[i].type == lex::EOL)
        {
            if (subtokens.size() > 0)
            {
                all_tokens.push_back(subtokens);
                subtokens.clear();
            }

            break;
        }

        else if (tokens[i].type == lex::SEMICOLON)
        {
            all_tokens.push_back(subtokens);
            subtokens.clear();
        }

        else
            subtokens.push_back(tokens[i]);
    }

    return all_tokens;
}
