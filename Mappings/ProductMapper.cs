using DataModels;
using DTOs;
using AmazonProduct = DataModels.AmazonProduct;

namespace Mappings;

public class ProductMapper
{
    public static AmazonProduct FromAmazon(AmazonProductDto p) => new AmazonProduct
    {
        Id = Guid.NewGuid(),
        Name = p.Name,
        Price = p.Price,
        Currency = p.Currency,
        CustomerReview = p.CustomerReview,
        ReviewCount = p.CustomerReviewCount,
        ImageLink = p.Image,
        ProductLink = p.AmazonLink
    };

    public static WalmartProduct FromWalmart(WalmartProductDto p) => new WalmartProduct
    {
        Id = Guid.NewGuid(),
        Name = p.Title,
        Price = p.Price.CurrentPrice,
        Currency = p.Price.Currency,
        CustomerReview = "",
        ReviewCount = p.ReviewsCount,
        ImageLink = p.Image,
        ProductLink = p.Link
    };

    public static ReweProduct FromRewe(ReweProductDto p) => new ReweProduct
    {
        Id = Guid.NewGuid(),
        Name = p.ProductName,
        Price = p.Cost,
        Currency = p.Currency,
        CustomerReview = p.Review,
        ReviewCount = p.ReviewCount,
        ImageLink = p.ImageUrl,
        ProductLink = p.ProductUrl
    };
}