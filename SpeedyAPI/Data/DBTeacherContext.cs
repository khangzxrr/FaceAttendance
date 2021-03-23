using Microsoft.EntityFrameworkCore;
using SpeedyAPI.Models;

namespace SpeedyAPI.Data
{
    public class DBTeacherContext : DbContext
    {
        public DBTeacherContext(DbContextOptions<DBTeacherContext> options)
                : base(options)
        {

        }

        public DbSet<TeacherAccount> TeacherAccounts { get; set; }
    }
}
