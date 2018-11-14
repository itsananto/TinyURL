using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace TinyURL.Models
{
    public class TinyURLContext : DbContext
    {
        public TinyURLContext (DbContextOptions<TinyURLContext> options)
            : base(options)
        {
        }

        public DbSet<TinyURL.Models.URLMap> URLMap { get; set; }
    }
}
