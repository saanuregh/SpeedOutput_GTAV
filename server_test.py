import bluelet
def echoer(conn):
    while True:
        data = yield conn.recv(25)
        if data:
            speed = str(data, 'utf-8',errors = 'namereplace')[11:23]
            print(float(speed))

bluelet.run(bluelet.server('localhost', 4915, echoer))