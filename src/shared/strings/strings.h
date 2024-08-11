#pragma once

namespace strings
{
    // https://stackoverflow.com/a/313990/18121288
    char asciitolower(char in);
    std::string lowercase(const std::string& str);
    std::string join(const std::string& separator, const std::vector<std::string>& arr);
    bool is_empty(const std::string& str);
};
