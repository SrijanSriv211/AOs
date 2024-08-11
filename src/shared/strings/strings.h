#pragma once

class strings
{
public:
    // https://stackoverflow.com/a/313990/18121288
    static char asciitolower(char in);
    static std::string lowercase(const std::string& str);
    static std::string join(const std::string& separator, const std::vector<std::string>& arr);
};
