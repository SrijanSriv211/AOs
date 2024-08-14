#pragma once

#include "argparse/argparse.h"

void unrecognized_argument_error(const std::string& err);
void init_folders();
void setup();

void exec_code(const std::string* code);
int exec_parsed_args(argparse& parser, const std::vector<argparse::parsed_argument>& parsed_args);
int take_entry(const std::vector<std::string> args);
