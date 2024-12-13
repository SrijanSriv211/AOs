#include "aopch.h"
#include "renderer.h"

#include "core/lexer/lex.h"
#include "console/console.h"
#include "strings/strings.h"

namespace console
{
    // `tokens_colors`: colorize tokens based on the token type
    // `index_colors`: colorize tokens based on their index
    renderer::renderer(const std::map<lex::token_type, console::color>& token_colors, const std::map<int, console::color>& index_colors)
    {
        this->token_colors = token_colors;
        this->index_colors = index_colors;
        this->m_rendered_text = "";
    }

    void renderer::clear_console()
    {
        const int n_whitespaces = this->m_rendered_text.length();
        std::cout << std::string(n_whitespaces, ' ');
    }

    void renderer::render_tokens(const std::vector<lex::token>& tokens)
    {
        for (std::vector<lex::token>::size_type i = 0; i < tokens.size(); i++)
            render_token(tokens[i], i, 0);
    }

    // render a single token
    // token_idx will help the render to colorize the token using index_colors
    // char_idx will allow to print the token starting from a particular index
    void renderer::render_token(const lex::token& token, const int& token_idx, const int& char_idx)
    {
        // EOL is useless so don't render it.
        if (token.type == lex::token_type::EOL)
            return;

        console::color color = console::color::WHITE;

        // print the token in color it is associated with
        if (this->token_colors.find(token.type) != token_colors.end())
            color = this->token_colors[token.type];

        else if (this->index_colors.find(token_idx) != index_colors.end())
            color = this->index_colors[token_idx];

        std::string token_name = token.name.substr(char_idx, token.name.length());
        console::print(token_name, color, false);
        this->m_rendered_text += token_name;
    }
}
