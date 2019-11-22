using CvEv6.API.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CvEv6.API.Controllers
{
    public class DummyController: Controller
    {
        private CvEContext _ctx;
        public DummyController(CvEContext ctx)
        {
            _ctx = ctx;
        }

        [HttpGet]
        [Route("api/testdatabase")]

        public IActionResult TestDatabase()
        {
            return Ok();
        }
    }
}
