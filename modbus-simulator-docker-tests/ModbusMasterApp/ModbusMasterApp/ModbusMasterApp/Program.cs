using NModbus;
using System.Net;
using System.Net.Sockets;

Console.WriteLine("Hello, World!");

var ipAddress = new IPAddress(new byte[] { 192, 168, 178, 25 });
IPEndPoint ipEndPoint = new(ipAddress, 16952);

using (var modbusClient = new TcpClient())
using (var modbusMaster = CreateModbusMaster(modbusClient))
{
    await modbusClient.ConnectAsync(ipAddress, 16952);
    //await modbusClient.ConnectAsync(ipAddress, 502);
    Console.WriteLine("Connection to Modbus device has been established!");
    try
    {
        // check written value in the coil Register
        var resultCoils = modbusMaster.ReadCoils(0, 0, 3);
        Console.WriteLine($"Slave detected. Result: {string.Join(", ", resultCoils)}");
        var resultCoilEquality = resultCoils.SequenceEqual(new bool[] { true, true, true });
        Console.WriteLine($"Result Holding equality: {string.Join(", ", resultCoilEquality)}");


        // check written value in holding register
        var resultHolding = modbusMaster.ReadHoldingRegisters(0, 0, 3);
        Console.WriteLine($"Result Holding: {string.Join(", ", resultHolding)}");
        var resultHoldingEquality = resultHolding.SequenceEqual(new ushort[] { 1, 2, 3 });
        Console.WriteLine($"Result Holding equality: {string.Join(", ", resultHoldingEquality)}");

        WriteInHoldingRegister(modbusMaster);
        WriteInCoildRegister(modbusMaster);

        Console.ReadKey();
    }
    catch (Exception ex)
    {
        Console.WriteLine("An error occurred: " + ex.Message);
    }
}

static async Task ConnectClient(TcpClient tcpClient, IPEndPoint ipEndPoint)
{
    await tcpClient.ConnectAsync(ipEndPoint.Address, ipEndPoint.Port);
    Console.WriteLine($"Connected to {ipEndPoint}");
}

static IModbusMaster CreateModbusMaster(TcpClient tcpClient)
{
    var factory = new ModbusFactory();
    IModbusMaster master = factory.CreateMaster(tcpClient);
    Console.WriteLine($"Modbus Master Created: {master}");
    return master;
}

static void WriteInHoldingRegister(IModbusMaster master)
{
    byte slaveId = 0;
    ushort startAddress = 0;
    ushort[] registers = new ushort[] { 10, 20, 30 };
    // write in all the register
    master.WriteMultipleRegisters(slaveId, startAddress, registers);
    Console.WriteLine("Message Wrote to multiple Register");
}

static void WriteInCoildRegister(IModbusMaster master)
{
    byte slaveId = 0;
    ushort startAddress = 0;
    bool[] registers = new bool[] { true, false, true, false };
    // write in all the register
    master.WriteMultipleCoils(slaveId, startAddress, registers);
    Console.WriteLine("Message Wrote to multiple Register");
}
