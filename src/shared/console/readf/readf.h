#pragma once

#include "console/console.h"
#include "core/lexer/lex.h"

namespace console
{
    class readf
    {
    public:
        readf(std::vector<int> cursor_start_pos, std::vector<std::string> suggestions_list, std::map<lex::token_type, console::color> color_codes);
        std::vector<lex::token> takeinput();

    private:
        struct cursor_config
        {
            int x;
            int y;
            int i;

            cursor_config(int x, int y, int i=0)
            {
                this->x = x;
                this->y = y;
                this->i = i;
            }
        };

        std::string text_buffer;
        readf::cursor_config config = readf::cursor_config(0, 0, 0);

    private:
        bool getconchar(KEY_EVENT_RECORD &krec);
    };
};
