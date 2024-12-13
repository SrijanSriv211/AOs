#include "aopch.h"
#include "lex.h"

lex::lex(const std::string& str, const bool& break_at_error, const bool& evaluate_tokens)
{
    this->break_at_error = break_at_error;
    this->evaluate_tokens = evaluate_tokens;
    std::regex re(R"(\"(?:\\.|[^\"\\])*\"|'(?:\\.|[^'\\])*'|`(?:\\.|[^'\\])*`|[_.a-zA-Z]+|\d+(?:_\d+)*\.?\d*|[-+*/()]+?|[?,!;&`'\"@>]|[ ]+|#.*|.+)");

    std::vector<std::string> toks = this->tokenizer(str, re);
    this->assign_token_type(toks);
    // push EOL in tokens just to make sure that `tokens.size()` is not zero.
    // because it will be used in rendering by `readf`.
    tokens.push_back({"", lex::EOL});
}

std::vector<std::string> lex::tokenizer(const std::string& str, const std::regex& re)
{
    std::vector<std::string> toks;

    // https://stackoverflow.com/a/73927301/18121288
    for (std::sregex_iterator it{str.begin(), str.end(), re}, end{}; it != end; ++it)
        toks.push_back(it->str());

    return toks;
}
