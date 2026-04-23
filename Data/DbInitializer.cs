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
            // Flowers (15)
            new Product { Name = "Red Rose Bouquet", Description = "A dozen fresh red roses in a beautiful arrangement.", Price = 1200.00m, ImageUrl = "https://images.unsplash.com/photo-1455659817273-f96807779a8a?w=400&h=300&fit=crop", Category = ProductCategory.Flower, ArrangementStyle = "Bouquet", StockQuantity = 15 },
            new Product { Name = "Crocheted Tulip Bouquet", Description = "A beautiful handmade bouquet of pink and purple crocheted tulips wrapped in elegant red paper.", Price = 950.00m, ImageUrl = "https://images.unsplash.com/photo-1518709268805-4e9042af9f23?w=400&h=300&fit=crop", Category = ProductCategory.Flower, ArrangementStyle = "Bouquet", StockQuantity = 5 },
            new Product { Name = "White Lily Elegance", Description = "Elegant white lilies in a clear glass vase.", Price = 1500.00m, ImageUrl = "https://images.unsplash.com/photo-1490750967868-88df5691548f?w=400&h=300&fit=crop", Category = ProductCategory.Flower, ArrangementStyle = "Vase", StockQuantity = 10 },
            new Product { Name = "Sunflower Sunshine", Description = "Bright sunflowers to bring joy to any room.", Price = 850.00m, ImageUrl = "https://images.unsplash.com/photo-1508739773434-c26b3d09e071?w=400&h=300&fit=crop", Category = ProductCategory.Flower, ArrangementStyle = "Bouquet", StockQuantity = 12 },
            new Product { Name = "Purple Orchid Vase", Description = "Exotic purple orchids gracefully arranged.", Price = 1800.00m, ImageUrl = "https://images.unsplash.com/photo-1566842600778-4f921af1a666?w=400&h=300&fit=crop", Category = ProductCategory.Flower, ArrangementStyle = "Vase", StockQuantity = 8 },
            new Product { Name = "Pink Peony Charm", Description = "Lush pink peonies perfect for a romantic gesture.", Price = 1400.00m, ImageUrl = "https://images.unsplash.com/photo-1488459716781-31db52582fe9?w=400&h=300&fit=crop", Category = ProductCategory.Flower, ArrangementStyle = "Bouquet", StockQuantity = 6 },
            new Product { Name = "Blue Hydrangea Bliss", Description = "Voluminous blue hydrangeas wrapped in rustic paper.", Price = 1100.00m, ImageUrl = "https://images.unsplash.com/photo-1444510258215-5c82f5d87aae?w=400&h=300&fit=crop", Category = ProductCategory.Flower, ArrangementStyle = "Bouquet", StockQuantity = 10 },
            new Product { Name = "Daisy Delight", Description = "A cheerful basket full of fresh daisies.", Price = 650.00m, ImageUrl = "https://images.unsplash.com/photo-1490474418585-ba9bad8fd0ea?w=400&h=300&fit=crop", Category = ProductCategory.Flower, ArrangementStyle = "Basket", StockQuantity = 20 },
            new Product { Name = "Mixed Wildflower Basket", Description = "A rustic mix of colorful seasonal wildflowers.", Price = 900.00m, ImageUrl = "https://images.unsplash.com/photo-1499360685094-97ef0f69a671?w=400&h=300&fit=crop", Category = ProductCategory.Flower, ArrangementStyle = "Basket", StockQuantity = 15 },
            new Product { Name = "Crimson Carnations", Description = "Classic red carnations for any occasion.", Price = 750.00m, ImageUrl = "https://images.unsplash.com/photo-1455659817273-f96807779a8a?w=400&h=300&fit=crop&sig=0", Category = ProductCategory.Flower, ArrangementStyle = "Bouquet", StockQuantity = 18 },
            new Product { Name = "Pastel Rose Medley", Description = "A soft mix of pink, white, and peach roses.", Price = 1350.00m, ImageUrl = "https://images.unsplash.com/photo-1455659817273-f96807779a8a?w=400&h=300&fit=crop&sig=1", Category = ProductCategory.Flower, ArrangementStyle = "Bouquet", StockQuantity = 12 },
            new Product { Name = "Golden Chrysanthemum", Description = "Bright golden mums arranged in a classic vase.", Price = 800.00m, ImageUrl = "https://images.unsplash.com/photo-1455659817273-f96807779a8a?w=400&h=300&fit=crop&sig=2", Category = ProductCategory.Flower, ArrangementStyle = "Vase", StockQuantity = 14 },
            new Product { Name = "Lavender Dream", Description = "Fresh, fragrant lavender bundled beautifully.", Price = 700.00m, ImageUrl = "https://images.unsplash.com/photo-1455659817273-f96807779a8a?w=400&h=300&fit=crop&sig=3", Category = ProductCategory.Flower, ArrangementStyle = "Bouquet", StockQuantity = 25 },
            new Product { Name = "Radiant Ranunculus", Description = "Vibrant ranunculus flowers in an elegant pot.", Price = 1250.00m, ImageUrl = "https://images.unsplash.com/photo-1455659817273-f96807779a8a?w=400&h=300&fit=crop&sig=4", Category = ProductCategory.Flower, ArrangementStyle = "Pot", StockQuantity = 7 },
            new Product { Name = "Tropical Hibiscus", Description = "A taste of the tropics with colorful hibiscus.", Price = 950.00m, ImageUrl = "https://images.unsplash.com/photo-1455659817273-f96807779a8a?w=400&h=300&fit=crop&sig=5", Category = ProductCategory.Flower, ArrangementStyle = "Vase", StockQuantity = 9 },

            // Cakes (25)
            new Product { Name = "Classic Chocolate Cake", Description = "Rich and moist chocolate cake layers.", Price = 850.00m, ImageUrl = "https://images.unsplash.com/photo-1578985545062-6d596e2f1a5f?w=400&h=300&fit=crop", Category = ProductCategory.Cake, SizeOptions = "Small, Medium, Large", StockQuantity = 10 },
            new Product { Name = "Vanilla Bean Dream", Description = "Light vanilla sponge with real vanilla bean buttercream.", Price = 750.00m, ImageUrl = "https://images.unsplash.com/photo-1565958011703-44f9829ba187?w=400&h=300&fit=crop", Category = ProductCategory.Cake, SizeOptions = "Small, Medium", StockQuantity = 12 },
            new Product { Name = "Red Velvet Romance", Description = "Classic red velvet with tangy cream cheese frosting.", Price = 900.00m, ImageUrl = "https://images.unsplash.com/photo-1586788680434-30d324b2d46f?w=400&h=300&fit=crop", Category = ProductCategory.Cake, SizeOptions = "Medium, Large", StockQuantity = 8 },
            new Product { Name = "Strawberry Shortcake", Description = "Fresh strawberries and whipped cream layered in sponge.", Price = 850.00m, ImageUrl = "https://images.unsplash.com/photo-1488477181946-6428a0291777?w=400&h=300&fit=crop", Category = ProductCategory.Cake, SizeOptions = "Medium", StockQuantity = 10 },
            new Product { Name = "Lemon Chiffon", Description = "Zesty and airy lemon cake with citrus glaze.", Price = 700.00m, ImageUrl = "https://images.unsplash.com/photo-1567620905732-2d1ec7ab7445?w=400&h=300&fit=crop", Category = ProductCategory.Cake, SizeOptions = "Small, Medium", StockQuantity = 6 },
            new Product { Name = "Black Forest Gateau", Description = "Chocolate sponge, cherries, and fresh whipped cream.", Price = 1000.00m, ImageUrl = "https://images.unsplash.com/photo-1606313462796-b29b985f7db7?w=400&h=300&fit=crop", Category = ProductCategory.Cake, SizeOptions = "Large", StockQuantity = 5 },
            new Product { Name = "Tiramisu Truffle", Description = "Coffee-soaked ladyfingers with mascarpone layers.", Price = 1200.00m, ImageUrl = "https://images.unsplash.com/photo-1571877227200-a0d98ea607e9?w=400&h=300&fit=crop", Category = ProductCategory.Cake, SizeOptions = "Medium", StockQuantity = 8 },
            new Product { Name = "Carrot Cake Classic", Description = "Spiced carrot cake packed with walnuts and raisins.", Price = 800.00m, ImageUrl = "https://images.unsplash.com/photo-1621743478914-cc8a86d7e7b5?w=400&h=300&fit=crop", Category = ProductCategory.Cake, SizeOptions = "Medium, Large", StockQuantity = 10 },
            new Product { Name = "Matcha Green Tea", Description = "Delicate matcha flavor in a smooth mousse cake.", Price = 1100.00m, ImageUrl = "https://images.unsplash.com/photo-1556909114-f6e7ad7d3136?w=400&h=300&fit=crop", Category = ProductCategory.Cake, SizeOptions = "Small, Medium", StockQuantity = 7 },
            new Product { Name = "Coffee Walnut", Description = "Rich coffee sponge layered with espresso buttercream.", Price = 850.00m, ImageUrl = "https://images.unsplash.com/photo-1578985545062-6d596e2f1a5f?w=400&h=300&fit=crop&sig=0", Category = ProductCategory.Cake, SizeOptions = "Medium", StockQuantity = 9 },
            new Product { Name = "Blueberry Cheesecake", Description = "Creamy New York style cheesecake with blueberry topping.", Price = 1300.00m, ImageUrl = "https://images.unsplash.com/photo-1578985545062-6d596e2f1a5f?w=400&h=300&fit=crop&sig=1", Category = ProductCategory.Cake, SizeOptions = "Large", StockQuantity = 12 },
            new Product { Name = "Mango Mousse Cake", Description = "Refreshing mango layers in a light mousse setting.", Price = 1150.00m, ImageUrl = "https://images.unsplash.com/photo-1578985545062-6d596e2f1a5f?w=400&h=300&fit=crop&sig=2", Category = ProductCategory.Cake, SizeOptions = "Medium", StockQuantity = 8 },
            new Product { Name = "Salted Caramel Crunch", Description = "Caramel cake topped with sea salt and toffee bits.", Price = 950.00m, ImageUrl = "https://images.unsplash.com/photo-1578985545062-6d596e2f1a5f?w=400&h=300&fit=crop&sig=3", Category = ProductCategory.Cake, SizeOptions = "Medium, Large", StockQuantity = 10 },
            new Product { Name = "Coconut Cream", Description = "Fluffy coconut layers topped with toasted coconut flakes.", Price = 800.00m, ImageUrl = "https://images.unsplash.com/photo-1578985545062-6d596e2f1a5f?w=400&h=300&fit=crop&sig=4", Category = ProductCategory.Cake, SizeOptions = "Small, Medium", StockQuantity = 6 },
            new Product { Name = "Pistachio Rose", Description = "Elegant pistachio sponge with subtle rosewater frosting.", Price = 1400.00m, ImageUrl = "https://images.unsplash.com/photo-1578985545062-6d596e2f1a5f?w=400&h=300&fit=crop&sig=5", Category = ProductCategory.Cake, SizeOptions = "Medium", StockQuantity = 5 },
            new Product { Name = "Hazelnut Praline", Description = "Decadent hazelnut chocolate cake with praline crunch.", Price = 1250.00m, ImageUrl = "https://images.unsplash.com/photo-1578985545062-6d596e2f1a5f?w=400&h=300&fit=crop&sig=6", Category = ProductCategory.Cake, SizeOptions = "Large", StockQuantity = 7 },
            new Product { Name = "Raspberry Ripple", Description = "Vanilla sponge rippled with fresh raspberry jam.", Price = 900.00m, ImageUrl = "https://images.unsplash.com/photo-1578985545062-6d596e2f1a5f?w=400&h=300&fit=crop&sig=7", Category = ProductCategory.Cake, SizeOptions = "Medium", StockQuantity = 9 },
            new Product { Name = "Earl Grey Chiffon", Description = "Sophisticated tea-infused cake with honey glaze.", Price = 850.00m, ImageUrl = "https://images.unsplash.com/photo-1578985545062-6d596e2f1a5f?w=400&h=300&fit=crop&sig=8", Category = ProductCategory.Cake, SizeOptions = "Small", StockQuantity = 5 },
            new Product { Name = "Cookies and Cream", Description = "Chocolate cake layered with crushed cookie frosting.", Price = 950.00m, ImageUrl = "https://images.unsplash.com/photo-1578985545062-6d596e2f1a5f?w=400&h=300&fit=crop&sig=9", Category = ProductCategory.Cake, SizeOptions = "Medium, Large", StockQuantity = 15 },
            new Product { Name = "Pineapple Upside Down", Description = "Retro classic with caramelized pineapple rings.", Price = 750.00m, ImageUrl = "https://images.unsplash.com/photo-1565958011703-44f9829ba187?w=400&h=300&fit=crop&sig=0", Category = ProductCategory.Cake, SizeOptions = "Medium", StockQuantity = 8 },
            new Product { Name = "Funfetti Birthday", Description = "Colorful sprinkles baked right into the vanilla cake.", Price = 800.00m, ImageUrl = "https://images.unsplash.com/photo-1565958011703-44f9829ba187?w=400&h=300&fit=crop&sig=1", Category = ProductCategory.Cake, SizeOptions = "Medium, Large", StockQuantity = 10 },
            new Product { Name = "Dark Chocolate Mud", Description = "Dense, fudgy mud cake for serious chocolate lovers.", Price = 1100.00m, ImageUrl = "https://images.unsplash.com/photo-1565958011703-44f9829ba187?w=400&h=300&fit=crop&sig=2", Category = ProductCategory.Cake, SizeOptions = "Small, Medium", StockQuantity = 6 },
            new Product { Name = "White Chocolate Raspberry", Description = "White chocolate cake dotted with fresh raspberries.", Price = 1050.00m, ImageUrl = "https://images.unsplash.com/photo-1565958011703-44f9829ba187?w=400&h=300&fit=crop&sig=3", Category = ProductCategory.Cake, SizeOptions = "Medium", StockQuantity = 8 },
            new Product { Name = "Caramel Apple Spice", Description = "Warmly spiced apple cake drizzled in caramel.", Price = 900.00m, ImageUrl = "https://images.unsplash.com/photo-1565958011703-44f9829ba187?w=400&h=300&fit=crop&sig=4", Category = ProductCategory.Cake, SizeOptions = "Medium", StockQuantity = 12 },
            new Product { Name = "Opera Cake", Description = "French classic with almond sponge, coffee syrup, and chocolate.", Price = 1500.00m, ImageUrl = "https://images.unsplash.com/photo-1565958011703-44f9829ba187?w=400&h=300&fit=crop&sig=5", Category = ProductCategory.Cake, SizeOptions = "Small", StockQuantity = 4 },

            // Chocolates (15)
            new Product { Name = "Dark Chocolate Truffles", Description = "Hand-rolled 70% dark chocolate truffles.", Price = 550.00m, ImageUrl = "https://images.unsplash.com/photo-1511381939415-e44a59fe3c67?w=400&h=300&fit=crop", Category = ProductCategory.Chocolate, StockQuantity = 30 },
            new Product { Name = "Milk Chocolate Assortment", Description = "A classic box of assorted milk chocolate favorites.", Price = 600.00m, ImageUrl = "https://images.unsplash.com/photo-1548741487-18d2bd769e47?w=400&h=300&fit=crop", Category = ProductCategory.Chocolate, StockQuantity = 25 },
            new Product { Name = "White Chocolate Raspberry Bark", Description = "Creamy white chocolate bark studded with freeze-dried raspberries.", Price = 450.00m, ImageUrl = "https://images.unsplash.com/photo-1549007994-bfece2a9c5a4?w=400&h=300&fit=crop", Category = ProductCategory.Chocolate, StockQuantity = 20 },
            new Product { Name = "Hazelnut Praline Box", Description = "Luxurious chocolates filled with smooth hazelnut praline.", Price = 750.00m, ImageUrl = "https://images.unsplash.com/photo-1606312619070-d421acac5a8c?w=400&h=300&fit=crop", Category = ProductCategory.Chocolate, StockQuantity = 15 },
            new Product { Name = "Sea Salt Caramel Chocolates", Description = "Dark chocolate domes filled with gooey salted caramel.", Price = 650.00m, ImageUrl = "https://images.unsplash.com/photo-1481391319250-a5db7e2e5b46?w=400&h=300&fit=crop", Category = ProductCategory.Chocolate, StockQuantity = 22 },
            new Product { Name = "Mint Chocolate Squares", Description = "Refreshing mint fondant coated in crisp dark chocolate.", Price = 400.00m, ImageUrl = "https://images.unsplash.com/photo-1575377427642-087cf684b79d?w=400&h=300&fit=crop", Category = ProductCategory.Chocolate, StockQuantity = 18 },
            new Product { Name = "Almond Bark", Description = "Roasted almonds smothered in rich milk chocolate.", Price = 450.00m, ImageUrl = "https://images.unsplash.com/photo-1587314168485-3236d6da7243?w=400&h=300&fit=crop", Category = ProductCategory.Chocolate, StockQuantity = 20 },
            new Product { Name = "Orange Zest Dark Chocolate", Description = "Intense dark chocolate infused with natural orange oils.", Price = 500.00m, ImageUrl = "https://images.unsplash.com/photo-1499363536502-87642509e31b?w=400&h=300&fit=crop", Category = ProductCategory.Chocolate, StockQuantity = 25 },
            new Product { Name = "Espresso Chocolate Beans", Description = "Real roasted coffee beans dipped in dark chocolate.", Price = 350.00m, ImageUrl = "https://images.unsplash.com/photo-1514432707541-4a2f19bc4e48?w=400&h=300&fit=crop", Category = ProductCategory.Chocolate, StockQuantity = 35 },
            new Product { Name = "Strawberry Chocolate Hearts", Description = "Sweet strawberry flavored chocolate in romantic heart shapes.", Price = 450.00m, ImageUrl = "https://images.unsplash.com/photo-1511381939415-e44a59fe3c67?w=400&h=300&fit=crop&sig=0", Category = ProductCategory.Chocolate, StockQuantity = 30 },
            new Product { Name = "Coconut Cream Chocolates", Description = "Tropical coconut fondant surrounded by milk chocolate.", Price = 480.00m, ImageUrl = "https://images.unsplash.com/photo-1511381939415-e44a59fe3c67?w=400&h=300&fit=crop&sig=1", Category = ProductCategory.Chocolate, StockQuantity = 15 },
            new Product { Name = "Peanut Butter Cups", Description = "Gourmet, thick-shelled milk chocolate peanut butter cups.", Price = 420.00m, ImageUrl = "https://images.unsplash.com/photo-1511381939415-e44a59fe3c67?w=400&h=300&fit=crop&sig=2", Category = ProductCategory.Chocolate, StockQuantity = 40 },
            new Product { Name = "Pistachio Chocolate Squares", Description = "Smooth milk chocolate paired with crunchy pistachios.", Price = 550.00m, ImageUrl = "https://images.unsplash.com/photo-1511381939415-e44a59fe3c67?w=400&h=300&fit=crop&sig=3", Category = ProductCategory.Chocolate, StockQuantity = 20 },
            new Product { Name = "Ruby Chocolate Gems", Description = "Trendy ruby chocolate with natural fruity notes.", Price = 800.00m, ImageUrl = "https://images.unsplash.com/photo-1511381939415-e44a59fe3c67?w=400&h=300&fit=crop&sig=4", Category = ProductCategory.Chocolate, StockQuantity = 10 },
            new Product { Name = "Vegan Dark Chocolate Bar", Description = "100% plant-based artisanal dark chocolate bar.", Price = 380.00m, ImageUrl = "https://images.unsplash.com/photo-1511381939415-e44a59fe3c67?w=400&h=300&fit=crop&sig=5", Category = ProductCategory.Chocolate, StockQuantity = 25 }
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
