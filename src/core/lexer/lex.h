#pragma once

class lex
{
public:
    enum token_type
    {
        EOL = 0, // END OF LINE
        UNKNOWN, // 1
        COMMENT, // 2
        WHITESPACE, // 3
        IDENTIFIER, // 4
        HIDDEN, // 5
        STRING, // 6
        SYMBOL, // 7
        FLAGS, // 8
        BOOL, // 9
        EXPR // 10
    };

    struct token
    {
        std::string name;
        token_type type;
    };

public:
    lex(const std::string& code);

public:
    std::vector<token> tokens;

private:
    void parse(const std::vector<std::string>& toks);
    std::vector<std::string> tokenizer(const std::string& str, const std::regex& re);
    std::vector<token> reduce_toks(const std::vector<token>& toks);

private:
    std::regex math_re = std::regex(R"(\(*\d+(?:[_\d]*)\)*(?:\s*[-+*/]\s*\(*\d+(?:[_\.\d]*)\)*)*)");
    std::regex str_re = std::regex(R"(\"(?:\\.|[^\"\\])*\"|'(?:\\.|[^'\\])*')");
    std::regex identifier_re = std::regex(R"([\w\d_\-.+*/]+)");
    std::regex symbol_re = std::regex(R"([(),;?@!:>]+)");
    std::regex extras_re = std::regex(R"([ ]+|#.*)"); // comments and white spaces
};
