using Microsoft.EntityFrameworkCore;
using SpeedyAPI.Models;

namespace SpeedyAPI.Data
{
    public class DBSubjectContext : DbContext
    {
        public DBSubjectContext(DbContextOptions<DBSubjectContext> options)
                : base(options)
        {

        }

        public DbSet<Subject> Subjects { get; set; }
    }
}
