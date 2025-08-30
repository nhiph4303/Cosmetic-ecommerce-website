using Cosmetic.Enums;
using Cosmetic.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Cosmetic.Data
{
    public static class SeedData
    {

        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();


            string[] roles = { "ADMIN", "CUSTOMER" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            string adminEmail = "admin@admin";
            string password = "admin";

            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var user = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

               

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Admin");
                    var admin = new Admin
                    {
                        Gender = true,
                        Address = "Ấp A",
                        DateOfBirth = DateTime.Now,
                        IsActive = true,
                        Name = "TanCoi",
                        PhoneNumber = "0783481811",
                        User = user,
                        UserId = user.Id
                    };
                    var cosmeticContext = serviceProvider.GetRequiredService<CosmeticContext>();
                    cosmeticContext.Admin.Add(admin);
                    await cosmeticContext.SaveChangesAsync();
                }
            }
        }



        //public static void UpdateProductTypeAndVariants(ModelBuilder modelBuilder)
        //{

        //    // Seed ProductVariants
        //    modelBuilder.Entity<ProductVariant>().HasData(
        //        // Eyeshadow Palette - ID 6 (VolumeBased)
        //        new ProductVariant { Id = 1, ProductId = 6, Price = 56, InStock = 120, Name = "12ml" },
        //        new ProductVariant { Id = 2, ProductId = 6, Price = 80, InStock = 80, Name = "18ml" },
        //        new ProductVariant { Id = 3, ProductId = 6, Price = 100, InStock = 50, Name = "24ml" },

        //        // Pressed Powder - ID 7 (VolumeBased)
        //        new ProductVariant { Id = 4, ProductId = 7, Price = 35, InStock = 100, Name = "10g" },
        //        new ProductVariant { Id = 5, ProductId = 7, Price = 52, InStock = 75, Name = "15g" },
        //        new ProductVariant { Id = 6, ProductId = 7, Price = 65, InStock = 75, Name = "20g" },

        //        // Highlighter - ID 8 (VolumeBased)
        //        new ProductVariant { Id = 7, ProductId = 8, Price = 30, InStock = 70, Name = "8g" },
        //        new ProductVariant { Id = 8, ProductId = 8, Price = 45, InStock = 50, Name = "12g" },
        //        new ProductVariant { Id = 9, ProductId = 8, Price = 55, InStock = 30, Name = "15g" },

        //        // Liquid Blush - ID 9 (VolumeBased)
        //        new ProductVariant { Id = 10, ProductId = 9, Price = 22, InStock = 80, Name = "5ml" },
        //        new ProductVariant { Id = 11, ProductId = 9, Price = 35, InStock = 70, Name = "10ml" },
        //        new ProductVariant { Id = 12, ProductId = 9, Price = 50, InStock = 50, Name = "15ml" },

        //        // Mascara - ID 10 (VolumeBased)
        //        new ProductVariant { Id = 13, ProductId = 10, Price = 20, InStock = 60, Name = "6ml" },
        //        new ProductVariant { Id = 14, ProductId = 10, Price = 32, InStock = 50, Name = "10ml" },
        //        new ProductVariant { Id = 15, ProductId = 10, Price = 40, InStock = 40, Name = "12ml" },

        //        // Liquid Eyeliner - ID 11 (VolumeBased)
        //        new ProductVariant { Id = 16, ProductId = 11, Price = 15, InStock = 100, Name = "3ml" },
        //        new ProductVariant { Id = 17, ProductId = 11, Price = 25, InStock = 100, Name = "5ml" },
        //        new ProductVariant { Id = 18, ProductId = 11, Price = 35, InStock = 80, Name = "8ml" },

        //        // Creamy Strawberry Trio Lip Set - ID 12 (VolumeBased)
        //        new ProductVariant { Id = 19, ProductId = 12, Price = 53, InStock = 110, Name = "10ml" },
        //        new ProductVariant { Id = 20, ProductId = 12, Price = 75, InStock = 80, Name = "15ml" },
        //        new ProductVariant { Id = 21, ProductId = 12, Price = 90, InStock = 60, Name = "20ml" },

        //        // Glossy Lipstick - ID 13 (VolumeBased)
        //        new ProductVariant { Id = 22, ProductId = 13, Price = 20, InStock = 100, Name = "3g" },
        //        new ProductVariant { Id = 23, ProductId = 13, Price = 30, InStock = 80, Name = "5g" },
        //        new ProductVariant { Id = 24, ProductId = 13, Price = 40, InStock = 70, Name = "8g" },

        //        // Juicy Strawberry Trio Lip Set - ID 14 (VolumeBased)
        //        new ProductVariant { Id = 25, ProductId = 14, Price = 72, InStock = 90, Name = "12ml" },
        //        new ProductVariant { Id = 26, ProductId = 14, Price = 95, InStock = 80, Name = "18ml" },
        //        new ProductVariant { Id = 27, ProductId = 14, Price = 120, InStock = 80, Name = "24ml" },

        //        // Midsummer Fairytales Perfume - ID 15 (VolumeBased)
        //        new ProductVariant { Id = 28, ProductId = 15, Price = 45, InStock = 40, Name = "30ml" },
        //        new ProductVariant { Id = 29, ProductId = 15, Price = 75, InStock = 30, Name = "50ml" },
        //        new ProductVariant { Id = 30, ProductId = 15, Price = 120, InStock = 30, Name = "100ml" },

        //        // Strawberry Cupid Solid Perfume - ID 16 (VolumeBased)
        //        new ProductVariant { Id = 31, ProductId = 16, Price = 62, InStock = 40, Name = "15g" },
        //        new ProductVariant { Id = 32, ProductId = 16, Price = 90, InStock = 30, Name = "25g" },
        //        new ProductVariant { Id = 33, ProductId = 16, Price = 115, InStock = 30, Name = "35g" },

        //        // Strawberry Cupid Perfume - ID 17 (VolumeBased)
        //        new ProductVariant { Id = 34, ProductId = 17, Price = 80, InStock = 20, Name = "30ml" },
        //        new ProductVariant { Id = 35, ProductId = 17, Price = 115, InStock = 15, Name = "50ml" },
        //        new ProductVariant { Id = 36, ProductId = 17, Price = 165, InStock = 15, Name = "100ml" },

        //        // Strawberry Cupid Hand Mirror - ID 18 (VolumeBased - only one size)
        //        new ProductVariant { Id = 37, ProductId = 18, Price = 25, InStock = 150, Name = "Standard" },

        //        // Scented Hand Cream - ID 19 (VolumeBased)
        //        new ProductVariant { Id = 38, ProductId = 19, Price = 15, InStock = 100, Name = "30ml" },
        //        new ProductVariant { Id = 39, ProductId = 19, Price = 25, InStock = 80, Name = "60ml" },
        //        new ProductVariant { Id = 40, ProductId = 19, Price = 40, InStock = 70, Name = "100ml" },

        //        // Chocolate Leather Tote Bag - ID 20 (VolumeBased - only one size)
        //        new ProductVariant { Id = 41, ProductId = 20, Price = 95, InStock = 50, Name = "Standard" },

        //        // Mini Powder Puff - ID 21 (VolumeBased - different sizes)
        //        new ProductVariant { Id = 42, ProductId = 21, Price = 5, InStock = 150, Name = "Small" },
        //        new ProductVariant { Id = 43, ProductId = 21, Price = 8, InStock = 120, Name = "Medium" },
        //        new ProductVariant { Id = 44, ProductId = 21, Price = 12, InStock = 80, Name = "Large" },

        //        // Lips serum - ID 22 (VolumeBased)
        //        new ProductVariant { Id = 45, ProductId = 22, Price = 15, InStock = 20, Name = "5ml" },
        //        new ProductVariant { Id = 46, ProductId = 22, Price = 25, InStock = 15, Name = "10ml" },
        //        new ProductVariant { Id = 47, ProductId = 22, Price = 35, InStock = 15, Name = "15ml" },

        //        // Radiant Eyes & Cheeks Duo A - ID 23 (VolumeBased)
        //        new ProductVariant { Id = 48, ProductId = 23, Price = 92, InStock = 20, Name = "15g" },
        //        new ProductVariant { Id = 49, ProductId = 23, Price = 120, InStock = 15, Name = "20g" },
        //        new ProductVariant { Id = 50, ProductId = 23, Price = 145, InStock = 15, Name = "25g" }
        //    );
        //}
    }
}
