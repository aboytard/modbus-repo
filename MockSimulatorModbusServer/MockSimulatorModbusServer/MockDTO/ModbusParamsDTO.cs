namespace MockSimulatorModbusServer.MockDTO
{
    public class ModbusParamsDTO
    {
        public string Id { get; set; }
        public string IPAddress { get; set; }
        public string Port { get; set; }
        public string SimulatorType { get; set; }
        public string Name { get; set; }
        public int SlaveAddress { get; set; }
        public string? AttachedModbus { get; set; }
        public string IsServer { get; set; }
        public int NumberBits { get; set; }
        public int StartingAddress { get; set; }

        // with parameters for defining list of different datas
        public static ModbusParamsDTO GetFakeModbusClientDTO()
        {
            return new ModbusParamsDTO()
            {
                Id = "1",
                IPAddress = "127.0.0.1",
                Port = "502",
                SimulatorType = "Modbus",
                Name = "modbus-sim",
                SlaveAddress = 1,
                AttachedModbus = "",
                IsServer = "false",
                NumberBits = 8,
                StartingAddress = 1,
            };
        }

        public static ModbusParamsDTO GetFakeModbusServerDTO()
        {
            return new ModbusParamsDTO()
            {
                Id = "1",
                IPAddress = "127.0.0.1",
                Port = "503",
                SimulatorType = "Modbus",
                Name = "modbus-sim",
                SlaveAddress = 1,
                AttachedModbus = "",
                IsServer = "true",
                NumberBits = 8,
                StartingAddress = 1,
            };
        }

        public static ModbusParamsDTO GetFakeModbusAttachedServerDTO()
        {
            return new ModbusParamsDTO()
            {
                Id = "1",
                IPAddress = "127.0.0.1",
                Port = "502",
                SimulatorType = "Modbus",
                Name = "modbus-sim",
                SlaveAddress = 1,
                AttachedModbus = "127.0.0.1_502",
                IsServer = "true",
                NumberBits = 8,
                StartingAddress = 1,
            };
        }
    }
}
