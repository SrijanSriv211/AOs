#include "aospch.h"
#include "entrypoint.h"
#include "core/lexer/lex.h"

void exec_code(const std::string& code)
{
    lex lexer(code);

    for (int i = 0; i < lexer.tokens.size(); i++)
        std::cout << lexer.tokens[i].type << " : [" << lexer.tokens[i].name << "]\n";
}
