#include "aopch.h"
#include "readf.h"

#include "console/console.h"

namespace console
{
    COORD readf::calc_xy_coord(const int& total_dist)
    {
        // calculate the exact x and y positions to put the cursor at.
        short y = total_dist / console::get_console_window_width();
        short x = total_dist - (y * console::get_console_window_width());

        return {x, y};
    }

    // properly set cursor in the terminal
    void readf::set_cursor_position(const int& total_dist)
    {
        COORD pos = this->calc_xy_coord(total_dist);
        pos.Y += this->vector3.y;

        if (pos.Y >= console::get_console_window_height() - 1 && pos.X >= console::get_console_window_width() - 1)
        {
            pos.Y--;
            this->vector3.y--;
            std::cout << std::endl;
        }

        console::set_cursor_pos({pos.X, pos.Y});
    }

    // (token_idx, char_idx)
    std::pair<int, int> readf::get_token_diff(const std::string& text, const std::string& text2)
    {
        // tokenize the updated input text
        this->lexer = lex(text, false, false);

        // tokenize the updated rendered input text
        lex ren_lexer = lex(text2, false, false);

        int smallest_token_list_len = std::min(this->lexer.tokens.size(), ren_lexer.tokens.size());

        for (int token_idx = 0; token_idx < smallest_token_list_len; token_idx++)
        {
            std::string text_token_name = this->lexer.tokens[token_idx].name;
            std::string text2_token_name = ren_lexer.tokens[token_idx].name;

            lex::token_type text_token_type = this->lexer.tokens[token_idx].type;
            lex::token_type text2_token_type = ren_lexer.tokens[token_idx].type;

            if (text_token_type != text2_token_type)
                return {token_idx, 0};

            else if (text_token_name != text2_token_name)
                return {token_idx, this->get_text_diff(text_token_name, text2_token_name)};
        }

        return {smallest_token_list_len - 1, 0};
    }

    int readf::get_text_diff(const std::string& text, const std::string& text2)
    {
        for (long long unsigned int char_idx = 0; char_idx < std::min(text.length(), text2.length()); char_idx++)
        {
            if (text[char_idx] != text2[char_idx])
                return char_idx;
        }

        if (text.length() != text2.length())
            return std::min(text.length(), text2.length());

        return 0;
    }

    void readf::clear_error_msg()
    {
        std::cout << "\n" << std::string(console::get_console_window_width(), ' ');
    }
}
