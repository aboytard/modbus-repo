using NModbus;
using SharedLibrary;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace ModbusSlaveUi
{
    public class SlaveNetwork
    {
        // define the tcpConnection
        public int Port { get; set; }
        public IPAddress IPAddress { get; set; }
        public TcpListener SlaveTcpListener { get; set; }
        // define the networkState
        public bool IsSlaveTcpListenerOpen { get; set; } = false;

        // key: name of the slave, value: slave with its own handler method
        public IDictionary<string, Slave> mySlaveMapping { get; set; } = new Dictionary<string, Slave>();

        public IModbusSlaveNetwork Network { get; set; }
        public IModbusFactory _networkFactory { get; set; } = new ModbusFactory();
        public CancellationTokenSource NetworkToken { get; set; }

        public SlaveNetwork(IPAddress ip, int port)
        {
            IPAddress = ip;
            Port = port;
            SlaveTcpListener = new TcpListener(IPAddress, Port);
            _networkFactory = new ModbusFactory();
            NetworkToken = new CancellationTokenSource();
        }

        public void OnTcpConnectionChangeState()
        {
            if (!IsSlaveTcpListenerOpen)
            {
                IsSlaveTcpListenerOpen = !IsSlaveTcpListenerOpen;
                OnStart();
                CreateMySlaveNetwork(SlaveTcpListener);
            }
        }


        public virtual void OnStart()
        {
            try
            {
                if (IsSlaveTcpListenerOpen)
                {
                    SlaveTcpListener.Start();
                    CreateMySlaveNetwork(SlaveTcpListener);
                    Network.ListenAsync(NetworkToken.Token).GetAwaiter().GetResult();
                }

            }
            catch
            {
                throw new NotImplementedException();
            }
        }

        public void OnStop()
        {
            NetworkToken.Cancel();
            IsSlaveTcpListenerOpen = false;
            SlaveTcpListener.Stop();
        }

        public virtual void CreateMySlaveNetwork(TcpListener slaveTcpListener)
        {
            Network = _networkFactory.CreateSlaveNetwork(slaveTcpListener);
        }

        public virtual void AddSlaveToNetwork(Slave slave)
        {
            Network.AddSlave(slave._slave);
            mySlaveMapping.TryAdd(slave.Name, slave);
        }

        // HERE I SHOULD ADD TWO DELEGATE THAT CAN THEN BE CHANGED DEPENDING
        public virtual void AddNewSlave(byte byteId, string name)
        {
            var slaveToAdd = new Slave(_networkFactory, Network, name, byteId);
            AddSlaveToNetwork(slaveToAdd);
        }
    }
}
