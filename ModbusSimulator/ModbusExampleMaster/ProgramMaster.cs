
using NModbus;
using NModbus.Extensions.Enron;
using System.Net;
using System.Net.Sockets;

public class ModbusMaster
{
    private static async Task<int> Main(string[] args)
    {
        var cts = new CancellationTokenSource();

        Console.CancelKeyPress += (sender, eventArgs) => cts.Cancel();

        try
        {
            //ModbusSocketSerialMasterReadRegisters();
            //ModbusSocketSerialMasterWriteRegisters();
            //ModbusSocketSerialMasterReadRegisters();
            await Task.Run(() => { });
            //ModbusTcpMasterReadInputs();
            //SimplePerfTest();
            //ModbusSerialRtuMasterWriteRegisters();
            //ModbusSerialAsciiMasterReadRegisters();
            //ModbusTcpMasterReadInputs();
            ModbusTcpMasterReadHoldingRegisters32();
            //StartModbusAsciiSlave();
            //ModbusTcpMasterReadInputsFromModbusSlave();
            //ModbusSerialAsciiMasterReadRegistersFromModbusSlave();
            //StartModbusTcpSlave();
            //StartModbusUdpSlave();
            //StartModbusAsciiSlave();
            //await StartModbusSerialRtuSlaveNetwork(cts.Token);
            //await StartModbusSerialRtuSlaveWithCustomMessage(cts.Token);
        }

        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();

        return 0;
    }

    /// <summary>
    ///     Simple Modbus TCP master read inputs example.
    /// </summary>
    public static void ModbusTcpMasterReadHoldingRegisters32()
    {
        using (TcpClient client = new TcpClient("127.0.0.1", 502))
        {
            var factory = new ModbusFactory();
            IModbusMaster master = factory.CreateMaster(client);

            byte slaveId = 1;
            ushort startAddress = 7165;
            ushort numInputs = 5;
            // EXAMPLE NMODBUS
            UInt32 www = 0x42c80083;
            //master.WriteSingleRegister32(slaveId, startAddress, www);


            ////EXAMPLE READCOIL
            //ushort[] registersInput = new ushort[] { 1, 0, 0 };
            //master.WriteMultipleRegisters(slaveId, startAddress, registersInput);

            //EXAMPLE  Read Discrete Inputs
            ushort[] registersInput = new ushort[] { 0, 0, 0, 0, 0x01 }; // function code / starting address / starting address / Quantity of Inputs / Quantity of Inputs


            // test
            master.WriteMultipleRegisters(slaveId, startAddress, registersInput);
            ushort[] registers = master.ReadHoldingRegisters(slaveId, startAddress, numInputs);


            ushort[] inputs = master.ReadInputRegisters(0, startAddress, numInputs);
            for (int i = 0; i < numInputs; i++)
            {
                Console.WriteLine($"Input {(startAddress + i)}={registers[i]}");
            }
        }
    }

    public static void ModbusTcpMasterReadInputs()
    {
        using (TcpClient client = new TcpClient("127.0.0.1", 502))
        {
            var factory = new ModbusFactory();
            IModbusMaster master = factory.CreateMaster(client);
              
            // read five input values
            ushort startAddress = 100;
            ushort numInputs = 5;
            bool[] inputs = master.ReadInputs(0, startAddress, numInputs);

            for (int i = 0; i < numInputs; i++)
            {
                Console.WriteLine($"Input {(startAddress + i)}={(inputs[i] ? 1 : 0)}");
            }
        }

        // output: 
        // Input 100=0
        // Input 101=0
        // Input 102=0
        // Input 103=0
        // Input 104=0
    }

    /// <summary>
    ///     Modbus TCP master and slave example.
    /// </summary>
    public static void ModbusTcpMasterReadInputsFromModbusSlave()
    {
        byte slaveId = 1;
        int port = 502;
        IPAddress address = new IPAddress(new byte[] { 127, 0, 0, 1 });

        //// create and start the TCP slave
        //TcpListener slaveTcpListener = new TcpListener(address, port);
        //slaveTcpListener.Start();

        var factory = new ModbusFactory();
        //var network = factory.CreateSlaveNetwork(slaveTcpListener);

        //IModbusSlave slave = factory.CreateSlave(slaveId);

        //network.AddSlave(slave);

        //var listenTask = network.ListenAsync();

        // create the master
        TcpClient masterTcpClient = new TcpClient(address.ToString(), port);
        IModbusMaster master = factory.CreateMaster(masterTcpClient);

        ushort numInputs = 5;
        ushort startAddress = 100;
        UInt32 www = 0x42c80083;

        // read five register values
        master.WriteSingleRegister32(0, startAddress, www);
        ushort[] inputs = master.ReadInputRegisters(0, startAddress, numInputs);

        for (int i = 0; i < numInputs; i++)
        {
            Console.WriteLine($"Register {(startAddress + i)}={(inputs[i])}");
        }

        // clean up
        masterTcpClient.Close();
        //slaveTcpListener.Stop();

        // output
        // Register 100=0
        // Register 101=0
        // Register 102=0
        // Register 103=0
        // Register 104=0
    }
}