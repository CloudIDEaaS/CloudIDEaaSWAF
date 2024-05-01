using Microsoft.EntityFrameworkCore;

namespace WAFWebSample.Data
{
    public class WAFWebSampleDbContext : DbContext
    {
        public virtual DbSet<WAFGlobalSettings> WAFGlobalSettings { get; set; }
        public virtual DbSet<WAFTransactionSettings> WAFTransactionSettings { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("WAFWebSampleDatabase");
        }
    }
}
