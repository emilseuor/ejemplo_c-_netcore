using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("Home")]
    public class Home : Controller
    {
        public ActionResult Index() {
            return Json("Welcome to the API!");
        }

    }
}
