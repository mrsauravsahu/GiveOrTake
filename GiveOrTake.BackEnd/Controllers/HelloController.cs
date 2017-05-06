using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GiveOrTake.BackEnd.Controllers
{
    [Route("api/[controller]")]
    public class HelloController
    {
        [HttpGet]
        [AllowAnonymous]
        public ObjectResult Get()
        {
            return new ObjectResult(new { Message = "Hello World!" });
        }
    }
}
