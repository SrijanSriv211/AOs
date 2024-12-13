#pragma once

#include "core/lexer/lex.h"

std::vector<std::vector<lex::token>> preprocess_tokens(const std::vector<lex::token>& tokens);
std::function<void()> get_command_func(const std::string& cmd);
std::function<void(const std::vector<std::string>&)> get_cmd_args_func(const std::string& cmd);
int exec_command(const lex::token& cmd, const std::vector<std::string>& args);
int execute_tokens(const std::vector<lex::token>& tokens);
