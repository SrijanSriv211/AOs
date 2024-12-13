#include "aopch.h"
#include "readf.h"

#include "console/console.h"
#include "array/array.h"

namespace console
{
    void readf::handle_ctrl_enter()
    {
        if (this->text_buffer != this->suggestion)
        {
            this->text_buffer += this->suggestion;
            this->vector3.i += this->suggestion.length();
            this->vector3.x += this->suggestion.length();
        }
        this->suggestion_idx = 0;
        this->suggestion = "";

        this->handle_escape();
        this->clear_suggestions();
        this->update_console(false);
    }

    void readf::handle_tab()
    {
        if (array::is_empty(this->suitable_suggestions))
            return;

        suggestion_idx += 1;
        suggestion_idx %= this->suitable_suggestions.size();

        if (suggestion_idx < 0 || suggestion_idx > this->suitable_suggestions.size())
            suggestion_idx = 0;

        handle_ctrl_spacebar();
    }

    void readf::handle_ctrl_spacebar()
    {
        this->clear_suggestions();
        this->render_suggestions();
    }

    void readf::handle_escape()
    {
        this->suggestion_idx = 0;
        this->clear_suggestions();
    }

    // clear all the text
    void readf::handle_shift_escape()
    {
        text_buffer = "";
        this->history_idx = 0;

        this->handle_home();
        this->handle_escape();
        this->update_console(false);
    }

    void readf::handle_backspace()
    {
        if (this->vector3.i <= 0)
            return;

        this->vector3.i--;
        this->vector3.x--;
        text_buffer.erase(this->vector3.i, 1);

        this->update_console();
    }

    void readf::handle_ctrl_backspace()
    {
        if (this->vector3.i <= 0)
            return;

        const size_t whitespace_pos = text_buffer.rfind(' ', this->vector3.x - init_cursor_pos.X - 1);
        if (whitespace_pos != std::string::npos && static_cast<int>(whitespace_pos) == this->vector3.x - init_cursor_pos.X - 1)
            this->handle_backspace();

        // if cannot find previous word idx, then move to start of the str otherwise move to prev word
        const size_t previous_word_idx = text_buffer.rfind(' ', this->vector3.x - init_cursor_pos.X - 1);
        int length = previous_word_idx == std::string::npos ? this->vector3.x - init_cursor_pos.X : this->vector3.x - init_cursor_pos.X - previous_word_idx - 1;

        this->vector3.x -= length;
        this->vector3.i -= length;
        text_buffer.erase(this->vector3.x - init_cursor_pos.X, length);

        this->update_console();
    }

    void readf::handle_delete()
    {
        if (this->vector3.i >= static_cast<int>(text_buffer.length()))
            return;

        text_buffer.erase(this->vector3.x - init_cursor_pos.X, 1);
        this->update_console();
    }

    void readf::handle_ctrl_delete()
    {
        if (this->vector3.i >= static_cast<int>(text_buffer.length()))
            return;

        const size_t whitespace_pos = text_buffer.find(' ', this->vector3.x - init_cursor_pos.X);
        if (whitespace_pos != std::string::npos && static_cast<int>(whitespace_pos) == this->vector3.x - init_cursor_pos.X)
            this->handle_delete();

        // if cannot find next word idx, then move to end of the str otherwise move to next word
        const size_t next_word_idx = text_buffer.find(' ', this->vector3.x - init_cursor_pos.X);
        int length = next_word_idx == std::string::npos ? text_buffer.size() - (this->vector3.x - init_cursor_pos.X) : next_word_idx - (this->vector3.x - init_cursor_pos.X);

        text_buffer.erase(this->vector3.x - init_cursor_pos.X, length);
        this->update_console();
    }

    void readf::handle_up_arrow()
    {
        if (array::is_empty(this->history_list) || this->history_idx <= 0)
            return;

        this->history_idx--;
        this->text_buffer = this->history_list[this->history_idx];
        this->vector3.i = this->history_list[this->history_idx].size();
        this->vector3.x = this->init_cursor_pos.X + this->history_list[this->history_idx].size();
        this->suggestion = "";
        this->suggestion_idx = 0;

        if (this->vector3.y >= console::get_console_window_height() - 1)
        {
            this->vector3.y--;
            this->set_cursor_position(this->vector3.x);
            this->vector3.y++;
        }

        this->handle_escape();
        this->update_console(false);
    }

    void readf::handle_down_arrow()
    {
        if (array::is_empty(this->history_list) || this->history_idx >= this->history_list.size() - 1)
            return;

        this->history_idx++;
        this->text_buffer = this->history_list[this->history_idx];
        this->vector3.i = this->history_list[this->history_idx].size();
        this->vector3.x = this->init_cursor_pos.X + this->history_list[this->history_idx].size();
        this->suggestion = "";
        this->suggestion_idx = 0;

        if (this->vector3.y >= console::get_console_window_height() - 1)
        {
            this->vector3.y--;
            this->set_cursor_position(this->vector3.x);
            this->vector3.y++;
        }

        this->handle_escape();
        this->update_console(false);
    }

    void readf::handle_left_arrow()
    {
        if (this->vector3.i <= 0)
            return;

        this->vector3.i--;
        this->vector3.x--;
    }

    void readf::handle_ctrl_left_arrow()
    {
        if (this->vector3.i <= 0)
            return;

        const size_t whitespace_pos = text_buffer.rfind(' ', this->vector3.i - 1);
        if (whitespace_pos != std::string::npos && static_cast<int>(whitespace_pos) == this->vector3.i - 1)
            this->handle_left_arrow();

        // if cannot find previous word idx, then move to start of the str otherwise move to prev word
        const size_t previous_word_idx = text_buffer.rfind(' ', this->vector3.i - 1);
        int length = previous_word_idx == std::string::npos ? this->vector3.i : this->vector3.i - previous_word_idx - 1;

        this->vector3.x -= length;
        this->vector3.i -= length;
    }

    void readf::handle_right_arrow()
    {
        if (this->vector3.i >= static_cast<int>(text_buffer.size()))
            return;

        this->vector3.i++;
        this->vector3.x++;
    }

    void readf::handle_ctrl_right_arrow()
    {
        if (this->vector3.i >= static_cast<int>(text_buffer.size()))
            return;

        const size_t whitespace_pos = text_buffer.find(' ', this->vector3.x - init_cursor_pos.X);
        if (whitespace_pos != std::string::npos && static_cast<int>(whitespace_pos) == this->vector3.x - init_cursor_pos.X)
            this->handle_right_arrow();

        // if cannot find next word idx, then move to end of the str otherwise move to next word
        const size_t next_word_idx = text_buffer.find(' ', this->vector3.x - init_cursor_pos.X);
        int length = next_word_idx == std::string::npos ? text_buffer.size() - (this->vector3.x - init_cursor_pos.X) : next_word_idx - (this->vector3.x - init_cursor_pos.X);

        this->vector3.x += length;
        this->vector3.i += length;
    }

    void readf::handle_home()
    {
        this->vector3.x = init_cursor_pos.X;
        this->vector3.y = init_cursor_pos.Y;
        this->vector3.i = 0;
    }

    void readf::handle_end()
    {
        COORD pos = this->calc_xy_coord(this->init_cursor_pos.X + text_buffer.length());
        pos.Y += this->vector3.y; // this will move the cursor to the end of the text

        // move the cursor to the end of the text
        this->vector3.i = text_buffer.length();
        this->vector3.x = pos.X;
        this->vector3.y = pos.Y;
    }
}
