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
    [Route("api/titles")]
    public class TitlesController: Controller
    {
        private ICvERepository _cvERepository;

        public TitlesController(ICvERepository cvERepository)
        {
            _cvERepository = cvERepository;
        }

        [HttpGet(Name = "GetTitle")]
        public IActionResult GetTitles()
        {
            var titleEntities = _cvERepository.GetTitles();
            var results = Mapper.Map<IEnumerable<TitleDto>>(titleEntities);
            Debug.WriteLine($"Domain Data Requested {DateTime.Now}");
            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetTitle(int id)
        {
            var title = _cvERepository.GetTitle(id);

            if (title == null)
            {
                return NotFound();
            }

            var result = Mapper.Map<TitleDto>(title);
            return Ok(result);

        }

        [HttpPost()]
        public IActionResult CreateTitle([FromBody]TitleForCreationDto title)
        {
            
            if (title == null || title.Name == "")
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_cvERepository.TitleEntityExists(title.Name))
            {
                return StatusCode(400, "Title already exists");
            }

            var finalTitle = Mapper.Map<Entities.Title>(title);

            _cvERepository.AddTitleEntity(finalTitle);

            if (!_cvERepository.Save())
            {
                return StatusCode(500, "A problem occured while handling your request");
            }

            var CreatedTitleToReturn = Mapper.Map<Models.TitleDto>(finalTitle);

            return CreatedAtRoute("GetTitle", new
            {
                id = CreatedTitleToReturn.Id
            }, CreatedTitleToReturn);
        }

        [HttpDelete("/{id}")]
        public IActionResult DeleteTitle(int id)
        {
            var title = _cvERepository.GetTitle(id);
            if (title == null || title.Name == "")
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_cvERepository.TitleEntityExists(title.Name))
            {
                return StatusCode(400, "Title already exists");
            }

            _cvERepository.DeleteTitleEntity(title);

            return null;

        }
    }
}
