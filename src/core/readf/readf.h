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
        struct cursor_vec3
        {
            int x;
            int y;
            int i;

            cursor_vec3(int x, int y, int i=0)
            {
                this->x = x;
                this->y = y;
                this->i = i;
            }
        };

        COORD init_cursor_pos;
        int suggestion_idx;
        std::vector<std::string> suggestions_list;
        std::string text_buffer;
        std::string ren_text_buffer; // rendered text buffer
        readf::cursor_vec3 vector3 = readf::cursor_vec3(0, 0, 0);
        std::map<lex::token_type, console::color> color_codes = {};
        std::map<std::pair<WORD, DWORD>, std::function<void()>> key_codes = {};
        lex lexer = lex("");
        std::pair<int, int> diff_token_idx;

    private:
        // define helper functions
        COORD get_cursor_pos();
        void set_cursor_pos(const COORD& c);
        COORD calc_xy_coord(const int& total_dist);
        int console_window_width();
        int console_window_height();
        bool getconchar(KEY_EVENT_RECORD& krec);
        DWORD get_modifier_state(KEY_EVENT_RECORD& krec);
        void set_cursor_position(const int& total_dist);
        std::pair<int, int> get_token_diff(const std::string& text, const std::string& text2);
        int get_text_diff(const std::string& text, const std::string& text2);

        // define rendering functions
        void clear_console();
        void update_console(const bool& render_suggestions=true);
        void render_tokens();
        void render_token(const int& token_idx, const int& char_idx);

        // define key functions
        void handle_ctrl_enter();
        void handle_backspace();
    };
};
