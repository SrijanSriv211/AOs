#pragma once

#include "console/console.h"

class lex
{
public:
    enum token_type
    {
        EOL = 0, // END OF LINE
        UNKNOWN, // 1
        COMMENT, // 2
        AMPERSAND, // 3
        WHITESPACE, // 4
        IDENTIFIER, // 5
        SEMICOLON, // 6
        INTERNAL, // 7
        STRING, // 8
        GREATER, // 9
        FLAG, // 10
        BOOL, // 11
        EXPR, // 12
        AT // 13
    };

    struct token
    {
        std::string name;
        token_type type;
    };

public:
    lex(const std::string& str, const bool& break_at_error=true, const bool& evaluate_tokens=true);
    std::map<int, console::color> get_whitepoints();

public:
    std::vector<token> tokens;
    std::string error;

private:
    void assign_token_type(const std::vector<std::string>& toks);
    std::vector<std::string> tokenizer(const std::string& str, const std::regex& re);
    std::vector<token> merge_tokens(const std::vector<token>& toks);
    std::vector<token> eval_tokens(const std::vector<token>& toks);

    bool is_valid_string(const std::string& str);
    bool any_token_type(const lex::token_type& str, const std::vector<lex::token_type>& iter);
    bool is_math_expr(const std::string& str);

    std::string create_env_filename(const std::string& filename);
    token get_env_var_val(const std::string& str);
    std::string unescape_string(const std::string& str);

private:
    bool break_at_error;
    bool evaluate_tokens;

    std::regex r_str = std::regex(R"(\"(?:\\.|[^\"\\])*\"|'(?:\\.|[^'\\])*'|`(?:\\.|[^'\\])*`)");
    std::regex r_id = std::regex(R"([-_/.a-zA-Z]+)");
    std::regex r_math = std::regex(R"(\d+(?:_\d+)*\.?\d*|[-+*/()]+?)");
    std::regex r_symbol = std::regex(R"([(),;&?@!:>])");
    std::regex r_extra = std::regex(R"([ ]+|#.*|.+)"); // comments and white spaces
};
