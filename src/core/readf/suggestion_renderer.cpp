#include "aopch.h"
#include "readf.h"

#include "console/console.h"
#include "strings/strings.h"
#include "array/array.h"

namespace console
{
    void readf::clear_suggestions()
    {
        if (array::is_empty(this->suitable_suggestions))
            return;

        std::string suitable_suggestions_s = strings::join("    ", this->suitable_suggestions); // _s means converted to string
        std::cout << "\n" + std::string(suitable_suggestions_s.size(), ' ');
        this->set_cursor_position(this->vector3.x);
    }

    void readf::render_suggestions()
    {
        if (strings::is_empty(this->text_buffer))
            return;

        std::cout << std::endl;
        console::color clr = console::color::GRAY;
        for (std::vector<std::string>::size_type i = 0; i < this->suitable_suggestions.size(); i++)
        {
            if (i == this->suggestion_idx)
            {
                suggestion = suitable_suggestions[i];
                clr = console::color::LIGHT_BLUE;
            }

            else
                clr = console::color::GRAY;

            console::print(this->suitable_suggestions[i], clr, false);
            std::cout << "    ";
        }
    }

    void readf::get_suitable_suggestions(const int& num_suggestions)
    {
        std::vector<std::string> suitable_suggestions_t = {}; // _t means temp
        this->suitable_suggestions = {};

        for (std::vector<std::string>::size_type i = 0; i < suggestion_list.size(); i++)
        {
            if (strings::contains_eachother(text_buffer, this->suggestion_list[i]))
                suitable_suggestions_t.push_back(this->suggestion_list[i]);
        }

        int slice_size = std::min(num_suggestions, static_cast<int>(suitable_suggestions_t.size()));

        if (slice_size < 1)
            return;

        this->suitable_suggestions = std::vector(suitable_suggestions_t.begin(), suitable_suggestions_t.begin() + slice_size);
    }
}
