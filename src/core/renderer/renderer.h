#pragma once

#include "core/lexer/lex.h"
#include "console/console.h"

namespace console
{
    class renderer
    {
    public:
        renderer(const std::map<lex::token_type, console::color>& token_colors, const std::map<int, console::color>& index_colors);
        void render_tokens(const std::vector<lex::token>& tokens);
        void render_token(const lex::token& token, const int& token_idx, const int& char_idx);
        void clear_console();

    public:
        std::map<lex::token_type, console::color> token_colors = {};
        std::map<int, console::color> index_colors = {};

    private:
        std::string m_rendered_text = "";
    };
}
