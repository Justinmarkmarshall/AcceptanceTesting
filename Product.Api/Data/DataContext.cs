using Microsoft.EntityFrameworkCore;

namespace Product.Api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Models.Product> Products { get; set; }
    }
}
