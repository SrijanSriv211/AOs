#pragma once

namespace strings
{
    char ascii_to_lower(char in);
    std::string lowercase(const std::string& str);
    std::string join(const std::string& separator, const std::vector<std::string>& arr);
    std::string replace_all(const std::string str, const std::string& old_str, const std::string& new_str);
    std::string trim(const std::string& str, const std::string& trim_char=" \t\n\r\f\v");
    std::string trim(const std::string& str, const std::size_t& first_n, const std::size_t& last_n);
    bool is_empty(const std::string& str);
    bool any(const std::string& str, const std::vector<std::string>& iter, const bool& strict=false);
    bool any(const int& str, const std::vector<int>& iter);
    bool startswith_any(const std::string& str, const std::vector<std::string>& iter);
    bool endswith_any(const std::string& str, const std::vector<std::string>& iter);
    bool in_array(const std::string& str, const std::vector<std::string>& iter);
    bool only(const std::string& str, const std::vector<std::string>& iter);
    bool is_number(const std::string& str);
    bool is_orp(const std::string& str);
    bool contains_eachother(const std::string& str1, const std::string& str2);
}
