#include "aospch.h"
#include "strings.h"

namespace strings
{
    char asciitolower(char in)
    {
        if (in <= 'Z' && in >= 'A')
            return in - ('Z' - 'z');
        return in;
    }

    std::string lowercase(const std::string& str)
    {
        std::string result = str;
        std::transform(result.begin(), result.end(), result.begin(), [](char c) { return asciitolower(c); });
        return result;
    }

    std::string join(const std::string& separator, const std::vector<std::string>& arr)
    {
        std::string str = "";
        for (int i = 0; i < arr.size(); ++i)
        {
            str += arr[i];
            if (i < arr.size() - 1)
                str += separator;
        }

        return str;
    }

    std::string replace_all(const std::string str, const std::string& old_str, const std::string& new_str)
    {
        std::string replaced_str = str;

        while (true)
        {
            if (str.find(old_str) == std::string::npos)
                break;

            replaced_str.replace(str.find(old_str), old_str.length(), new_str);
        }

        return replaced_str;
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
}
