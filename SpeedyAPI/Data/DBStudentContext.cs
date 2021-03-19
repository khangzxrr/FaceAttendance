using Microsoft.EntityFrameworkCore;
using SpeedyAPI.Models;

namespace SpeedyAPI.Data
{
    public class DBStudentContext: DbContext
    {
        public DBStudentContext(DbContextOptions<DBStudentContext> options)
            : base(options)
        {

        }

        public DbSet<Student> Students { get; set; }
    }
}
