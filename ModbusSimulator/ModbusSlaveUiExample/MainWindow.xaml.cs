using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ModbusSlaveUiExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ModbusSlave _slave;
        private Task _listenTask;
        private TcpListener _listener;
        private TcpClient _client;
        private NetworkStream _stream;

        public MainWindow()
        {
            InitializeComponent();

            // Create the Modbus TCP slave
            ModbusFactory factory = new ModbusFactory();
            _slave = factory.CreateSlave(1);
            _slave.DataStore = DataStoreFactory.CreateDefaultDataStore();

            // Start listening for Modbus TCP requests
            _listener = new TcpListener(IPAddress.Any, 502);
            _listener.Start();
            _listenTask = ListenForRequests();

            // Update the UI with any messages received by the slave
            _slave.ModbusSlaveRequestReceived += (sender, args) =>
            {
                Dispatcher.Invoke(() =>
                {
                    MessageTextBox.Text += $"{DateTime.Now} - Message received: {args.Message.FunctionCode} {args.Message.ReadRequest.Address} {args.Message.ReadRequest.Count}\n";
                });
            };
        }

        private async Task ListenForRequests()
        {
            while (true)
            {
                _client = await _listener.AcceptTcpClientAsync();
                _stream = _client.GetStream();
                await _slave.ListenAsync(_stream);
            }
        }

        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            // Send a hardcoded message from the slave to the master
            ushort[] values = new ushort[] { 1, 2, 3, 4 };
            _slave.DataStore.HoldingRegisters.WriteRange(0, values);
            byte[] message = new byte[] { 1, 16, 0, 0, 0, 4, 8, 1, 2, 3, 4 };
            _stream.Write(message, 0, message.Length);
            MessageTextBox.Text += $"{DateTime.Now} - Message sent: {message[1]} {message[2]} {message[3]} {message[4]} {message[5]} {message[6]} {message[7]} {message[8]} {message[9]} {message[10]}\n";
        }
    }
}
