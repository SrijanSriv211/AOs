#include "aopch.h"
#include "server.h"
#include "ao.h"

#include "core/entrypoint/entrypoint.h"
#include "core/renderer/renderer.h"
#include "core/execute/execute.h"
#include "core/lexer/lex.h"

#include "console/console.h"
#include "strings/strings.h"
#include "nlohmann/json.hpp"

using json = nlohmann::json;

// initialize Winsock and return a boolean indicating success or failure
bool initialize_winsock()
{
    WSADATA wsaData;
    if (WSAStartup(MAKEWORD(2, 2), &wsaData) != 0)
    {
        console::print("WSAStartup failed.", console::color::LIGHT_RED);
        return false;
    }
    return true;
}

// create a server socket and return it
SOCKET create_server_socket()
{
    SOCKET server_socket = socket(AF_INET, SOCK_STREAM, 0);
    if (server_socket == INVALID_SOCKET)
    {
        console::print("Socket creation failed.", console::color::LIGHT_RED);
        WSACleanup();
        return INVALID_SOCKET;
    }
    return server_socket;
}

// bind the server socket to the specified IP and port
bool bind_server_socket(const SOCKET& server_socket, const std::string& ip_address, const int& port)
{
    sockaddr_in server_addr;
    server_addr.sin_family = AF_INET;
    server_addr.sin_addr.s_addr = inet_addr(ip_address.c_str());
    server_addr.sin_port = htons(port);

    if (bind(server_socket, (struct sockaddr*)&server_addr, sizeof(server_addr)) == SOCKET_ERROR)
    {
        console::print("Bind failed.", console::color::LIGHT_RED);
        closesocket(server_socket);
        WSACleanup();
        return false;
    }
    return true;
}

// start listening on the server socket
bool start_listening(const SOCKET& server_socket, const std::string& message, const int& backlog)
{
    if (listen(server_socket, backlog) == SOCKET_ERROR)
    {
        console::print("Listen failed.", console::color::LIGHT_RED);
        closesocket(server_socket);
        WSACleanup();
        return false;
    }

    console::print("> ", console::color::GRAY, false);
    console::print(message, console::color::LIGHT_WHITE);
    return true;
}

// accept a client connection and return the client socket
SOCKET accept_client(const SOCKET& server_socket)
{
    sockaddr_in client_addr;
    int client_addr_size = sizeof(client_addr);
    SOCKET client_socket = accept(server_socket, (struct sockaddr*)&client_addr, &client_addr_size);

    if (client_socket == INVALID_SOCKET)
    {
        console::print("Accept failed.", console::color::LIGHT_RED);
        closesocket(server_socket);
        WSACleanup();
        return INVALID_SOCKET;
    }
    return client_socket;
}

// receive data from the client
std::string receive_data(const SOCKET& client_socket)
{
    char buffer[1024];
    int recv_size = recv(client_socket, buffer, sizeof(buffer), 0);
    if (recv_size > 0)
    {
        // null-terminate the received data
        buffer[recv_size] = '\0';
        return std::string(buffer);
    }
    return "";
}

// process the received string and return a JSON response
std::string process_input(const std::string& input_string, const std::string& status, const int& return_code)
{
    json response_json;
    response_json["input"] = input_string;
    response_json["status"] = status;
    response_json["code"] = return_code;

    // convert JSON object to string
    return response_json.dump(4);
}

// send a response to the client
void send_response(const SOCKET& client_socket, const std::string& response)
{
    std::string http_response = "HTTP/1.1 200 OK\r\n"
                                "Content-Type: application/json\r\n\r\n" +
                                response;
    send(client_socket, http_response.c_str(), http_response.size(), 0);
}

// clean up the socket and Winsock
void cleanup(SOCKET client_socket, SOCKET server_socket)
{
    closesocket(client_socket);
    closesocket(server_socket);
    WSACleanup();
}

// main server loop to handle incoming connections and requests
int start_server(const std::string& ip_address, const int& port)
{
    if (!initialize_winsock())
        return -1; // -1 means error

    SOCKET server_socket = create_server_socket();
    const std::string message = "Server listening on " + ip_address + ":" + std::to_string(port);
    if (server_socket == INVALID_SOCKET || !bind_server_socket(server_socket, ip_address, port) || !start_listening(server_socket, message))
        return -1;

    int return_code = 1;
    while (return_code == 1)
    {
        SOCKET client_socket = accept_client(server_socket);
        if (client_socket == INVALID_SOCKET)
            continue;

        std::string input_string = receive_data(client_socket);
        if (!strings::is_empty(input_string))
        {
            // find the end of the HTTP headers (indicated by two consecutive CRLFs)
            size_t header_end = input_string.find("\r\n\r\n");
            // extract the body of the request
            std::string body = input_string.substr(header_end + 4); // skip past "\r\n\r\n"

            if (strings::is_empty(body))
            {
                std::string response = process_input("", "bad request", 400);
                send_response(client_socket, response);
            }

            else
            {
                AO::print_prompt();

                lex lexer = lex(body, false, false);

                std::map<lex::token_type, console::color> token_colors = {
                    { lex::STRING, console::LIGHT_YELLOW },
                    { lex::EXPR, console::LIGHT_CYAN },
                    { lex::BOOL, console::CYAN },
                    { lex::AMPERSAND, console::LIGHT_BLUE },
                    { lex::AT, console::LIGHT_BLUE },
                    { lex::FLAG, console::GRAY },
                    { lex::GREATER, console::GRAY },
                    { lex::COMMENT, console::GRAY },
                    { lex::SEMICOLON, console::GRAY },
                    { lex::INTERNAL, console::GREEN }
                };

                console::renderer renderer = console::renderer(token_colors, lexer.get_whitepoints());
                renderer.render_tokens(lexer.tokens);
                std::cout << std::endl;

                return_code = execute_tokens(lexer.tokens);

                std::string response = process_input(body, "ok", 200);
                send_response(client_socket, response);
            }
        }

        // close client socket after handling the request
        closesocket(client_socket);
    }

    // cleanup resources (only server socket needs to be cleaned up in this loop)
    cleanup(INVALID_SOCKET, server_socket);
    return return_code;
}
