#include "aopch.h"
#include "readf.h"

#include "console/console.h"
#include "core/renderer/renderer.h"
#include "strings/strings.h"
#include "array/array.h"

namespace console
{
    void readf::update_console(const bool& to_render_suggestions)
    {
        if (this->text_buffer == this->ren_text_buffer)
            return;

        // if the cursor has reached the bottom of the window, then move it up by one point.
        // to ensure that the cursor is not going beyond the window which will crash the program.
        if (this->vector3.y >= console::get_console_window_height() - 1)
        {
            std::cout << std::endl;
            this->vector3.y--;
            this->set_cursor_position(this->vector3.x);
        }

        this->clear_console();
        this->render_tokens();

        if (!to_render_suggestions || array::is_empty(this->suggestion_list) || strings::is_empty(text_buffer))
            return;

        this->clear_suggestions();
        this->get_suitable_suggestions(12);
        this->render_suggestions();
    }

    void readf::clear_console()
    {
        // find the position where text buffer and rendered text buffer differ at.
        this->diff_token_idx = this->get_token_diff(text_buffer, ren_text_buffer);
        std::string concatenated_names;

        // concatenate names of tokens up to diff_token_idx.first
        for (int i = 0; i < diff_token_idx.first; i++)
            concatenated_names += this->lexer.tokens[i].name;

        // calculate diff_start by adding the length and diff_token_idx.second
        int diff_start = concatenated_names.length() + diff_token_idx.second;
        // get starting point of the difference to start clearing the screen from.
        int total_dist = this->init_cursor_pos.X + diff_start;

        // set cursor to the place where text buffer and rendered text buffer mismatched
        // then print whitespaces of length rendered text buffer's difference point
        // after that reset the cursor pos.
        this->set_cursor_position(total_dist);
        const int n_whitespaces = ren_text_buffer.substr(diff_start, ren_text_buffer.length()).length();
        std::cout << std::string(n_whitespaces, ' ');
        this->set_cursor_position(total_dist);
        // clear the error message log that was printed (if it was printed).
        this->clear_error_msg();
        this->set_cursor_position(total_dist);
    }

    void readf::render_token(const int& token_idx, const int& char_idx)
    {
        lex::token token = this->lexer.tokens[token_idx];

        // EOL is useless so don't render it.
        if (token.type == lex::token_type::EOL)
            return;

        std::string Token = token.name.substr(char_idx, token.name.length());
        console::color c = console::color::WHITE;

        // print those tokens in color they are associated with
        if (this->color_codes.find(token.type) != this->color_codes.end())
            c = this->color_codes[token.type];

        else if (strings::any(token_idx, whitepoints))
            c = console::color::LIGHT_WHITE;

        else
            c = console::color::WHITE;

        console::print(Token, c, false);
    }

    void readf::render_tokens()
    {
        // poop through each token starting from first different token
        render_token(diff_token_idx.first, diff_token_idx.second);
        for (std::vector<lex::token>::size_type i = diff_token_idx.first + 1; i < this->lexer.tokens.size(); i++)
            render_token(i, 0);

        this->ren_text_buffer = this->text_buffer;
    }
}
