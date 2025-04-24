namespace DataModels;

public abstract class ProductBase
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Price { get; set; }
    public string Currency { get; set; }
    public string CustomerReview { get; set; }
    public string ReviewCount { get; set; }
    public string ImageLink { get; set; }
    public string ProductLink { get; set; }
}