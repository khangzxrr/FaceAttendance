using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeedyAPI.Data
{
    public class DBKeyContext : DbContext
    {
        public DBKeyContext(DbContextOptions<DBKeyContext> options)
           : base(options)
        {

        }

        public DbSet<Models.Key> Keys { get; set; }
    }
}
