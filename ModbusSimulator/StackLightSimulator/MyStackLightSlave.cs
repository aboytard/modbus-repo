using NModbus;
using SharedLibrary;
using System;

namespace StackLightSimulator
{
    public class MyStackLightSlave : Slave
    {
        public MyStackLightSlave(IModbusSlaveNetwork slaveNetwork, string name, byte unitId) : base(slaveNetwork, name, unitId)
        {
        }

        // need to check something based on the sender
        public override void StorageOperationOccuredForInputRegisterAction(object sender, StorageEventArgs<ushort> args)
        {
            throw new NotImplementedException();
        }

        public override void StorageOperationOccuredForHoldingRegisterAction(object sender, StorageEventArgs<ushort> args)
        {
            throw new NotImplementedException();
        }
    }
}
