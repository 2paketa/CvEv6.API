using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CvEv6.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CvEv6.API.Services
{
    public class CvERepository : ICvERepository
    {
        private CvEContext _context;
        public CvERepository(CvEContext context)
        {
            _context = context;
        }
        public bool domainEntityExists(int domainId)
        {
            return _context.Domains.Any(d => d.Id == domainId);
        }

        public bool TitleEntityExists(string name)
        {
            return _context.Titles.Any(d => d.Name == name);
        }

        public bool domainEntityExists(string name)
        {
            return _context.Domains.Any(d => d.Name == name);
        }

        public Domain GetDomainEntity(int domainId, bool includeDocuments)
        {
            if (includeDocuments)
            {
                return _context.Domains.Include(d => d.Documents)
                    .Where(d => d.Id == domainId).FirstOrDefault();
            }
            return _context.Domains
                .Where(d => d.Id == domainId).FirstOrDefault();
        }

        public IEnumerable<Domain> GetDomainEntities()
        {
            return _context.Domains.OrderBy(d => d.Name).ToList();
        }

        public void AddDDomainEntity(Domain domain)
        {
            _context.Domains.Add(domain);
        }

        public void AddDocumentForDomainEntity(int domainId, Document document)
        {
            var domain = _context.Domains.FirstOrDefault(d => d.Id == domainId);
            domain.Documents.Add(document);
        }

        public void AddTitleEntity(Title title)
        {
            _context.Titles.Add(title);
        }

        public Document GetDocumentForDomainEntity(int domainId, int documentId)
        {
            return _context.Documents
                .Where(d => d.DomainId == domainId && d.Id == documentId).FirstOrDefault();
        }

        public IEnumerable<Document> GetDocumentsForDomainEntity(int domainId)
        {
            return _context.Documents
                .Where(d => d.DomainId == domainId).OrderBy(d => d.Name).ToList();
        }

        public void DeleteDocumentForDomainEntity(Document document)
        {
            _context.Documents.Remove(document);
        }
        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

        public Title GetTitle(int titleId)
        {
            return _context.Titles
                .Where(t => t.Id == titleId).FirstOrDefault();
        }

        public List<Title> GetTitles()
        {
            return _context.Titles.ToList();
        }

        public void DeleteTitleEntity(Title title)
        {
            _context.Titles.Remove(title);
        }

        public void DeleteDomainEntity(Domain domainEntity)
        {
            var documents = _context.Documents;
            foreach (var doc in domainEntity.Documents)
            {
                documents.Remove(doc);
            }
            _context.Domains.Remove(domainEntity);
        }
    }
}
