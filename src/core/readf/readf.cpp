#include "aospch.h"
#include "readf.h"

#include "console/console.h"

#include "core/lexer/lex.h"

namespace console
{
    readf::readf(std::vector<std::string> suggestions_list)
    {
        this->suggestion_idx = 0;
        this->suggestions_list = suggestions_list;

        this->text_buffer = "";
        this->ren_text_buffer = "";

        this->init_cursor_pos = this->get_cursor_pos();

        this->vector3.x = this->init_cursor_pos.X;
        this->vector3.y = this->init_cursor_pos.Y;
        this->vector3.i = 0;

        // init color codes
        this->color_codes = {
            { lex::STRING, console::LIGHT_YELLOW },
            { lex::EXPR, console::CYAN },
            { lex::BOOL, console::LIGHT_MAGENTA },
            { lex::SYMBOL, console::GRAY },
            { lex::COMMENT, console::GRAY },
            { lex::EOL, console::GRAY },
            { lex::HIDDEN, console::LIGHT_GREEN }
        };

        // init key codes
        this->key_codes = {
            // the construct [this]() { ... } is a lambda expression in C++.
            // lambda expression capturing 'this' to access class members.
            // [this]() captures the current object, allowing the lambda to call member functions.
            // the lambda is used here to define and associate an inline action with a key combination.
            //*NOTE: explanation by chatgpt
            { {VK_RETURN, LEFT_CTRL_PRESSED}, [this](){ this->handle_ctrl_enter(); } },
            { {VK_BACK, 0}, [this](){ this->handle_backspace(); } },
        };
    }

    std::vector<lex::token> readf::takeinput()
    {
        KEY_EVENT_RECORD key;

        while (true)
        {
            if (!this->getconchar(key))
                continue;

            std::pair<WORD, DWORD> key_combo = std::make_pair(key.wVirtualKeyCode, this->get_modifier_state(key));

            // check if the key combination exists in the map
            if (this->key_codes.find(key_combo) != this->key_codes.end())
                this->key_codes[key_combo]();

            else if (key.wVirtualKeyCode == VK_RETURN)
            {
                int total_dist = this->init_cursor_pos.X + text_buffer.length();

                COORD pos = this->calc_xy_coord(total_dist);

                // this will move the cursor to the end of the text
                vector3.x = 0;
                vector3.y += pos.Y + 1;

                std::cout << std::endl;
                break;
            }

            else if (!std::iscntrl(key.uChar.UnicodeChar))
            {
                text_buffer.insert(this->vector3.i, 1, key.uChar.AsciiChar);

                this->vector3.i++;
                this->vector3.x++;
                this->suggestion_idx = 0;

                this->update_console();
            }

            this->set_cursor_position((short)vector3.x);
        }

        return lexer.tokens;
    }

    // properly set cursor in the terminal
    void readf::set_cursor_position(const int& total_dist)
    {
        COORD pos = this->calc_xy_coord(total_dist);
        pos.Y += this->vector3.y;

        if (pos.Y >= this->console_window_height() - 1 && pos.X >= this->console_window_width() - 1)
        {
            pos.Y--;
            this->vector3.y--;
            std::cout << std::endl;
        }

        this->set_cursor_pos({pos.X, pos.Y});
    }
}
