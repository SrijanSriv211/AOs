#include "aopch.h"
#include "readf.h"

#include "console/console.h"
#include "strings/strings.h"
#include "array/array.h"

#include "core/lexer/lex.h"

namespace console
{
    readf::readf(std::vector<std::string> suggestion_list)
    {
        this->suggestion_idx = 0;
        this->suggestion_list = suggestion_list;

        this->text_buffer = "";
        this->ren_text_buffer = "";

        this->init_cursor_pos = console::get_cursor_pos();

        this->vector3.x = this->init_cursor_pos.X;
        this->vector3.y = this->init_cursor_pos.Y;
        this->vector3.i = 0;

        // init color codes
        this->color_codes = {
            { lex::STRING, console::LIGHT_YELLOW },
            { lex::EXPR, console::LIGHT_CYAN },
            { lex::BOOL, console::CYAN },
            { lex::AMPERSAND, console::LIGHT_BLUE },
            { lex::AT, console::LIGHT_BLUE },
            { lex::FLAG, console::GRAY },
            { lex::GREATER, console::GRAY },
            { lex::COMMENT, console::GRAY },
            { lex::SEMICOLON, console::GRAY },
            { lex::INTERNAL, console::GREEN }
        };

        // init key codes
        this->key_codes = {
            // the construct [this]() { ... } is a lambda expression in C++.
            // lambda expression capturing 'this' to access class members.
            // [this]() captures the current object, allowing the lambda to call member functions.
            // the lambda is used here to define and associate an inline action with a key combination.
            //*NOTE: explanation by chatgpt
            { {VK_RETURN, LEFT_CTRL_PRESSED}, [this](){ this->handle_ctrl_enter(); } },
            { {VK_TAB, 0}, [this](){ this->handle_tab(); } },
            { {VK_SPACE, LEFT_CTRL_PRESSED}, [this](){ this->handle_ctrl_spacebar(); } },

            { {VK_ESCAPE, 0}, [this](){ this->handle_escape(); } },
            { {VK_ESCAPE, SHIFT_PRESSED}, [this](){ this->handle_shift_escape(); } },

            { {VK_BACK, 0}, [this](){ this->handle_backspace(); } },
            { {VK_BACK, LEFT_CTRL_PRESSED}, [this](){ this->handle_ctrl_backspace(); } },

            { {VK_DELETE, 0}, [this](){ this->handle_delete(); } },
            { {VK_DELETE, LEFT_CTRL_PRESSED}, [this](){ this->handle_ctrl_delete(); } },

            { {VK_UP, 0}, [this](){ this->handle_up_arrow(); } },
            { {VK_DOWN, 0}, [this](){ this->handle_down_arrow(); } },

            { {VK_RIGHT, 0}, [this](){ this->handle_right_arrow(); } },
            { {VK_RIGHT, LEFT_CTRL_PRESSED}, [this](){ this->handle_ctrl_right_arrow(); } },

            { {VK_LEFT, 0}, [this](){ this->handle_left_arrow(); } },
            { {VK_LEFT, LEFT_CTRL_PRESSED}, [this](){ this->handle_ctrl_left_arrow(); } },

            { {VK_END, 0}, [this](){ this->handle_end(); } },
            { {VK_HOME, 0}, [this](){ this->handle_home(); } }
        };
    }

    std::vector<lex::token> readf::takeinput()
    {
        KEY_EVENT_RECORD key;
        this->history_idx = array::is_empty(this->history_list) ? 0 : this->history_list.size();

        while (true)
        {
            if (!console::getconchar(key))
                continue;

            std::pair<WORD, DWORD> key_combo = std::make_pair(key.wVirtualKeyCode, console::get_modifier_state(key));

            // check if the key combination exists in the map
            if (this->key_codes.find(key_combo) != this->key_codes.end())
                this->key_codes[key_combo]();

            else if (key.wVirtualKeyCode == VK_RETURN)
            {
                this->lexer = lex(text_buffer, false, true);

                if (strings::is_empty(lexer.error))
                {
                    COORD pos = this->calc_xy_coord(this->init_cursor_pos.X + text_buffer.length());

                    // this will move the cursor to the end of the text
                    this->vector3.x = 0;
                    this->vector3.y += pos.Y + 1;

                    this->clear_suggestions();
                    console::set_cursor_pos({(short)this->vector3.x, (short)this->vector3.y});
                    break;
                }

                std::cout << std::endl;
                console::errors::syntax(this->lexer.error);
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

        this->lexer = lex(text_buffer, false, true);
        this->history_list.push_back(text_buffer);
        return this->lexer.tokens;
    }

    std::vector<lex::token> readf::render_text(const std::string& input)
    {
        this->text_buffer = input;
        this->update_console(false);
        std::cout << std::endl;

        this->lexer = lex(text_buffer, false, false);
        return this->lexer.tokens;
    }
}
