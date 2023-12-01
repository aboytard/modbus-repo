using SharedLibrary;
using StackLightSimulator;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows;

namespace ModbusSlaveUi
{
    public class MyMainSlaveNetwork : SlaveNetwork
    {
        public MainWindow UiWindow { get; set; }
        public List<string> SelectedIODeviceToSend = new();

        public MyMainSlaveNetwork(MainWindow uiWindow, IPAddress ip, int port) : base(ip, port)
        {
            UiWindow = uiWindow;
        } 

        public override void OnStart()
        {
            try
            {
                if (IsSlaveTcpListenerOpen)
                {
                    base.CreateMySlaveNetwork(SlaveTcpListener);
                    WriteLog("Listener server start");
                    SlaveTcpListener.Start();
                    CreateMySlaveNetwork(SlaveTcpListener);
                    Network.ListenAsync(NetworkToken.Token).GetAwaiter().GetResult();
                }
            }
            catch
            {
                UiWindow.Dispatcher.Invoke(delegate
                {
                    UiWindow.Tbl_Infos.Text += "\n" + "Network thread stopped listening." + "\n" + "TcpListener stopped listening.";
                });
            }
        }

        public override void CreateMySlaveNetwork(TcpListener slaveTcpListener)
        {
            base.CreateMySlaveNetwork(slaveTcpListener);
            AddNewModbusSlave(0, "ModbusSlave");
        }

        public override void AddSlaveToNetwork(Slave slave)
        {
            base.AddSlaveToNetwork(slave);
        }


        public void AddNewModbusSlave(ModbusSlaveWindow modbusSlaveWindow, ModbusSlave modbusSlave, CancellationTokenSource cancellationTokenSource)
        {
            var slaveToAdd = new MyModbusSlave(modbusSlaveWindow, _networkFactory, Network , modbusSlave, cancellationTokenSource);
            AddSlaveToNetwork(slaveToAdd);
            WriteLog($"New slave added: {modbusSlave.Name} {modbusSlave.StartAdress}");
        }

        public void AddNewStacklightSlave(StackLightWindow stackLightWindow, StackLightSlave stackLight, CancellationTokenSource cancellationTokenSource)
        {
            // ADD CANCELLATION TOKEN?
            var slaveToAdd = new MyStackLightSlave(stackLightWindow, _networkFactory, Network, stackLight, cancellationTokenSource);
            AddSlaveToNetwork(slaveToAdd);
            WriteLog($"New slave added: {stackLight.Name} {stackLight.ByteId}");
        }

        #region Helper Graphic
        public void WriteLog(string log)
        {
            UiWindow.Dispatcher.Invoke(delegate
            {
                UiWindow.Tbl_Infos.Text += "\n" + log;
            });
        }
        #endregion
    }
}
