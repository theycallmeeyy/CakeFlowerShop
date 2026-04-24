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

public class CartService
{
    private List<CartItem> _items = new();
    private readonly IJSRuntime _js;
    private bool _isInitialized;

    public CartService(IJSRuntime js)
    {
        _js = js;
    }

    public IReadOnlyList<CartItem> Items => _items.AsReadOnly();

    public event Action? OnChange;

    public async Task InitializeAsync()
    {
        if (_isInitialized) return;
        
        try 
        {
            var json = await _js.InvokeAsync<string>("localStorage.getItem", "cart");
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
            var json = JsonSerializer.Serialize(_items);
            await _js.InvokeVoidAsync("localStorage.setItem", "cart", json);
        }
        catch { /* Ignore during pre-rendering */ }
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
