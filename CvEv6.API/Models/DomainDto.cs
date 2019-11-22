using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CvEv6.API.Models
{
    public class DomainDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int NumberOfDocuments
        {
            get
            {
                return Documents.Count;
            }
        }

        public ICollection<DocumentDto> Documents { get; set; } = new List<DocumentDto>();
    }
}
