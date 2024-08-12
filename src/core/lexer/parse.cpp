#include "aospch.h"
#include "lex.h"
#include "strings/strings.h"

void lex::parse(const std::vector<std::string>& toks)
{
    std::string tok;

    for (int i = 0; i < toks.size(); i++)
    {
        tok = toks[i];

        if (strings::is_empty(tok))
        {
            tokens.push_back({tok, lex::WHITESPACE});
            tok.clear();
        }

        // COMMENT
        else if (tok.starts_with('#'))
        {
            tokens.push_back({tok, lex::COMMENT});
            tok.clear();
        }

        else if (tok == ";")
        {
            tokens.push_back({tok, lex::EOL});
            tok.clear();
        }

        else if (strings::any(tok, {">", "@", "!", "?", ":"}, true))
        {
            tokens.push_back({tok, lex::SYMBOL});
            tok.clear();
        }

        else if ((tok.starts_with("\"") && tok.ends_with("\"")) || (tok.starts_with("'") && tok.ends_with("'")))
        {
            tokens.push_back({tok, lex::STRING});
            tok.clear();
        }

        // check for math expressions
        else if (std::regex_match(tok, this->math_re))
        {
            tokens.push_back({tok, lex::EXPR});
            tok.clear();
        }

        else
        {
            tokens.push_back({tok, lex::IDENTIFIER});
            tok.clear();
        }
    }
}
