#pragma once

namespace strings
{
    // https://stackoverflow.com/a/313990/18121288
    char asciitolower(char in);
    std::string lowercase(const std::string& str);
    std::string join(const std::string& separator, const std::vector<std::string>& arr);
    std::string replace_all(const std::string str, const std::string& old_str, const std::string& new_str);
    bool is_empty(const std::string& str);
    bool any(const std::string& str, const std::vector<std::string>& iter, const bool& strict=false);
}
