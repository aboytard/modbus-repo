using System.Net.Sockets;
using System.Net;
using NModbus;
using ModbusExampleSlave;
using System.Collections;

public class ProgramTestModbusSlave
{
    private static async Task<int> Main(string[] args)
    {
        var cts = new CancellationTokenSource();
        Console.CancelKeyPress += (sender, eventArgs) => cts.Cancel();
        IDictionary<byte, IModbusFunctionService> functionServices = new Dictionary<byte, IModbusFunctionService>() {};

        try
        {
            StartModbusTcpSlave();
        }

        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();

        return 0;
    }

    public static void StartModbusTcpSlave()
    {
        int port = 502;
        IPAddress address = new IPAddress(new byte[] { 127, 0, 0, 1 });

        // create and start the TCP slave
        TcpListener slaveTcpListener = new TcpListener(address, port);
        slaveTcpListener.Start();

        IModbusFactory factory = new ModbusFactory();

        // the network is wrapping the server
        IModbusSlaveNetwork network = factory.CreateSlaveNetwork(slaveTcpListener);

        var slave0DataStore = new SlaveStorage();
        slave0DataStore.InputRegisters.StorageOperationOccurred += (sender, args) => Console.WriteLine($"slave1 Input registers: {args.Operation} starting at {args.StartingAddress}");
        slave0DataStore.HoldingRegisters.StorageOperationOccurred += (sender, args) => Console.WriteLine($"slave1 Holding registers: {args.Operation} starting at {args.StartingAddress}");

        var slave1DataStore = new SlaveStorage();
        slave1DataStore.InputRegisters.StorageOperationOccurred += (sender, args) => Console.WriteLine($"slave1 Input registers: {args.Operation} starting at {args.StartingAddress}");
        slave1DataStore.HoldingRegisters.StorageOperationOccurred += (sender, args) => Console.WriteLine($"slave1 Holding registers: {args.Operation} starting at {args.StartingAddress}");

        var slave2DataStore = new SlaveStorage();
        slave2DataStore.InputRegisters.StorageOperationOccurred += (sender, args) => Console.WriteLine($"slave1 Input registers: {args.Operation} starting at {args.StartingAddress}");
        slave2DataStore.HoldingRegisters.StorageOperationOccurred += (sender, args) => Console.WriteLine($"slave1 Holding registers: {args.Operation} starting at {args.StartingAddress}");

        // create a handler for the handler to also have the possibility to write a message

        //ModbusSlave slave0 = new ModbusSlave(0, slave0DataStore, GetAllFunctionServices(functionServices));
        IModbusSlave slave0 = factory.CreateSlave(0, slave0DataStore);
        IModbusSlave slave1 = factory.CreateSlave(1, slave1DataStore);
        IModbusSlave slave2 = factory.CreateSlave(2, slave2DataStore);

        // try to attach events directly to the slave
        //NModbus.Device.

        network.AddSlave(slave0);
        network.AddSlave(slave1);
        network.AddSlave(slave2);

        Console.WriteLine("Start the slave Network on the tcpListener side");

        // here I should put this to an event
        network.ListenAsync().GetAwaiter().GetResult();

        // prevent the main thread from exiting
        Thread.Sleep(Timeout.Infinite);
    }

    public IModbusFunctionService[] GetAllFunctionServices(IDictionary<byte, IModbusFunctionService> functionServices)
    {
        return functionServices
            .Values
            .ToArray();
    }
}