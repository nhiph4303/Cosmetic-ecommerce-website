using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shop.Models;

namespace Cosmetic.Data
{
    public class CosmeticContext : DbContext 
    {
        public CosmeticContext(DbContextOptions<CosmeticContext> options) : base(options)
        {
        }

        public DbSet<Category> Category { get; set; } = default!;
        public DbSet<Product> Product { get; set; } = default!;
        public DbSet<Order> Order { get; set; } = default!;
        public DbSet<Customer> Customer { get; set; } = default!;
    }
}
