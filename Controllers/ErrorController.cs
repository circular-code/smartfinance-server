using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using smartfinance_server.Models;

namespace smartfinance_server.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [HttpGet]
        [HttpPost]
        [Route("/error")]
        public IActionResult Error(UncaughtError e)
        {
            return Problem("{message:'Uncaught Error'}");
        }
    }
}
