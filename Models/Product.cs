namespace CakeFlowerShop.Models;

public enum ProductCategory
{
    Cake,
    Flower,
    Chocolate
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public ProductCategory Category { get; set; }

    // Cake specific
    public string? SizeOptions { get; set; } // e.g. "Small, Medium, Large"

    // Flower specific
    public string? ArrangementStyle { get; set; } // e.g. "Bouquet, Vase, Basket"

    public int StockQuantity { get; set; }
}
