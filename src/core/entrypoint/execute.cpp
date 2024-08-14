#include "aospch.h"
#include "entrypoint.h"
#include "console/readf/readf.h"
#include "console/console.h"
#include "core/lexer/lex.h"

void exec_code(const std::string* code)
{
    if (code == nullptr)
    {
        // ask user to use AOs in Windows Terminal if they are not already
        const char* env_var_val = std::getenv("WT_SESSION");
        if (env_var_val == nullptr || env_var_val[0] == '\0')
            console::print("Please use AOs in Windows Terminal for better experience.", console::color::RED);

        // enter AOs shell
        console::print("AOs 2024 [Version 2.8]", console::color::LIGHT_YELLOW, false);
        console::print("  (User)", console::color::LIGHT_WHITE);
        console::print("$ ", console::color::LIGHT_WHITE);

        std::map<lex::token_type, console::color> color_codes = {
            { lex::STRING, console::LIGHT_YELLOW }
        };
        console::readf readf = console::readf({0, 0}, {""}, color_codes);
        std::vector<lex::token> tokens = readf.takeinput();
    }

    else
    {
        lex lexer(*code);

        for (int i = 0; i < lexer.tokens.size(); i++)
            std::cout << lexer.tokens[i].type << " : [" << lexer.tokens[i].name << "]\n";
    }
}
