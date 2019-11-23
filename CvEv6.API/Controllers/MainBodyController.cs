using AutoMapper;
using CvEv6.API.Models;
using CvEv6.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CvEv6.API.Controllers
{
    [Route("/api/mainbody/")]
    public class MainBodyController : Controller
    {
        private ICvERepository _cvERepository;

        public MainBodyController(ICvERepository cvERepository)
        {
            _cvERepository = cvERepository;
        }

        [HttpGet("{id}")]
        public IActionResult GetMainBody(int id)
        {
            var mainBody = _cvERepository.GetMainBodyEntity(id);

            if (mainBody == null)
            {
                return NotFound();
            }

            var result = Mapper.Map<MainBodyDto>(mainBody);
            return Ok(result);
        }

        [HttpGet(Name = "GetMainBody")]
        public IActionResult GetMainBodies()
        {
            var MainBodyEntities = _cvERepository.GetMainBodyEntities();
            var results = Mapper.Map<IEnumerable<MainBodyDto>>(MainBodyEntities);
            Debug.WriteLine($"MainBody Data Requested {DateTime.Now}");
            return Ok(results);
        }

        [HttpPost()]
        public IActionResult CreateMainBody([FromBody] MainBodyForCreationDto mainbody)
        {
            if (mainbody == null || mainbody.Name == "")
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_cvERepository.MainBodyExists(mainbody.Name))
            {
                return StatusCode(400, "MainBody already exists");
            }

            var finalMainBody = Mapper.Map<Entities.MainBody>(mainbody);

            _cvERepository.AddMainBodyEntity(finalMainBody);

            if (!_cvERepository.Save())
            {
                return StatusCode(500, "A problem occured while handling your request");
            }

            var CreatedMainBodyToReturn = Mapper.Map<Models.MainBodyDto>(finalMainBody);

            return CreatedAtRoute("GetMainBody", new
            {
                id = CreatedMainBodyToReturn.Id
            }, CreatedMainBodyToReturn);
        }
    }
}
