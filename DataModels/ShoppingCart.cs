namespace DataModels;

public class ShoppingCart
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string AuthenticationUid { get; set; } = string.Empty;
    public string ProductId { get; set;}
    public int Quantity { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set; }
}