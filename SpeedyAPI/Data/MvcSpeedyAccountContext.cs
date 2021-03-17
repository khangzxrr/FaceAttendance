using Microsoft.EntityFrameworkCore;

namespace SpeedyAPI.Data
{
    public class MvcSpeedyAccountContext : DbContext
    {
        public MvcSpeedyAccountContext(DbContextOptions<MvcSpeedyAccountContext> options)
           : base(options)
        {

        }

    }
}
