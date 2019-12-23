using AutoMapper;
using CvEv6.API.Models;
using CvEv6.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CvEv6.API.Controllers
{
    [Route("api/domains")]
    public class DocumentsController : Controller
    {
        private ICvERepository _cvERepository;

        public DocumentsController(ICvERepository cvERepository)
        {
            _cvERepository = cvERepository;
        }

        [HttpGet("{domainId}/documents")]
        public IActionResult GetDocuments(int domainId)
        {
            if (!_cvERepository.domainEntityExists(domainId))
            {
                return NotFound();
            }

            var documentsForDomain = _cvERepository.GetDocumentsForDomainEntity(domainId);
            var documentsForDomainResult = Mapper.Map<IEnumerable<DocumentDto>>(documentsForDomain);
            Debug.WriteLine($"Documents Data Requested {DateTime.Now}");
            return Ok(documentsForDomainResult);
        }

        [HttpGet("{domainId}/documents/{id}", Name = "GetDocument")]
        public IActionResult GetDocument(int domainId, int Id)
        {
            if (!_cvERepository.domainEntityExists(domainId))
            {
                return NotFound();
            }

            var documentForDomain = _cvERepository.GetDocumentForDomainEntity(domainId, Id);

            if (documentForDomain == null)
            {
                return NotFound();
            }

            var documentForDomainResult = Mapper.Map<DocumentDto>(documentForDomain);

            return Ok(documentForDomainResult);
        }

        [HttpPost("{domainId}/documents")]
        public IActionResult CreateDocument(int domainId,
            [FromBody]DocumentForCreationDto document)
        {
            if (document == null || document.Name == "")
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_cvERepository.domainEntityExists(domainId))
            {
                return NotFound();
            }


            var finalDocument = Mapper.Map<Entities.Document>(document);

            _cvERepository.AddDocumentForDomainEntity(domainId, finalDocument);

            if (!_cvERepository.Save())
            {
                return StatusCode(500, "A problem occured while handling your request");
            }

            var CreatedDocumentToReturn = Mapper.Map<Models.DocumentDto>(finalDocument);

            return CreatedAtRoute("GetDocument", new
            {
                domainId = domainId,
                id = CreatedDocumentToReturn.Id
            }, CreatedDocumentToReturn);
        }


        [HttpPut("{domainId}/documents/{id}")]
        public IActionResult UpdateDocument(int domainId, int  id, 
            [FromBody] DocumentForUpdateDto document)
        {
            if (document == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_cvERepository.domainEntityExists(domainId))
            {
                return NotFound();
            }

            var documentEntity = _cvERepository.GetDocumentForDomainEntity(domainId, id);
            if (documentEntity == null)
            {
                return NotFound();
            }

            Mapper.Map(document, documentEntity);
            if (!_cvERepository.Save())
            {
                return StatusCode(500, "A problem occured while handling your request");
            }

            return NoContent();
        }

        [HttpPatch("{domainId}/documents/{id}")]
        public IActionResult PartiallyUpdateDocument(int domainId, int Id,
        [FromBody] JsonPatchDocument<DocumentForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return NotFound();
            }

            if (!_cvERepository.domainEntityExists(domainId))
            {
                return NotFound();
            }

            var documentEntity = _cvERepository.GetDocumentForDomainEntity(domainId, Id);
            if (documentEntity == null)
            {
                return NotFound();
            }

            var documentToPatch = Mapper.Map<DocumentForUpdateDto>(documentEntity);

            patchDoc.ApplyTo(documentToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Mapper.Map(documentToPatch, documentEntity);

            if (!_cvERepository.Save())
            {
                return StatusCode(500, "A problem occured while handling your request");
            }

            return NoContent();
        }

        [HttpDelete("{domainId}/documents/{id}")]
        public IActionResult DeleteDocument(int domainId, int id)
        {
            if (!_cvERepository.domainEntityExists(domainId))
            {
                return NotFound();
            }

            var documentEntity = _cvERepository.GetDocumentForDomainEntity(domainId, id);
            if (documentEntity == null)
            {
                return NotFound();
            }

            _cvERepository.DeleteDocumentForDomainEntity(documentEntity);

            if (!_cvERepository.Save())
            {
                return StatusCode(500, "A problem occured while handling your request");
            }

            return NoContent();
        }
    }
}
