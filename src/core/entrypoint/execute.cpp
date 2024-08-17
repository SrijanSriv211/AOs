#include "aospch.h"
#include "entrypoint.h"

#include "core/readf/readf.h"

#include "core/lexer/lex.h"
#include "aos.h"

void exec_code(const std::string* code)
{
    // if code is null execute it in AOs shell (much like python's shell or CMD)
    if (code == nullptr)
    {
        AOs::clear_console();
        console::readf readf = console::readf({""});
        std::vector<lex::token> tokens = readf.takeinput();

        for (int i = 0; i < tokens.size(); i++)
            std::cout << tokens[i].type << " : [" << tokens[i].name << "]\n";
    }

    else
    {
        lex lexer(*code);

        for (int i = 0; i < lexer.tokens.size(); i++)
            std::cout << lexer.tokens[i].type << " : [" << lexer.tokens[i].name << "]\n";
    }
}
