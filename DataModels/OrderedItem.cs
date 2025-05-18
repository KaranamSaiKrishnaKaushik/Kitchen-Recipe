namespace DataModels;

public class OrderedItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string AuthenticationUid { get; set; } = string.Empty;
    public string OrderId { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string ImageLink { get; set; } = string.Empty;
    public string ProductId { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool IsOnSale { get; set; }
    public string SourceName { get; set; } = string.Empty;
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDateTime { get; set; }
}
