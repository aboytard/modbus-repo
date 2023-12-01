using Microsoft.AspNetCore.Mvc;

namespace Mock_Modbus_Master.Controllers
{
    [ApiController]
    [Route("api")]
    public class ModbusMasterController : ControllerBase
    {
        [HttpPost]
        [Route("/simulate/modbus/write")]
        public ActionResult WriteMultipleRegister()
        {
            var response = ModbusMaster.Mock_WriteMultipleRegister();
            return Ok(response);
        }

        [HttpGet]
        [Route("/test")]
        public ActionResult Test()
        {
            return Ok("test");
        }
    }
}