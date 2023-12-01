using System.Net.Sockets;
using System.Net;
using NModbus;

namespace Mock_Modbus_Master
{
    public class ModbusMaster
    {

        // this should be continuously running
        public static string Mock_WriteMultipleRegister()
        {
            using (var sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                var serverIP = IPAddress.Parse("192.168.178.25");
                var serverFullAddr = new IPEndPoint(serverIP, 80);
                TcpClient tcpClient = new TcpClient(serverFullAddr);
                //sock.Connect(serverFullAddr);
                var factory = new ModbusFactory();
                IModbusMaster master = factory.CreateMaster(tcpClient);

                byte slaveId = 0;
                ushort startAddress = 1;
                ushort[] registers = new ushort[] { 10, 20, 30 };

                // write in all the register
                master.WriteMultipleRegisters(slaveId, startAddress, registers);
                return "OK";
            }
        }

        public static void ModbusSocketSerialMasterReadRegisters()
        {
            using (var sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                var serverIP = IPAddress.Parse("192.168.178.25");
                var serverFullAddr = new IPEndPoint(serverIP, 80);
                sock.Connect(serverFullAddr);

                var factory = new ModbusFactory();
                IModbusMaster master = factory.CreateMaster(sock);

                byte slaveId = 1;
                ushort startAddress = 100;
                ushort[] registers = master.ReadHoldingRegisters(slaveId, startAddress, 3);

                // TODO: Return instead of printing
                for (int i = 0; i < 3; i++)
                {
                    Console.WriteLine($"Input {(startAddress + i)}={registers[i]}");
                }
            }
        }
    }
}
