#pragma once

#include "nlohmann/json.hpp"
using json = nlohmann::json;

namespace settings
{
    extern std::string format;
    json load();
    std::vector<std::string> get_all_suggestions();
    int get_command_by_name(const std::string& cmd);
    void run_command_by_id(const std::string& cmd, const std::vector<std::string>& args);
}
