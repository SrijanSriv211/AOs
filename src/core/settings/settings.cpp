#include "aopch.h"
#include "settings.h"
#include "ao.h"

#include "strings/strings.h"
#include "console/console.h"
#include "array/array.h"

#include "nlohmann/json.hpp"

using json = nlohmann::json;

namespace settings
{
    std::string format = R"({
    "startlist": [""],
    "suggestions": ["cd", "exit", "--workspace", "cls", "_initAO", "--diagxt", "--setup", "${dirs}"],
    "commands": [
        {
            "names": [""],
            "paths": [""],
            "argpath_id": -1
        }
    ]
}
)";

    json load()
    {
        const std::string env_path = AO::ao_env_path + "\\.ao\\settings.json";
        if (!std::filesystem::exists(env_path))
            return json::parse(format);

        std::ifstream f(env_path);
        return json::parse(f);
    }

    std::vector<std::string> get_all_suggestions()
    {
        json settings = load();
        std::vector<std::string> suggestions = {};

        for (size_t i = 0; i < settings["suggestions"].size(); i++)
        {
            if (settings["suggestions"][i] == "${dirs}")
            {
                for (const auto & entry : std::filesystem::directory_iterator(std::filesystem::current_path()))
                    suggestions.push_back("\"" + entry.path().string() + "\"&");
            }

            else
                suggestions.push_back(settings["suggestions"][i]);
        }

        for (size_t i = 0; i < settings["commands"].size(); i++)
        {
            if (array::is_empty(settings["commands"][i]["names"]))
                continue;

            suggestions.insert(suggestions.end(), settings["commands"][i]["names"].begin(), settings["commands"][i]["names"].end());
        }

        return suggestions;
    }

    int get_command_by_name(const std::string& cmd)
    {
        json commands = load()["commands"];

        for (size_t i = 0; i < commands.size(); i++)
        {
            if (array::is_empty(array::trim(commands[i]["names"])))
                continue;

            if (strings::any(cmd, array::trim(commands[i]["names"])))
                return i;
        }

        return -1;
    }

    void run_command_by_id(const std::string& cmd, const std::vector<std::string>& args)
    {
        int id = get_command_by_name(cmd);
        json command = load()["commands"][id];

        int argpath_id = std::stoi(command["argpath_id"].dump());

        if (argpath_id >= static_cast<int>(command["paths"].size()) || argpath_id < -1)
        {
            console::errors::runtime("argpath_id = " + command["argpath_id"].dump(), "Invalid setting for `argpath_id` at command index " + std::to_string(id));
            return;
        }

        std::vector<std::string> paths = command["paths"];

        if (argpath_id >= 0)
        {
            paths[argpath_id] += " " + strings::join("", args);
            paths[argpath_id] = strings::trim(paths[argpath_id]);
        }

        std::system(strings::join(" && ", paths).c_str());
    }
}
