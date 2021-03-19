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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Major>().ToTable("Majors");
        }

        public DbSet<Major> Majors { get; set; }
    }
}
