using CakeFlowerShop.Models;

namespace CakeFlowerShop.Services;

public class CartItem
{
    public Product Product { get; set; } = default!;
    public int Quantity { get; set; }
    public decimal TotalPrice => Product.Price * Quantity;
}

public class CartService
{
    private List<CartItem> _items = new();

    public IReadOnlyList<CartItem> Items => _items.AsReadOnly();

    public event Action? OnChange;

    public void AddToCart(Product product, int quantity = 1)
    {
        var existingItem = _items.FirstOrDefault(i => i.Product.Id == product.Id);
        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            _items.Add(new CartItem { Product = product, Quantity = quantity });
        }
        NotifyStateChanged();
    }

    public void RemoveFromCart(int productId)
    {
        var item = _items.FirstOrDefault(i => i.Product.Id == productId);
        if (item != null)
        {
            _items.Remove(item);
            NotifyStateChanged();
        }
    }

    public void ClearCart()
    {
        _items.Clear();
        NotifyStateChanged();
    }

    public int GetTotalCount() => _items.Sum(i => i.Quantity);
    
    public decimal GetTotalAmount() => _items.Sum(i => i.TotalPrice);

    private void NotifyStateChanged() => OnChange?.Invoke();
}
