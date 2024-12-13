#pragma once

#include <winsock2.h>
#include <ws2tcpip.h>

bool initialize_winsock();
SOCKET create_server_socket();
bool bind_server_socket(const SOCKET& server_socket, const std::string& ip_address, const int& port);
bool start_listening(const SOCKET& server_socket, const std::string& message, const int& backlog=10);
SOCKET accept_client(const SOCKET& server_socket);
std::string receive_data(const SOCKET& client_socket);
std::string process_input(const std::string& input_string, const std::string& status, const int& return_code);
void send_response(const SOCKET& client_socket, const std::string& response);
void cleanup(SOCKET client_socket, SOCKET server_socket);
int start_server(const std::string& ip_address, const int& port);
