using NModbus;
using SharedLibrary;
using System.Linq;
using System.Threading;

namespace ModbusSlaveUi
{
    public class MyModbusSlave : Slave
    {
        public ModbusSlaveWindow ModbusSlaveWindow { get; set; }
        private string pointWord { get; set; }

        public MyModbusSlave(ModbusSlaveWindow window, IModbusFactory networkFactory, IModbusSlaveNetwork slaveNetwork, ModbusSlave modbusSlave,CancellationTokenSource cancellationTokenSource) : base(networkFactory, slaveNetwork, modbusSlave.Name, (byte)modbusSlave.StartAdress)
        {
            ModbusSlaveWindow = window;
        }

        // need to check something based on the sender
        public override void StorageOperationOccuredForInputRegisterAction(object sender, StorageEventArgs<ushort> args)
        {
            // HERE NEED TO PUT
            WriteLog($"Input registers: {args.Operation} starting at {args.StartingAddress}");
        }

        public override void StorageOperationOccuredForHoldingRegisterAction(object sender, StorageEventArgs<ushort> args)
        {
            // HERE I AM NOT USING ANY THREAD?? OR THE CANCELLATION TOKEN??
            if (args.Points.Length == ModbusSlaveWindow.SelectedNumberOfInputBit)
            {
                var newPointWord = string.Join(" ", args.Points.Select(point => ((int)point).ToString()));
                if (pointWord != newPointWord || string.IsNullOrEmpty(pointWord))
                {
                    WriteLog($"Holding registers: {args.Operation} starting at {args.StartingAddress} with value : {newPointWord}.");
                    pointWord = newPointWord;
                }
            }
        }

        public void WriteLog(string log)
        {
            ModbusSlaveWindow.Dispatcher.Invoke(delegate
            {
                ModbusSlaveWindow.Tbl_Infos.Text += "\n" + log;
            });
        }
    }
}
