// See https://aka.ms/new-console-template for more information
using NModbus;
using NModbus.Data;
using System.Net;
using System.Net.Sockets;

Console.WriteLine("Start");


var ipAddress = new IPAddress(new byte[] { 192, 168, 178, 25 });

TcpListener _tcpListener = new TcpListener(ipAddress, 16952);
_tcpListener.Start();

IModbusFactory factory = new ModbusFactory();
IModbusSlaveNetwork network = factory.CreateSlaveNetwork(_tcpListener);

// maybe this should be coming from there
var dataStore = new SlaveDataStore();

//Log the operations for debugging purposes.
dataStore.HoldingRegisters.BeforeRead += (sender, args) => Console.WriteLine("HoldingRegister(s)", args);
dataStore.InputRegisters.BeforeRead += (sender, args) => Console.WriteLine("InputRegister(s)", args);
dataStore.CoilDiscretes.BeforeRead += (sender, args) => Console.WriteLine("CoilDiscrete(s)", args);
dataStore.CoilInputs.BeforeRead += (sender, args) => Console.WriteLine("CoilInput(s)", args);

var device = factory.CreateSlave(0, dataStore);
CancellationToken cts = new CancellationToken();

Console.WriteLine("Listen");
await network.ListenAsync(cts);
