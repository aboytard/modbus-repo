# modbus-simulator-docker-tests
for the first stage and first commit :
we run firstly the server locally

in the tcp-client folder
we run the client in a docker container:
$ docker build -t tcp-client .
$ docker run -it tcp-client tcp-client 

in the modbus-master folder:
we run 
$ docker build -t modbus-master .
$ docker run -it modbus-master modbus-master 