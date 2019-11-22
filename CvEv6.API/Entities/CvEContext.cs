using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CvEv6.API.Entities
{
    public class CvEContext: DbContext
    {
        public CvEContext(DbContextOptions<CvEContext> options)
        : base(options)
        {
            Database.Migrate();
        }

        public DbSet<Domain> Domains { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Title> Titles { get; set; }
    }
}
