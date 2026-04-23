namespace CakeFlowerShop.Models;

public enum PaymentMethod
{
    GCash,
    CashOnDelivery,
    Card
}

public enum PaymentStatus
{
    Pending,
    Paid,
    Failed
}

public class Payment
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; } = default!;
    
    public PaymentMethod Method { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public DateTime? PaymentDate { get; set; }
    public string? TransactionId { get; set; } // For GCash
}
