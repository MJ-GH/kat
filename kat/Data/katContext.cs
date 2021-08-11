using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using kat.Models;

namespace kat.Data
{
    public class katContext : DbContext
    {
        public katContext (DbContextOptions<katContext> options)
            : base(options)
        {
        }
        public DbSet<kat.Models.Note> Note { get; set; }
    }
}
