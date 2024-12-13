#include "aopch.h"
#include "lex.h"

#include "strings/strings.h"
#include "console/console.h"
#include "math/math.h"

void lex::assign_token_type(const std::vector<std::string>& toks)
{
    std::vector<lex::token> tokens;
    lex::token tok = {};
    std::string str;

    for (std::vector<std::string>::size_type i = 0; i < toks.size(); i++)
    {
        str = toks[i];

        if (strings::is_empty(str))
            tok = {str, lex::WHITESPACE};

        else if (str.starts_with("#"))
            tok = {str, lex::COMMENT};

        else if (str == ";")
            tok = {str, lex::SEMICOLON};

        else if (str == "&")
            tok = {str, lex::AMPERSAND};

        else if (str == "@")
            tok = {str, lex::AT};

        else if (str == ">")
            tok = {str, lex::GREATER};

        else if (strings::any(str, {"true", "false"}, true))
            tok = {str, lex::BOOL};

        else if (strings::startswith_any(str, {"\"", "'", "`"}))
        {
            if (!this->is_valid_string(str))
                break;

            tok = {str, lex::STRING};
        }

        else if (this->is_math_expr(str))
            tok = {str, lex::EXPR};

        else
            tok = {str, lex::IDENTIFIER};

        tokens.push_back(tok);
        str.clear();
    }

    std::vector<lex::token> merged_tokens = this->merge_tokens(tokens);
    this->tokens = this->evaluate_tokens ? this->eval_tokens(merged_tokens) : merged_tokens;
}

std::vector<lex::token> lex::merge_tokens(const std::vector<lex::token>& toks)
{
    std::vector<lex::token> tokens;
    lex::token_type tok_type = lex::UNKNOWN;
    std::string tok_name = "";

    for (std::vector<lex::token>::size_type i = 0; i < toks.size(); i++)
    {
        if (this->any_token_type(toks[i].type, {lex::IDENTIFIER, lex::EXPR}) && this->any_token_type(toks[i+1].type, {lex::IDENTIFIER, lex::EXPR}))
            tok_name += strings::is_empty(tok_name) ? toks[i].name + toks[i+1].name : toks[i+1].name;

        else if (!strings::is_empty(tok_name))
        {
            if (this->is_math_expr(tok_name))
                tok_type = lex::EXPR;

            else if (tok_name.starts_with("_"))
                tok_type = lex::INTERNAL;

            else if (strings::startswith_any(tok_name, {"-", "/"}))
                tok_type = lex::FLAG;

            else
                tok_type = lex::IDENTIFIER;

            // push the previous common tokens to the array
            tokens.push_back({tok_name, tok_type});

            // clear the concatenated tok_name and tok_type
            tok_name.clear();
            tok_type = UNKNOWN;
        }

        // push the current tokens to the array
        else
        {
            if (strings::startswith_any(toks[i].name, {"-", "/"}))
                tokens.push_back({toks[i].name, lex::FLAG});

            else if (strings::startswith_any(toks[i].name, {"_"}))
                tokens.push_back({toks[i].name, lex::INTERNAL});

            else
                tokens.push_back(toks[i]);
        }
    }

    return tokens;
}

std::vector<lex::token> lex::eval_tokens(const std::vector<lex::token>& toks)
{
    std::vector<lex::token> tokens;
    lex::token tok = {};

    for (std::vector<lex::token>::size_type i = 0; i < toks.size(); i++)
    {
        // ampersand just after a string literal will mean that string is to be evaluated into a `IDENTIFIER` or an `EXPR`,
        // the string literals are used to define some ID or EXPR with spaces or other symbols which are a part of that token,
        // therefore by placing ampersand just after the ending string literal will set the string to be evaluable,
        // if the token name is not a math expression then set it as an ID.
        if (toks[i].name.size() >= 3 && toks[i].type == lex::STRING && toks[i+1].type == lex::AMPERSAND)
        {
            std::string trimmed_str = strings::trim(toks[i].name, 1, 2);
            tok = this->is_math_expr(trimmed_str) ? lex::token({math::eval(trimmed_str), lex::EXPR}) : lex::token({trimmed_str, lex::IDENTIFIER});
            i++;
        }

        // @ just after a string literal will mean that string is to be evaluated into an `EXPR`,
        // meaning the string contains a math expression and is to be evaluated.
        else if (toks[i].name.size() >= 3 && toks[i].type == lex::STRING && toks[i+1].type == lex::AT)
        {
            std::string trimmed_str = strings::trim(toks[i].name, 1, 2);
            tok = {math::eval(trimmed_str, false), lex::EXPR};
            i++;
        }

        // check if string is env var string, a env var string in AO is defined by `env_var_name`, for eg, `username` -> SrijanSrivastava
        else if (toks[i].type == lex::STRING && toks[i].name.front() == '`' && toks[i].name.back() == '`' && toks[i].name.size() >= 3)
            tok = this->get_env_var_val(toks[i].name);

        else if (toks[i].type == lex::STRING)
            tok = {this->unescape_string(toks[i].name), toks[i].type};

        else if (toks[i].type == lex::EXPR)
            tok = {math::eval(toks[i].name), toks[i].type};

        else if (toks[i].type == lex::SEMICOLON && toks[i+1].type == lex::WHITESPACE)
        {
            tok = toks[i];
            i++;
        }

        else
            tok = toks[i];

        tokens.push_back(tok);
    }

    return tokens;
}

std::map<int, console::color> lex::get_whitepoints()
{
    std::map<int, console::color> whitepoints = {{0, console::color::LIGHT_WHITE}};

    for (std::vector<lex::token>::size_type i = 0; i < tokens.size(); i++)
    {
        // if the current token is a `;` then push the next token index to whitepoints
        // if the next token is not a whitespace, otherwise move the next of next token index
        if (tokens[i].type == lex::SEMICOLON || (tokens[i].type == lex::GREATER && i == 0))
            whitepoints.insert({tokens[i+1].type == lex::WHITESPACE ? i+2 : i+1, console::color::LIGHT_WHITE});
    }

    return whitepoints;
}
