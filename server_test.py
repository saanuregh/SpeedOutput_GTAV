import socket
import sys

# Create a UDP socket
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

# Bind the socket to the port
server_address = ('localhost', 4915)
print('starting up on {} port {}'.format(*server_address))
sock.bind(server_address)
while True:
	data = sock.recv(25)
	if data:
	   speed = str(data, 'utf-8',errors = 'namereplace')
	   print(float(speed))
