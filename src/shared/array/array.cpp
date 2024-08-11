#include "aospch.h"
#include "array.h"

namespace array
{
    std::vector<std::string> reduce(const std::vector<std::string>& arr)
    {
        std::vector<std::string> result;
        std::copy_if(arr.begin(), arr.end(), std::back_inserter(result), [](const std::string& str) { return !str.empty(); });

        return result;
    }

    bool is_empty(const std::vector<std::string>& arr)
    {
        return arr.empty() || arr.size() == 0;
    }
}
