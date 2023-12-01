namespace MockSimulatorModbusServer.MockDTO
{
    public class ModbusDevicesDTO
    {
        public IEnumerable<ModbusClientDTO> ModbusClients { get; set; }
        public IEnumerable<ModbusParamsDTO> StandaloneServer { get; set; }

        public static ModbusDevicesDTO getFakeModbusDeviceDTO() 
        {
            return new ModbusDevicesDTO()
            {
                ModbusClients = new List<ModbusClientDTO>
                {
                    new ModbusClientDTO()
                    {
                        ModbusClient = ModbusParamsDTO.GetFakeModbusServerDTO(),
                        AttachedModbusServer = null
                    },
                    new ModbusClientDTO()
                    {
                        ModbusClient = ModbusParamsDTO.GetFakeModbusClientDTO(),
                        AttachedModbusServer = new List<ModbusParamsDTO>()
                        {
                            ModbusParamsDTO.GetFakeModbusAttachedServerDTO(),
                        }
                    }
                }
            };
        }
    }

    public class ModbusClientDTO
    {
        public ModbusParamsDTO ModbusClient { get; set; }
        public IEnumerable<ModbusParamsDTO> AttachedModbusServer { get; set; }
    }
}
