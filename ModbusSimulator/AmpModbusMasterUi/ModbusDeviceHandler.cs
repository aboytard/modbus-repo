using AmpModbusMasterUi.Configuration;
using NModbus;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AmpModbusMasterUi
{
    public class ModbusDeviceHandler : IDisposable
    {
        private readonly CancellationTokenSource cts = new CancellationTokenSource();
        private readonly MainConfiguration config;
        private readonly Dictionary<ushort, List<InputSignalChangedHandlerDelegate>> inputSignalListeners;
        private readonly byte slaveAddress;
        private IModbusMaster modbusMaster;
        public delegate void InputSignalChangedHandlerDelegate(bool state); // not sure to really understand this definition

        public ModbusDeviceHandler(MainConfiguration config /*,EventLoggingConsumer eventLoggingConsumer*/)
        {
            this.config = config;
            // I will use another way to log information in the ui
            //this.eventLoggingConsumer = eventLoggingConsumer;
            inputSignalListeners = new Dictionary<ushort, List<InputSignalChangedHandlerDelegate>>();
            slaveAddress = config.SlaveAddress;
        }

        public void Dispose()
        {
            cts.Cancel();
            cts.Dispose();
        }
    }
}
