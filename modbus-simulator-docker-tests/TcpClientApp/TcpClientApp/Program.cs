using System.Net;
using System.Net.Sockets;

Console.WriteLine("Hello, World!");

var ipAddress = new IPAddress(new byte[] { 192, 168, 178, 25 });
IPEndPoint ipEndPoint = new(ipAddress, 12171);

TcpClient tcpClient = new TcpClient();

try
{
    await tcpClient.ConnectAsync(ipEndPoint.Address, ipEndPoint.Port);
    Console.WriteLine($"Connected to {ipEndPoint}");
}
catch (Exception e)
{
    Console.WriteLine($"Error connecting to {ipEndPoint}: {e.Message}");
}
finally
{
    Console.ReadLine();
    tcpClient.Close();
}
