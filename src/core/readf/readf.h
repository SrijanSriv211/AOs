#pragma once

#include "console/console.h"
#include "core/lexer/lex.h"

namespace console
{
    class readf
    {
    public:
        std::vector<std::string> history_list;

    public:
        readf(std::vector<std::string> suggestion_list);
        std::vector<lex::token> takeinput();
        std::vector<lex::token> render_text(const std::string& input);

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
        std::vector<std::string>::size_type history_idx;

        std::vector<std::string>::size_type suggestion_idx;
        std::vector<std::string> suggestion_list;
        std::vector<std::string> suitable_suggestions;
        std::string suggestion;

        std::string text_buffer;
        std::string ren_text_buffer; // rendered text buffer

        readf::cursor_vec3 vector3 = readf::cursor_vec3(0, 0, 0);
        std::map<lex::token_type, console::color> color_codes = {};
        std::map<std::pair<WORD, DWORD>, std::function<void()>> key_codes = {};

        lex lexer = lex("", false, false);

        std::pair<int, int> diff_token_idx;
        std::vector<int> whitepoints = {};

    private:
        // define helper functions
        COORD calc_xy_coord(const int& total_dist);
        bool getconchar(KEY_EVENT_RECORD& krec);
        DWORD get_modifier_state(KEY_EVENT_RECORD& krec);
        void set_cursor_position(const int& total_dist);
        std::pair<int, int> get_token_diff(const std::string& text, const std::string& text2);
        int get_text_diff(const std::string& text, const std::string& text2);
        void clear_error_msg();

        // define rendering functions
        void update_console(const bool& to_render_suggestions=true);
        void clear_console();

        void render_token(const int& token_idx, const int& char_idx);
        void render_tokens();

        void clear_suggestions();
        void render_suggestions();
        void get_suitable_suggestions(const int& num_suggestions);

        // define key functions
        void handle_ctrl_enter();
        void handle_tab();
        void handle_ctrl_spacebar();
        void handle_escape();
        void handle_shift_escape();
        void handle_backspace();
        void handle_ctrl_backspace();
        void handle_delete();
        void handle_ctrl_delete();
        void handle_up_arrow();
        void handle_down_arrow();
        void handle_left_arrow();
        void handle_ctrl_left_arrow();
        void handle_right_arrow();
        void handle_ctrl_right_arrow();
        void handle_home();
        void handle_end();
    };
};
