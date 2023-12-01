using AmpModbusMasterUi.Configuration;
using AmpModbusMasterUi.Libs;
using AmpModbusMasterUi.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace AmpModbusMasterUi
{
    public class IOSignalHandler : IDisposable
    {
        private readonly string id;
        private readonly IOSignalProvider ioSignalProvider;
        private readonly ModbusDeviceHandler modbusDeviceHandler;
        private readonly Dictionary<IOSignalAddress, ushort> outputSignalMapping;
        private readonly Dictionary<IOSignalAddress, bool> currentStatus = new Dictionary<IOSignalAddress, bool>();
        private readonly Dictionary<IOSignalAddress, InputDebouncer> debouncers = new Dictionary<IOSignalAddress, InputDebouncer>();

        private readonly ILogger logger = Logging.Factory.CreateLogger<IOSignalHandler>();

        // check How is the Provider working in that case --> cause no need 
        public IOSignalHandler(IOSignalConfiguration config, IOSignalProvider ioSignalProvider, ModbusDeviceHandler modbusDeviceHandler)
        {
            id = config.Id;
            this.ioSignalProvider = ioSignalProvider;
            this.modbusDeviceHandler = modbusDeviceHandler;

            ioSignalProvider.GetCurrentSignalRequestHandler = ProcessGetCurrentSignalRequest;
            ioSignalProvider.SetSignalRequestHandler = ProcessSetSignalRequest;

            outputSignalMapping = config.OutputSignals.ToDictionary(s => s.SignalAddress, s => s.ModbusAddress);

            foreach (var inputSignal in config.InputSignals)
            {
                var signalAddress = inputSignal.SignalAddress;
                if (inputSignal.DebounceTime.HasValue)
                {
                    debouncers.Add(signalAddress, new InputDebouncer(inputSignal.DebounceTime.Value,
                        state => PublishInputSignalChange(signalAddress, state)));
                }
                modbusDeviceHandler.ListenToInputSignal(inputSignal.ModbusAddress,
                    state => ProcessInputSignalChange(signalAddress, state));
            }
        }

        public void Dispose()
        {
            publishSignalChangeActionBlock.Complete();
        }

        // LET'S UNDERSTAND WHAT IS THIS ACTION-BLOCK 
        private readonly ActionBlock<Func<Task>> publishSignalChangeActionBlock = new ActionBlock<Func<Task>>(async action => await action());

    }
}
