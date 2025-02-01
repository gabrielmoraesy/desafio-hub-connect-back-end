using api_products_hub_connect.Models;
using Microsoft.EntityFrameworkCore;

namespace api_products_hub_connect.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<ProductModel> Products { get; set; }
    }
}
