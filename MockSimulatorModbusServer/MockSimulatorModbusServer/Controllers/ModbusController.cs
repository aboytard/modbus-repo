using Microsoft.AspNetCore.Mvc;
using MockSimulatorModbusServer.MockDTO;

namespace MockSimulatorModbusServer.Controllers
{
    [ApiController]
    [Route("api")]
    public class ModbusController : ControllerBase
    {
        [HttpGet]
        [Route("/simulate/modbus/getall")]
        public ActionResult GetClients()
        {
            var data = ModbusDevicesDTO.getFakeModbusDeviceDTO();
            return Ok(data);
        }



        [HttpPost]
        [Route("/modbus/add")]
        public ActionResult AddModbusClient(ModbusParamsDTO modbusClientParams)
        {
            //create-mock-data
            var data = ModbusParamsDTO.GetFakeModbusClientDTO();
            return Ok(data);
        }

        [HttpPost]
        [Route("/modbus/delete/{id}")]
        public ActionResult DeleteModbusClient(string id)
        {
            return Ok(id);
        }
    }
}