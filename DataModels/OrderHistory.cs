namespace DataModels;

public class OrderHistory
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string OrderId { get; set; } = string.Empty;
    public string AuthenticationUid { get; set; } = string.Empty;
    public double TotalOrderPrice { get; set; }
    public bool IsPaymentCompleted { get; set; }
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDateTime { get; set; }
}
