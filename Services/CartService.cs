using CakeFlowerShop.Models;
using Microsoft.JSInterop;
using System.Text.Json;

namespace CakeFlowerShop.Services;

public class CartItem
{
    public Product Product { get; set; } = default!;
    public int Quantity { get; set; }
    public decimal TotalPrice => Product.Price * Quantity;
}

public class CartService(IJSRuntime js)
{
    private List<CartItem> _items = new();
    private bool _isInitialized;
    // You can now use 'js' directly in your methods without needing a separate '_js' field.

    public IReadOnlyList<CartItem> Items => _items.AsReadOnly();

    public event Action? OnChange;

    public async Task InitializeAsync()
    {
        if (_isInitialized) return;
        
        try 
        {
            var json = await js.InvokeAsync<string>("localStorage.getItem", "cart");
            if (!string.IsNullOrEmpty(json))
            {
                _items = JsonSerializer.Deserialize<List<CartItem>>(json) ?? new();
                NotifyStateChanged();
            }
        }
        catch { /* Ignore during pre-rendering */ }
        
        _isInitialized = true;
    }

    private async Task SaveAsync()
    {
        try 
        {
            if (js != null)
            {
                var json = JsonSerializer.Serialize(_items);
                await js.InvokeVoidAsync("localStorage.setItem", "cart", json);
            }
        }
        catch { /* JS not ready */ }
    }

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
        _ = SaveAsync();
    }

    public void RemoveFromCart(int productId)
    {
        var item = _items.FirstOrDefault(i => i.Product.Id == productId);
        if (item != null)
        {
            _items.Remove(item);
            NotifyStateChanged();
            _ = SaveAsync();
        }
    }

    public void ClearCart()
    {
        _items.Clear();
        NotifyStateChanged();
        _ = SaveAsync();
    }

    public int GetTotalCount() => _items.Sum(i => i.Quantity);
    
    public decimal GetTotalAmount() => _items.Sum(i => i.TotalPrice);

    private void NotifyStateChanged() => OnChange?.Invoke();
}
