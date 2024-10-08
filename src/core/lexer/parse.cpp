#include "aospch.h"
#include "lex.h"

#include "strings/strings.h"

void lex::parse(const std::vector<std::string>& toks)
{
    std::vector<lex::token> tokens;
    std::string tok;

    std::map<std::string, std::string> escape_chars = {
        {"\\\\", "\\"},
        {"\\\"", "\""},
        {"\\'", "'"},
        {"\\n", "\n"},
        {"\\n", "\n"},
        {"\\0", "\0"},
        {"\\t", "\t"},
        {"\\r", "\r"},
        {"\\b", "\b"},
        {"\\a", "\a"},
        {"\\f", "\f"}
    };

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

        else if (tok.starts_with("\"") || tok.starts_with("'"))
        {
            if (tok.length() == 1)
            {
                this->error = "unexpected end of tokens after " + std::string(1, tok.front());

                if (this->break_at_error)
                    break;
            }

            else if (tok.front() != tok.back())
            {
                this->error = "missing terminating " + std::string(1, tok.front()) + " character";

                if (this->break_at_error)
                    break;
            }

            for (auto const& [key, val] : escape_chars)
            {
                if (tok.find(key) != std::string::npos)
                    tok = strings::replace_all(tok, key, val);
            }

            tokens.push_back({tok, lex::STRING});
            tok.clear();
        }

        // check for math expressions
        else if (std::regex_match(tok, this->math_re))
        {
            std::string normalized_expr = strings::replace_all(tok, "_", "");

            //TODO: Implement a math engine to evaluate math exprs.
            tokens.push_back({normalized_expr, lex::EXPR});
            tok.clear();
        }

        else
        {
            lex::token_type type;
            if (tok == "true" || tok == "false")
                type = lex::BOOL;

            else if (tok.starts_with("-") || tok.starts_with("/"))
                type = lex::FLAGS;

            else if (tok.starts_with("_"))
                type = lex::HIDDEN;

            else
                type = lex::IDENTIFIER;

            tokens.push_back({tok, type});
            tok.clear();
        }
    }

    this->tokens = this->reduce_toks(tokens);
}

// reduce tokens by combining just succeeding tokens with same type into a single token
std::vector<lex::token> lex::reduce_toks(const std::vector<lex::token>& toks)
{
    std::vector<lex::token> new_tokens;
    lex::token_type tok_type;
    std::string tok_name;

    for (int i = 0; i < toks.size(); i++)
    {
        if (toks[i].type == toks[i+1].type)
        {
            tok_name += toks[i].name + toks[i+1].name;
            tok_type = toks[i].type;
            i++;
        }

        else
        {
            if (tok_name != "")
            {
                // push the previous common tokens to the array
                new_tokens.push_back({tok_name, tok_type});

                // clear the concatenated tok_name and tok_type
                tok_name.clear();
                tok_type = UNKNOWN;
            }

            // push the current tokens to the array
            new_tokens.push_back({toks[i].name, toks[i].type});
        }
    }

    return new_tokens;
}
