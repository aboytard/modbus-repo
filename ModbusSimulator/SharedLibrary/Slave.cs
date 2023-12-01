using NModbus;
using NModbus.Data;
using System.Xml.Linq;

namespace SharedLibrary
{
    public class Slave
    {
        public string Name { get; set; }

        private IModbusSlaveNetwork _slaveNetwork { get; } // should not be able to set the _modbusFactory
        private SlaveStorage Storage { get; }

        public byte UnitId { get; set; }

        public ISlaveDataStore DataStore { get; set; }

        public IModbusSlave _slave { get; set; }
        private IModbusFactory _factory { get; set; }

        public Slave(IModbusFactory factory,IModbusSlaveNetwork slaveNetwork,string name, byte unitId)
        {
            Name = name;
            UnitId = unitId;
            _slaveNetwork = slaveNetwork;
            Storage = new SlaveStorage();
            _factory = factory;
            _slave = _factory.CreateSlave(UnitId, Storage);
            // here we assign the method to the storage
            Storage.InputRegisters.StorageOperationOccurred += (sender, args) => StorageOperationOccuredForInputRegisterAction(sender, args);
            Storage.HoldingRegisters.StorageOperationOccurred += (sender, args) => StorageOperationOccuredForHoldingRegisterAction(sender, args);
        }

        public Slave(IModbusSlaveNetwork slaveNetwork, SlaveStorage storage, string name)
        {
            Name = name;
            _slaveNetwork = slaveNetwork;
            Storage = storage;
            // here we assign the method to the storage
            Storage.InputRegisters.StorageOperationOccurred += (sender, args) => StorageOperationOccuredForInputRegisterAction(sender, args);
            Storage.HoldingRegisters.StorageOperationOccurred += (sender, args) => StorageOperationOccuredForHoldingRegisterAction(sender, args);
        }


        public void AddSlaveToFactory()
        {
            _slaveNetwork.AddSlave(_slave);
        }

        public void WriteInHoldingRegisterFromSlave(ushort startAdress, ushort[] outputWord)
        {
            _slave.DataStore.HoldingRegisters.WritePoints(startAdress, outputWord);
        }

        // need to check something based on the sender
        public virtual void StorageOperationOccuredForInputRegisterAction(object sender, StorageEventArgs<ushort> args)
        {
            throw new NotImplementedException();
        }

        public virtual void StorageOperationOccuredForHoldingRegisterAction(object sender, StorageEventArgs<ushort> args)
        {
            throw new NotImplementedException();
        }
    }
}
