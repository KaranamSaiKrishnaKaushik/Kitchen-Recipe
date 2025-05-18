namespace DTOs;

public class AmazonProductDto
{
    public string Name { get; set; }
    public double Price { get; set; }
    public string Currency { get; set; }
    public string CustomerReview { get; set; }
    public string CustomerReviewCount { get; set; }
    public string AmazonLink { get; set; }
    public string Image { get; set; }
    public string? ProductId { get; set; }
    public string? Grammage { get; set; }
    public string? Category { get; set; }
    public Boolean IsOnSale { get; set; }
    public string? sourceName { get; set; }
}