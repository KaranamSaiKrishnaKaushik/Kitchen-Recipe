using System.Runtime.InteropServices.JavaScript;

namespace DataModels;

public abstract class ProductBase
{
    public Guid Id { get; set; }
    public string ProductId { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public string Currency { get; set; }
    public string? CustomerReview { get; set; }
    public string? ReviewCount { get; set; }
    public string? Grammage { get; set; }
    public string? Category { get; set; }
    public string? ImageLink { get; set; }
    public Boolean IsOnSale { get; set; }
    public string? ProductLink { get; set; }
    public string? sourceName { get; set; }
}