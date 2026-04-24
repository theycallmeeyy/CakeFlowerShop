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
            new Product { Name = "Red Rose Bouquet", Description = "A dozen fresh red roses in a beautiful arrangement.", Price = 1200.00m, ImageUrl = "https://images.unsplash.com/photo-1455659817273-f96807779a8a?w=400&h=300&fit=crop", Category = ProductCategory.Flower, ArrangementStyle = "Bouquet", StockQuantity = 15 },
            new Product { Name = "Crocheted Tulip Bouquet", Description = "A beautiful handmade bouquet of pink and purple crocheted tulips.", Price = 950.00m, ImageUrl = "https://images.unsplash.com/photo-1518709268805-4e9042af9f23?w=400&h=300&fit=crop", Category = ProductCategory.Flower, ArrangementStyle = "Bouquet", StockQuantity = 5 },
            new Product { Name = "White Lily Elegance", Description = "Elegant white lilies in a clear glass vase.", Price = 1500.00m, ImageUrl = "https://images.unsplash.com/photo-1490750967868-88df5691548f?w=400&h=300&fit=crop", Category = ProductCategory.Flower, ArrangementStyle = "Vase", StockQuantity = 10 },
            new Product { Name = "Sunflower Sunshine", Description = "Bright sunflowers to bring joy to any room.", Price = 850.00m, ImageUrl = "https://images.unsplash.com/photo-1508739773434-c26b3d09e071?w=400&h=300&fit=crop", Category = ProductCategory.Flower, ArrangementStyle = "Bouquet", StockQuantity = 12 },

            // Cakes (4)
            new Product { Name = "Classic Chocolate Cake", Description = "Rich and moist chocolate cake layers.", Price = 850.00m, ImageUrl = "https://images.unsplash.com/photo-1578985545062-6d596e2f1a5f?w=400&h=300&fit=crop", Category = ProductCategory.Cake, SizeOptions = "Small, Medium, Large", StockQuantity = 10 },
            new Product { Name = "Vanilla Bean Dream", Description = "Light vanilla sponge with real vanilla bean buttercream.", Price = 750.00m, ImageUrl = "https://images.unsplash.com/photo-1565958011703-44f9829ba187?w=400&h=300&fit=crop", Category = ProductCategory.Cake, SizeOptions = "Small, Medium", StockQuantity = 12 },
            new Product { Name = "Red Velvet Romance", Description = "Classic red velvet with tangy cream cheese frosting.", Price = 900.00m, ImageUrl = "https://images.unsplash.com/photo-1586788680434-30d324b2d46f?w=400&h=300&fit=crop", Category = ProductCategory.Cake, SizeOptions = "Medium, Large", StockQuantity = 8 },
            new Product { Name = "Strawberry Shortcake", Description = "Fresh strawberries and whipped cream layered in sponge.", Price = 850.00m, ImageUrl = "https://images.unsplash.com/photo-1488477181946-6428a0291777?w=400&h=300&fit=crop", Category = ProductCategory.Cake, SizeOptions = "Medium", StockQuantity = 10 },

            // Chocolates (2)
            new Product { Name = "Dark Chocolate Truffles", Description = "Hand-rolled 70% dark chocolate truffles.", Price = 550.00m, ImageUrl = "https://images.unsplash.com/photo-1511381939415-e44a59fe3c67?w=400&h=300&fit=crop", Category = ProductCategory.Chocolate, StockQuantity = 30 },
            new Product { Name = "Hazelnut Praline Box", Description = "Luxurious chocolates filled with smooth hazelnut praline.", Price = 750.00m, ImageUrl = "https://images.unsplash.com/photo-1606312619070-d421acac5a8c?w=400&h=300&fit=crop", Category = ProductCategory.Chocolate, StockQuantity = 15 }
        };

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
            }
        }
        await context.SaveChangesAsync();
    }
}
