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

    bool is_empty(const std::string& str)
    {
        return all_of(str.begin(), str.end(), ::isspace) || str.empty();
    }
}
