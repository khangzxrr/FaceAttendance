using Microsoft.EntityFrameworkCore;
using SpeedyAPI.Models;

namespace SpeedyAPI.Data
{
    public class DBMajorContext: DbContext
    {
        public DBMajorContext(DbContextOptions<DBMajorContext> options)
            : base(options)
        {

        }

        public DbSet<Major> majors { get; set; }
    }
}
