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
        STRING, // 5
        SYMBOL, // 6
        FLAGS, // 7
        BOOL, // 8
        EXPR // 9
    };

    struct token
    {
        std::string name;
        token_type type;
    };

public:
    lex(const std::string& code);

private:
    std::vector<std::string> tokenizer(const std::string& str, const std::regex& re);
    void parse(const std::vector<std::string>& toks);

private:
    std::vector<token> tokens;
    std::regex math_re = std::regex(R"(\(*\d+(?:[_\d]*)\)*(?:\s*[-+*/]\s*\(*\d+(?:[_\.\d]*)\)*)*)");
    std::regex str_re = std::regex(R"(\"(?:\\.|[^\"\\])*\"|'(?:\\.|[^'\\])*')");
    std::regex identifier_re = std::regex(R"([\w\d_\-.+*/]+)");
    std::regex symbol_re = std::regex(R"([(),;?@!:>]+)");
    std::regex extras_re = std::regex(R"([ ]+|#.*)"); // comments and white spaces
};
