using CakeFlowerShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CakeFlowerShop.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

        await context.Database.MigrateAsync();

        string[] roleNames = { "Admin", "Customer" };
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        var adminEmail = "admin@shop.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new ApplicationUser { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
            await userManager.CreateAsync(adminUser, "Admin123!");
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }

        var seedProducts = new List<Product>
        {
            // Flowers (4)
            new Product { Name = "Red Rose Bouquet", Description = "A dozen fresh red roses in a beautiful arrangement.", Price = 1200.00m, ImageUrl = "images/products/red_roses.png", Category = ProductCategory.Flower, ArrangementStyle = "Bouquet", StockQuantity = 15 },
            new Product { Name = "Crocheted Tulip Bouquet", Description = "A beautiful handmade bouquet of pink and purple crocheted tulips.", Price = 950.00m, ImageUrl = "images/products/crocheted_tulip.png", Category = ProductCategory.Flower, ArrangementStyle = "Bouquet", StockQuantity = 5 },
            new Product { Name = "White Lily Elegance", Description = "Elegant white lilies in a clear glass vase.", Price = 1500.00m, ImageUrl = "images/products/white_lilies.png", Category = ProductCategory.Flower, ArrangementStyle = "Vase", StockQuantity = 10 },
            new Product { Name = "Sunflower Sunshine", Description = "Bright sunflowers to bring joy to any room.", Price = 850.00m, ImageUrl = "images/products/sunflowers.png", Category = ProductCategory.Flower, ArrangementStyle = "Bouquet", StockQuantity = 12 },

            // Cakes (4)
            new Product { Name = "Classic Chocolate Cake", Description = "Rich and moist chocolate cake layers.", Price = 850.00m, ImageUrl = "images/products/chocolate_cake.png", Category = ProductCategory.Cake, SizeOptions = "Small, Medium, Large", StockQuantity = 10 },
            new Product { Name = "Vanilla Bean Dream", Description = "Light vanilla sponge with real vanilla bean buttercream.", Price = 750.00m, ImageUrl = "images/products/vanilla_cake.png", Category = ProductCategory.Cake, SizeOptions = "Small, Medium", StockQuantity = 12 },
            new Product { Name = "Red Velvet Romance", Description = "Classic red velvet with tangy cream cheese frosting.", Price = 900.00m, ImageUrl = "images/products/red_velvet_cake.png", Category = ProductCategory.Cake, SizeOptions = "Medium, Large", StockQuantity = 8 },
            new Product { Name = "Strawberry Shortcake", Description = "Fresh strawberries and whipped cream layered in sponge.", Price = 850.00m, ImageUrl = "images/products/strawberry_cake.png", Category = ProductCategory.Cake, SizeOptions = "Medium", StockQuantity = 10 },

            // Chocolates (2)
            new Product { Name = "Dark Chocolate Truffles", Description = "Hand-rolled 70% dark chocolate truffles.", Price = 550.00m, ImageUrl = "images/products/chocolate_truffles.png", Category = ProductCategory.Chocolate, StockQuantity = 30 },
            new Product { Name = "Hazelnut Praline Box", Description = "Luxurious chocolates filled with smooth hazelnut praline.", Price = 750.00m, ImageUrl = "images/products/hazelnut_box.png", Category = ProductCategory.Chocolate, StockQuantity = 15 }
        };

        // 1. Remove products that are NOT in our new curated list
        var seedNames = seedProducts.Select(p => p.Name).ToList();
        var productsToRemove = await context.Products
            .Where(p => !seedNames.Contains(p.Name))
            .ToListAsync();
            
        if (productsToRemove.Any())
        {
            context.Products.RemoveRange(productsToRemove);
            await context.SaveChangesAsync();
        }

        // 2. Add or Update the curated products
        foreach (var sp in seedProducts)
        {
            var dbProduct = await context.Products.FirstOrDefaultAsync(p => p.Name == sp.Name);
            if (dbProduct == null)
            {
                context.Products.Add(sp);
            }
            else
            {
                dbProduct.Price = sp.Price;
                dbProduct.Category = sp.Category;
                dbProduct.Description = sp.Description;
                dbProduct.ImageUrl = sp.ImageUrl;
                dbProduct.StockQuantity = sp.StockQuantity;
            }
        }
        await context.SaveChangesAsync();
    }
}
