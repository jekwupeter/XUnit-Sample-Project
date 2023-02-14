using Microsoft.EntityFrameworkCore;
using TimiTest.Models;

namespace TimiTest.Data
{
    public class KitchenDbContext : DbContext
    {
        public KitchenDbContext(DbContextOptions<KitchenDbContext> options): base(options)
        {}

        public DbSet<Kitchen> Kitchens { get; set; }
    }
}
