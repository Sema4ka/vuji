import socket
from threading import Thread
import threading

SERVER = "127.0.0.1"
PORT = 5000

client = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
client.connect((SERVER, PORT))
client.sendall(bytes("979879789", "UTF-8"))


def task():
    while True:
        in_data = client.recv(4096)
        print("FROM SERVER :", in_data.decode())


def task2():
    while True:
        out_data = input("input: ")
        client.sendall(bytes(out_data, 'UTF-8'))
        print("SEND :" + str(out_data))


t1 = Thread(target=task2)
t2 = Thread(target=task)

t1.start()
t2.start()

t1.join()
t2.join()

