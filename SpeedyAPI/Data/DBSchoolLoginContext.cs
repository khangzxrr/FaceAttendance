using Microsoft.EntityFrameworkCore;
using SpeedyAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpeedyAPI.Data
{
    public class DBSchoolLoginContext: DbContext
    {
        public DBSchoolLoginContext(DbContextOptions<DBSchoolLoginContext> options)
            : base(options)
        {

        }

        public DbSet<SchoolAccount> SchoolAccounts { get; set; }
    }
}
