#include "aospch.h"
#include "lex.h"

lex::lex(const std::string& code, const bool& break_at_error)
{
    this->break_at_error = break_at_error;
    std::regex re(R"(\(*\d+(?:[_\d]*)\)*(?:\s*[-+*/]\s*\(*\d+(?:[_\.\d]*)\)*)*|\"(?:\\.|[^\"\\])*\"|'(?:\\.|[^'\\])*'|[\w\d_\-.+*/]+|[(),;?@!:>]|[ ]+|#.*|.+)");

    std::vector<std::string> toks = this->tokenizer(code, re);
    this->parse(toks);
}

std::vector<std::string> lex::tokenizer(const std::string& str, const std::regex& re)
{
    std::vector<std::string> toks;

    // https://stackoverflow.com/a/73927301/18121288
    for (std::sregex_iterator it{str.begin(), str.end(), re}, end{}; it != end; ++it)
        toks.push_back(it->str());

    return toks;
}
