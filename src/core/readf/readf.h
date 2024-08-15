#pragma once

#include "console/console.h"
#include "core/lexer/lex.h"

namespace console
{
    class readf
    {
    public:
        readf(std::vector<std::string> suggestions_list);
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

        int suggestion_idx;
        std::vector<std::string> suggestions_list;
        std::string text_buffer;
        std::string ren_text_buffer; // rendered text buffer
        readf::cursor_config config = readf::cursor_config(0, 0, 0);
        std::map<lex::token_type, console::color> color_codes = {};
        std::map<std::pair<WORD, DWORD>, std::function<void()>> key_codes = {};
        lex lexer = lex("");

    private:
        bool getconchar(KEY_EVENT_RECORD& krec);
        COORD get_cursor_pos();
        void set_cursor_pos(const COORD& c);
        DWORD get_modifier_state(KEY_EVENT_RECORD& krec);

        // define rendering functions
        void clear_console();
        void update_console();
        void render_tokens();

        // define key functions
        void handle_ctrl_enter();
        void handle_backspace();
    };
};
