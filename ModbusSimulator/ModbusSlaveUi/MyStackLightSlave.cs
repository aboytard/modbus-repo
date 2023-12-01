using ModbusSlaveUi;
using NModbus;
using SharedLibrary;
using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Media;

namespace StackLightSimulator
{
    public class MyStackLightSlave : Slave
    {
        public StackLightWindow StackLightWindow { get; set; }
        private StackLightSlave _stackLight { get; set; }
        // will be changed and added to the main class
        private CancellationTokenSource _cancellationTokenSource;

        public MyStackLightSlave(StackLightWindow stackLightWindow, IModbusFactory factory, IModbusSlaveNetwork slaveNetwork, StackLightSlave stackLight, CancellationTokenSource cancellationTokenSource) : base(factory,slaveNetwork, stackLight.Name, stackLight.ByteId)
        {
            // ADD STACKLIGHT OBJECT THERE!!! 
            _stackLight = stackLight;
            StackLightWindow = stackLightWindow;
            _cancellationTokenSource = cancellationTokenSource;
        }

        // need to check something based on the sender
        public override void StorageOperationOccuredForInputRegisterAction(object sender, StorageEventArgs<ushort> args)
        {
            WriteLog($"Input registers: {args.Operation} starting at {args.StartingAddress}");
        }

        public override void StorageOperationOccuredForHoldingRegisterAction(object sender, StorageEventArgs<ushort> args)
        {
            if (args.Points.Length == _stackLight.NbWord)
            {
                string sLWord = string.Join(" ", args.Points.Select(point => ((int)point).ToString()));
                WriteLog($"STACKLIGHT HEARD : Holding registers: {args.Operation} starting at {args.StartingAddress} with value : {sLWord}.");
                // HERE IS WANT TO START IT ONLY ONCE
                // NEED TO SET IT FOR ONLY ONCE AS WELL
                // How can I take control back on this thread
                var _mySlThread = new Thread(new ThreadStart(this.ListenToStackLightCall));
                _mySlThread.Start();
            }
        }

        public void ListenToStackLightCall()
        {
            var numberOfBlicking = 0;
            // HERE I BLOCK THE THREAD
            // I am blocking the thread of 
            while (numberOfBlicking < _stackLight.Repetition && !_cancellationTokenSource.Token.IsCancellationRequested)
            {
                // CAREFUL I AM GONNA MAKE THE MAIN THREAD SLEEP
                ChangeStackLightColor(_stackLight.Color);
                Thread.Sleep(_stackLight.Active);
                ChangeStackLightColor(StackLightColor.None);
                Thread.Sleep(_stackLight.Inactive);
                numberOfBlicking += 1;
            }
        }

        #region StackLightWindow


        public void ChangeStackLightColor(StackLightColor? color)
        {
            StackLightWindow.Dispatcher.Invoke(delegate
            {
                switch (color)
                {
                    case StackLightColor.None:
                        StackLightWindow.RedLight.Fill = Brushes.Gray;
                        StackLightWindow.YellowLight.Fill = Brushes.Gray;
                        StackLightWindow.GreenLight.Fill = Brushes.Gray;
                        break;
                    case StackLightColor.Green:
                        StackLightWindow.RedLight.Fill = Brushes.Gray;
                        StackLightWindow.YellowLight.Fill = Brushes.Gray;
                        StackLightWindow.GreenLight.Fill = Brushes.Green;
                        break;
                    case StackLightColor.Yellow:
                        StackLightWindow.RedLight.Fill = Brushes.Gray;
                        StackLightWindow.YellowLight.Fill = Brushes.Yellow;
                        StackLightWindow.GreenLight.Fill = Brushes.Gray;
                        break;
                    case StackLightColor.Red:
                        StackLightWindow.RedLight.Fill = Brushes.Red;
                        StackLightWindow.YellowLight.Fill = Brushes.Gray;
                        StackLightWindow.GreenLight.Fill = Brushes.Gray;
                        break;
                }
            });
        }

        public void WriteLog(string log)
        {
            StackLightWindow.Dispatcher.Invoke(delegate
            {
                StackLightWindow.Tbl_Log.Text += "\n" + log;
            });
        }
        #endregion
    }
}
