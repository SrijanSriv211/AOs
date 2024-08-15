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

        this->config.x = this->get_cursor_pos().X;
        this->config.y = this->get_cursor_pos().Y;
        this->config.i = 0;

        // init color codes
        this->color_codes = {
            { lex::STRING, console::LIGHT_YELLOW }
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

            // Check if the key combination exists in the map
            if (this->key_codes.find(key_combo) != this->key_codes.end())
                this->key_codes[key_combo]();

            else if (key.wVirtualKeyCode == VK_RETURN)
            {
                std::cout << std::endl;
                break;
            }

            else if (!std::iscntrl(key.uChar.UnicodeChar))
            {
                text_buffer.insert(this->config.i, 1, key.uChar.AsciiChar);

                this->config.i++;
                this->config.x++;
                this->suggestion_idx = 0;
            }
        }

        return lexer.tokens;
    }
}
