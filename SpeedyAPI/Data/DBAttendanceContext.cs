using Microsoft.EntityFrameworkCore;
using SpeedyAPI.Models;

namespace SpeedyAPI.Data
{
    public class DBAttendanceContext : DbContext
    {
        public DBAttendanceContext(DbContextOptions<DBAttendanceContext> options) 
                : base(options)
        {

        }

        public DbSet<Attendance> Attendances;

        public DbSet<SpeedyAPI.Models.Attendance> Attendance { get; set; }
    }
}
