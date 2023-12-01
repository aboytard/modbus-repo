using SharedLibrary;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace ModbusSlaveUi
{
    /// <summary>
    /// Interaction logic for ModbusSlaveWindow.xaml
    /// </summary>
    public partial class ModbusSlaveWindow : Window
    {
        public ObservableCollection<string> MyInputBits { get; set; }
        public ObservableCollection<string> MyOutputBits { get; set; }
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
        private ushort[] _outputWords { get; set; } = new ushort[] { };
        private List<string> _selectedOutputBit = new();
        private ushort _startAdress;
        #endregion

        public MainWindow MainWindow { get; set; }
        public MyMainSlaveNetwork MainSlaveNetwork { get; set; }

        public ModbusSlave ModbusSlave { get; set; }
        public ModbusSlaveWindow(MainWindow mainWindow, MyMainSlaveNetwork mainSlaveNetwork, ModbusSlave modbusSlave)
        {
            MainWindow = mainWindow;
            MainSlaveNetwork = mainSlaveNetwork;
            ModbusSlave = modbusSlave;
            InitializeComponent();
            MyInputBits = new ObservableCollection<string> { "0", "1", "2", "3", "4", "5" };
            myListBoxInputBits.ItemsSource = MyInputBits;

            MyOutputBits = new ObservableCollection<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9",
                "10", "11", "12", "13", "14", "15" };
            myListBoxOutputBits.ItemsSource = MyOutputBits;
        }

        private void Btn_AddOutputBit_Click(object sender, RoutedEventArgs e)
        {
            string myUshortString = string.Empty;
            int myUshortInt = 0;
            for (int i = 0; i < myListBoxOutputBits.Items.Count; i++)
            {
                if (_selectedOutputBit.Contains(this.myListBoxOutputBits.Items[i].ToString()))
                {
                    myUshortString = "1" + myUshortString;
                    myUshortInt += (int)Math.Pow(2, i);
                }
                else
                    myUshortString = "0" + myUshortString;
            }
            AddOutputWordToTbl(myUshortInt.ToString());
            var arrayResized = _outputWords;
            Array.Resize(ref arrayResized, arrayResized.Length + 1);
            arrayResized[arrayResized.Length - 1] = (ushort)myUshortInt;
            _outputWords = arrayResized;
            _startAdress = (ushort)(Int32.Parse(Tb_StartAdress.Text));

            // unselect the output bits
            myListBoxOutputBits.UnselectAll();
            _selectedOutputBit.Clear();
        }

        public void AddOutputWordToTbl(string word)
        {
            Dispatcher.Invoke(delegate
            {
                Tbl_OutputBits.Text += " " + word;
            });
        }

        private void myListBoxOutputBits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
                OutputBitSelectionChanged(e.AddedItems[0].ToString());
            if (e.RemovedItems.Count > 0)
                OutputBitSelectionChanged(e.RemovedItems[0].ToString());
        }

        public void OutputBitSelectionChanged(string outputBit)
        {
            if (_selectedOutputBit.Contains(outputBit))
                _selectedOutputBit.Remove(outputBit);
            else
                _selectedOutputBit.Add(outputBit);
        }

        private void myListBoxInputBits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
                SelectedNumberOfInputBit = Int32.Parse(e.AddedItems[0].ToString());
        }

        private void Btn_ClearMessage_Click(object sender, RoutedEventArgs e)
        {
            _outputWords = new ushort[] { };
            ClearOutputWord();
        }

        public void ClearOutputWord()
        {
            Dispatcher.Invoke(delegate
            {
                Tbl_OutputBits.Text = "";
            });
        }

        private void Btn_SendMessage_Click(object sender, RoutedEventArgs e)
        {
            // HERE I NEED TO FIND A WAY TO GET IT : MAYBE ONLY DEFINE TO THE BYTE NUMBER
            foreach (var slave in MainSlaveNetwork.SelectedIODeviceToSend)
            {
                MainSlaveNetwork.mySlaveMapping[slave].WriteInHoldingRegisterFromSlave(_startAdress, _outputWords);
            }
            _outputWords = new ushort[] { };
            ClearOutputWord();
        }
    }
}
