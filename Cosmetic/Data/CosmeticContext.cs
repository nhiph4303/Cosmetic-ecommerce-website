using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Shop.Models;

namespace Cosmetic.Data
{
    public class CosmeticContext : DbContext
    {
        public CosmeticContext(DbContextOptions<CosmeticContext> options) : base(options)
        {
        }

        public DbSet<Shop.Models.Category> Category { get; set; } = default!;
        public DbSet<Shop.Models.Product> Product { get; set; } = default!;
        public DbSet<Shop.Models.Order> Order { get; set; } = default!;
        public DbSet<Shop.Models.Customer> Customer { get; set; } = default!;
    }
}
