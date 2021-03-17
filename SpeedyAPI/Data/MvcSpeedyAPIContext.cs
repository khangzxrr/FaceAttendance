using Microsoft.EntityFrameworkCore;
using SpeedyAPI.Models;

namespace SpeedyAPI.Data
{
    public class MvcSpeedyAPIContext: DbContext
    {
        public MvcSpeedyAPIContext(DbContextOptions<MvcSpeedyAPIContext> options)
            : base(options)
        {

        }

        public DbSet<Key> keys { get; set; }
    }
}
