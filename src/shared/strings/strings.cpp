#include "aopch.h"
#include "strings.h"

namespace strings
{
    // https://stackoverflow.com/a/313990/18121288
    char ascii_to_lower(char in)
    {
        if (in <= 'Z' && in >= 'A')
            return in - ('Z' - 'z');
        return in;
    }

    std::string lowercase(const std::string& str)
    {
        std::string result = str;
        std::transform(result.begin(), result.end(), result.begin(), [](char c) { return ascii_to_lower(c); });
        return result;
    }

    std::string join(const std::string& separator, const std::vector<std::string>& arr)
    {
        std::string str = "";
        for (std::vector<std::string>::size_type i = 0; i < arr.size(); ++i)
        {
            str += arr[i];
            if (i < arr.size() - 1)
                str += separator;
        }

        return str;
    }

    // https://stackoverflow.com/a/29752943/18121288
    std::string replace_all(const std::string str, const std::string& old_str, const std::string& new_str)
    {
        std::string new_string;
        new_string.reserve(str.length()); // avoids a few memory allocations

        std::string::size_type last_pos = 0;
        std::string::size_type find_pos;

        while(std::string::npos != (find_pos = str.find(old_str, last_pos)))
        {
            new_string.append(str, last_pos, find_pos - last_pos);
            new_string += new_str;
            last_pos = find_pos + old_str.length();
        }

        // care for the rest after last occurrence
        new_string += str.substr(last_pos);
        return new_string;
    }

    std::string trim(const std::string& str, const std::string& trim_char)
    {
        // find the first non-trim_char character
        const std::string::size_type str_begin = str.find_first_not_of(trim_char);

        if (str_begin == std::string::npos)
            return str;

        // find the last non-trim_char character
        const std::string::size_type str_end = str.find_last_not_of(trim_char);
        const std::string::size_type str_range = str_end - str_begin + 1;

        // return the substring without leading and trailing trim_char
        return str.substr(str_begin, str_range);
    }

    // `first_n` - first 'n' chars of the str
    // `last_n` - last 'n' chars of the str
    std::string trim(const std::string& str, const std::size_t& first_n, const std::size_t& last_n)
    {
        return str.substr(first_n, str.size() - last_n);
    }

    bool is_empty(const std::string& str)
    {
        return all_of(str.begin(), str.end(), ::isspace) || str.empty();
    }

    // return true if any string from an iterable is present in target string
    // `strict` argument checks if any string from the iterable is equal to the target string
    bool any(const std::string& str, const std::vector<std::string>& iter, const bool& strict)
    {
        for (const std::string& i : iter)
        {
            if (strict && str == i)
                return true;

            else if (!strict && str.find(i) != std::string::npos)
                return true;
        }

        return false;
    }

    bool any(const int& str, const std::vector<int>& iter)
    {
        for (const int& i : iter)
        {
            if (str == i)
                return true;
        }

        return false;
    }

    // return true if any string from an iterable startswith the target string
    bool startswith_any(const std::string& str, const std::vector<std::string>& iter)
    {
        for (const std::string& i : iter)
        {
            if (str.starts_with(i))
                return true;
        }

        return false;
    }

    // return true if any string from an iterable endswith the target string
    bool endswith_any(const std::string& str, const std::vector<std::string>& iter)
    {
        for (const std::string& i : iter)
        {
            if (str.ends_with(i))
                return true;
        }

        return false;
    }

    bool in_array(const std::string& str, const std::vector<std::string>& iter)
    {
        return std::find(iter.begin(), iter.end(), str) != iter.end();
    }

    // return false if any `str` is found not to be present in the iter
    bool only(const std::string& str, const std::vector<std::string>& iter)
    {
        for (std::vector<std::string>::size_type i = 0; i < str.size(); i++)
        {
            if (!in_array(std::string(1, str[i]), iter))
                return false;
        }

        return true;
    }

    bool is_number(const std::string& str)
    {
        return only(str, {"1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "_", "."});
    }

    bool is_orp(const std::string& str)
    {
        return only(str, {"-", "+", "*", "/", "(", ")"});
    }

    bool contains_eachother(const std::string& str1, const std::string& str2)
    {
        if (is_empty(str1) || is_empty(str2))
            return false;

        const size_t x = str1.find(str2);
        const size_t y = str2.find(str1);

        if (x == std::string::npos && y == std::string::npos)
            return false;

        return true;
    }
}
