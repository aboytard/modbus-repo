using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace ModbusSlaveUi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MyMainSlaveNetwork MyMainSlaveNetwork;

        // key : name of the slave, value : byteId
        public Dictionary<string, byte> _slaveMapping = new();
        public ObservableCollection<string> _slaves { get; set; } = new();

        // key: name of the slave / Title of the window, value : cancellation token in of the slave
        public Dictionary<string, CancellationTokenSource> _runningSlaveCancellationToken = new();
        public Dictionary<string, Window> _runningWindows = new();

        public Thread MyListenerThread { get; set; }

        #region StackLight
        public StackLightSlave currentStackLight { get; set; }
        public ObservableCollection<string> SlColor { get; set; }
        public ObservableCollection<string> SlInputBit { get; set; }
        public ObservableCollection<string> SlNbWord { get; set; }
        #endregion
        #region Input
        public int SelectedNumberOfInputBit { get; set; }
        #endregion

        #region Output
        public ModbusSlave currentModbusSlave { get; set; }
        private ushort _startAdress;
        #endregion

        public MainWindow()
        {
            InitializeComponent();

            myListSlaves.ItemsSource = _slaves;

            SlColor = new ObservableCollection<string> { "Red", "Yellow", "Green" };
            LB_SlColor.ItemsSource = SlColor;

            SlInputBit = new ObservableCollection<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", };
            LB_InputBit_Sl.ItemsSource = SlInputBit;

            SlNbWord = new ObservableCollection<string> { "0", "1", "2", "3", "4", "5" };
            LB_NbWord.ItemsSource = SlNbWord;
        }

        private void Btn_Start_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _slaves.Add("ModbusSlave");
                _slaveMapping.TryAdd("ModbusSlave", (byte)Int32.Parse("0"));
                MyMainSlaveNetwork = new MyMainSlaveNetwork(this, IPAddress.Parse(Tb_Ip.Text), Int32.Parse(Tb_Port.Text));
                MyListenerThread = new Thread(new ThreadStart(MyMainSlaveNetwork.OnTcpConnectionChangeState));
                MyListenerThread.Start();
            }
            catch (Exception exeption)
            {
                Console.WriteLine(exeption.Message);
            }
        }

        private void Btn_Stop_Click(object sender, RoutedEventArgs e)
        {
            MyListenerThread.Join(TimeSpan.FromSeconds(1));
            MyMainSlaveNetwork.OnStop();
            foreach(var pair in _runningSlaveCancellationToken)
            {
                pair.Value.Cancel();
            }
            foreach(var pair in _runningWindows)
            {
                pair.Value.Close();
            }
            // Can create a Dispose method
            _runningWindows.Clear();
            _runningSlaveCancellationToken.Clear();
            MyMainSlaveNetwork.mySlaveMapping.Clear();
            _slaveMapping.Clear();
        }

        private void myListSlaves_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
                MyMainSlaveNetwork.SelectedIODeviceToSend.Add(e.AddedItems[0].ToString());
            if (e.RemovedItems.Count > 0)
                MyMainSlaveNetwork.SelectedIODeviceToSend.Remove(e.RemovedItems[0].ToString());
        }

        public void SetupStackLight()
        {
            Dispatcher.Invoke(delegate
            {
                currentStackLight = new StackLightSlave()
                {
                    Ip = Tb_Ip.Text,
                    Port = Int32.Parse(Tb_Port.Text.ToString()),
                    Name = Tb_Name_Sl.Text,
                    ByteId = (byte)Int32.Parse(Tb_ByteId_Sl.Text),
                    Active = Int32.Parse(Tb_SeqActive.Text),
                    Inactive = Int32.Parse(Tb_SeqInactive.Text),
                    Repetition = Int32.Parse(Tb_Repetition.Text),
                    NbWord = Int32.Parse(LB_NbWord.SelectedItem.ToString()),
                    InputBit = Int32.Parse(LB_InputBit_Sl.SelectedItem.ToString()),
                    Color = Enum.Parse<StackLightColor>(LB_SlColor.SelectedItem.ToString())
                };
            });
        }

        private void Btn_ClearLogs_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(delegate
            {
                Tbl_Infos.Text = "";
            });
        }

        private void Btn_CreateSl_Click(object sender, RoutedEventArgs e)
        {
            // HERE I WILL CREATE THE MAPPING BASED ON THE CANCELLATIONTOKEN
            var cancellationSourceToken = new CancellationTokenSource();
            Dispatcher.Invoke(delegate
            {
                SetupStackLight();
                var stackLightWindow = new StackLightWindow(this, MyMainSlaveNetwork, currentStackLight);
                stackLightWindow.Show();
                var slaveByteId = (byte)(Int32.Parse(Tb_ByteId_Sl.Text));
                MyMainSlaveNetwork.AddNewStacklightSlave(stackLightWindow, stackLightWindow.StackLight, cancellationSourceToken);

                // mapping
                _slaveMapping.TryAdd(Tb_Name_Sl.Text, slaveByteId);
                _slaves.Add(Tb_Name_Sl.Text);
                _runningWindows.TryAdd(Tb_Name_Sl.Text, stackLightWindow);
                _runningSlaveCancellationToken.TryAdd(Tb_Name_Sl.Text, cancellationSourceToken);
            });
        }

        private void Btn_Create_MD_Click(object sender, RoutedEventArgs e)
        {
            var cancellationSourceToken = new CancellationTokenSource();
            Dispatcher.Invoke(delegate
            {
                SetupModbusDevice();
                var modbusSlaveWindow = new ModbusSlaveWindow(this, MyMainSlaveNetwork, currentModbusSlave);
                modbusSlaveWindow.Show();
                var slaveByteId = (byte)(Int32.Parse(Tb_StartAdress_Md.Text));
                MyMainSlaveNetwork.AddNewModbusSlave(modbusSlaveWindow, currentModbusSlave, cancellationSourceToken);

                // mapping
                _slaveMapping.TryAdd(Tb_Name_Sl.Text, slaveByteId);
                _slaves.Add(Tb_Name_Sl.Text);
                _runningWindows.TryAdd(Tb_Name_Sl.Text, modbusSlaveWindow);
                _runningSlaveCancellationToken.TryAdd(Tb_Name_Sl.Text, cancellationSourceToken);
            });
        }

        public void SetupModbusDevice()
        {
            Dispatcher.Invoke(delegate
            {
                currentModbusSlave = new ModbusSlave()
                {
                    Name = Tb_Name_Md.Text,
                    StartAdress = (ushort)Int32.Parse(Tb_StartAdress_Md.Text),
                };
            });
        }
    }
}
