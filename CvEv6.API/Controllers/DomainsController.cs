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
    [Route("api/domains")]
    public class DomainsController: Controller
    {
        private ICvERepository _cvERepository;

        public DomainsController(ICvERepository cvERepository)
        {
            _cvERepository = cvERepository;
        }

        [HttpGet(Name = "GetDomain")]
        public IActionResult GetDomains()
        {
            var DomainEntities = _cvERepository.GetDomainEntities();
            var results = Mapper.Map<IEnumerable<DomainWithoutDocumentsDto>>(DomainEntities);
            Debug.WriteLine($"Domain Data Requested {DateTime.Now}");
            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetDomain(int id, bool includeDocuments = false)
        {
            var domain = _cvERepository.GetDomainEntity(id, includeDocuments);
            
            if (domain == null)
            {
                return NotFound();
            }

            if (includeDocuments)
            {
                var resultWithDocuments = Mapper.Map<DomainDto>(domain);
                return Ok(resultWithDocuments);
            }
            else
            {
                var resultWithoutDocuments = Mapper.Map<DomainWithoutDocumentsDto>(domain);
                return Ok(resultWithoutDocuments);
            }

        }

        [HttpPost()]
        public IActionResult CreateDomain([FromBody]DomainForCreationDto domain)
        {

            if (domain == null || domain.Name == "")
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_cvERepository.domainEntityExists(domain.Name))
            {
                return StatusCode(400, "Domain already exists");
            }

            var finalDomain = Mapper.Map<Entities.Domain>(domain);

            _cvERepository.AddDDomainEntity(finalDomain);

            if (!_cvERepository.Save())
            {
                return StatusCode(500, "A problem occured while handling your request");
            }

            var CreatedDomainToReturn = Mapper.Map<Models.DomainDto>(finalDomain);

            return CreatedAtRoute("GetDomain", new
            {
                id = CreatedDomainToReturn.Id
            }, CreatedDomainToReturn);
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteDomain(int id)
        {

            if (!_cvERepository.domainEntityExists(id))
            {
                return NotFound();
            }

            var domainEntity = _cvERepository.GetDomainEntity(id, true);


            if (domainEntity == null)
            {
                return NotFound();
            }

            _cvERepository.DeleteDomainEntity(domainEntity);

            if (!_cvERepository.Save())
            {
                return StatusCode(500, "A problem occured while handling your request");
            }

            return NoContent();
        }
    }
}
