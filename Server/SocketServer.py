# -*- coding: utf-8 -*-
import socket
import threading
import JWT_func

HOST = "127.0.0.1"
PORT = 5000
CLIENTS_SOCKETS = {}
COMMAND_OPEN_CONNECT = "600"  # [КЛИЕНТ] подключение к серверу
COMMAND_INVITE = "700"  # [КЛИЕНТ] приглашение в игру
COMMAND_HAVE_INVITE = "701"  # [КЛИЕНТ], ответ на приглашение(принял/отказался) - не используется

COMMAND_OPEN_CONNECT_SERVER = "600S"  # [СЕРВЕР] отвечает на 600 запрос (хорошо/ плохо)
COMMAND_INVITE_SERVER = "700S"  # [СЕРВЕР] отвечает на 700 запрос (хорошо/ плохо)
COMMAND_HAVE_INVITE_SERVER = "701S"  # [СЕРВЕР] отправляет другому игроку приглашение


def generate_message(command, *args):
    message = f"{command}:"
    for i in args:
        message += str(i) + ":"
    return message[:-1]


def send_message_to_client(to_user_id, message):
    CLIENTS_SOCKETS[to_user_id].send(bytes(message), "UTF-8")


class Client(threading.Thread):
    def __init__(self, client_address, client_socket):
        threading.Thread.__init__(self)
        self.client_socket = client_socket
        self.client_address = client_address
        self.my_user_id = None
        self.client_online = True

        print("New connect: ", self.client_address)

    def save_info_about_client(self, message):
        token = message[1]
        self.my_user_id = JWT_func.jwt_user_id(token)
        CLIENTS_SOCKETS[self.my_user_id] = self.client_socket

    def invite_friend(self, message):
        invited_user_id = int(message[2])
        room_name = message[3]
        try:
            res = generate_message(COMMAND_HAVE_INVITE_SERVER, self.my_user_id, room_name)
            CLIENTS_SOCKETS[invited_user_id].send(bytes(res, encoding="UTF-8"))
            res = generate_message(COMMAND_INVITE_SERVER, "The user has been invited to your room")
            CLIENTS_SOCKETS[self.my_user_id].send(bytes(res, encoding="UTF-8"))
        except KeyError:
            print(f"CANT FIND USER. Users list{CLIENTS_SOCKETS}")
            res = generate_message(COMMAND_INVITE_SERVER, "User was not found in the list of connected to SocketServer")
            CLIENTS_SOCKETS[self.my_user_id].send(bytes(res, encoding="UTF-8"))

    def open_client_socket(self, message):
        self.save_info_about_client(message)
        res = generate_message(COMMAND_OPEN_CONNECT_SERVER, "You connected to SocketServer")
        CLIENTS_SOCKETS[self.my_user_id].send(bytes(res, encoding="UTF-8"))

    def run(self):
        while True:
            try:
                data = self.client_socket.recv(4096)
                message = data.decode().split(":")
                command = message[0]

                if command == COMMAND_OPEN_CONNECT:
                    self.open_client_socket(message)

                if command == COMMAND_INVITE:
                    self.invite_friend(message)

                print(f"MESSAGE: {message}")

                if message == ['']:
                    print("Disconnect: ", self.client_address)
                    CLIENTS_SOCKETS.pop(self.my_user_id)
                    break
            except:
                CLIENTS_SOCKETS.pop(self.my_user_id)
                break


# types:
# 1 - start {command:token}
# 2 - invite {command:token:invitedUserID:roomName}


def main():
    server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)

    server.bind((HOST, PORT))
    print("Server was started!")

    while True:
        server.listen(1)
        client_socket, client_address = server.accept()
        new_thread = Client(client_address, client_socket)
        new_thread.start()


if __name__ == '__main__':
    main()
