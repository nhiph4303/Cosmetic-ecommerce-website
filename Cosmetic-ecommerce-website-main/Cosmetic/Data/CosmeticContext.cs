using System.Data;
using Cosmetic.Enums;
using Cosmetic.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Cosmetic.Data
{
    public class CosmeticContext : IdentityDbContext<IdentityUser>
    {
        public CosmeticContext(DbContextOptions<CosmeticContext> options) : base(options)
        {


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Rank>().HasData(new Rank(1, "NONE", 0.0, 0), new Rank(2, "SILVER", 2.0, 2000), new Rank(3, "GOLD", 5.0, 5000), new Rank(4, "PLATINUM", 7.0, 7000));



            modelBuilder.Entity<Product>()
                        .Property(p => p.ProductType)
                        .HasConversion<string>();

            modelBuilder.Entity<Cart>()
                .HasOne(eachCart => eachCart.Customer)
                .WithOne(eachCustomer => eachCustomer.Cart)
                .HasForeignKey<Cart>(eachCart => eachCart.CustomerId);

            modelBuilder.Entity<Cart>()
                .HasMany(eachCart => eachCart.CartItems)
                .WithOne(eachCartItem => eachCartItem.Cart)
                .HasForeignKey(eachCartItem => eachCartItem.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(eachCartItem => eachCartItem.Product)
                .WithMany(eachProduct => eachProduct.CartItems)
                .HasForeignKey(eachCartItem => eachCartItem.ProductId);

            modelBuilder.Entity<Order>()
                .HasOne(eachOrder => eachOrder.Customer)
                .WithMany(eachCustomer => eachCustomer.Orders)
                .HasForeignKey(eachOrder => eachOrder.CustomerId);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(eachOrderDetail => eachOrderDetail.Order)
                .WithMany(eachOrder => eachOrder.OrderDetails)
                .HasForeignKey(eachOrderDetail => eachOrderDetail.OrderId);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(eachOrderDetail => eachOrderDetail.Product)
                .WithMany(eachProduct => eachProduct.OrderDetails)
                .HasForeignKey(eachOrderDetail => eachOrderDetail.ProductId);

            modelBuilder.Entity<Customer>()
                .HasOne(eachCustomer => eachCustomer.User)
                .WithMany()
                .HasForeignKey(c => c.UserId);

            modelBuilder.Entity<Customer>()
                .HasOne(eachCustomer => eachCustomer.Rank)
                .WithMany(eachRank => eachRank.Customers)
                .HasForeignKey(eachCustomer => eachCustomer.RankId);


            modelBuilder.Entity<Admin>()
                .HasOne(eachAdmin => eachAdmin.User)
                .WithMany()
                .HasForeignKey(a => a.UserId);

            modelBuilder.Entity<Product>()
                .HasOne(eachProduct => eachProduct.Category)
                .WithMany(eachCategory => eachCategory.Products)
                .HasForeignKey(eachProduct => eachProduct.CategoryId);

            modelBuilder.Entity<ProductVariant>()
                .HasOne(eachProductVariant => eachProductVariant.Product)
                .WithMany(eachProduct => eachProduct.ProductVariants)
                .HasForeignKey(eachProductVariant => eachProductVariant.ProductId);

            modelBuilder.Entity<AddressShipping>()
                .HasOne(eachAddressShipping => eachAddressShipping.Customer)
                .WithMany(eachCustomer => eachCustomer.AddressShippings)
                .HasForeignKey(eachAddressShipping => eachAddressShipping.CustomerId);

            //SeedData.UpdateProductTypeAndVariants(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Category> Category { get; set; }
        public DbSet<Product> Product { get; set; }

        public DbSet<ProductVariant> ProductVariant { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Customer> Customer { get; set; }

        public DbSet<Rank> Rank { get; set; }

        public DbSet<Admin> Admin { get; set; }

        public DbSet<AddressShipping> AddressShipping { get; set; }

        public DbSet<Cart> Cart { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<OrderDetail> OrderDetail { get; set; }
    }
}
