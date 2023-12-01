using System.Net;
using System.Net.Sockets;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
        await StartTcpClient();
    }

    static async Task StartTcpClient()
    {
        var serverIP = IPAddress.Parse("192.168.178.25");
        var serverFullAddr = new IPEndPoint(serverIP, 80);
        var tcpClientTest = new TcpClient(serverFullAddr);

        while (true)
        {
            Console.WriteLine("TCP Client Loop");
            await Task.Delay(200);
        }
    }
}