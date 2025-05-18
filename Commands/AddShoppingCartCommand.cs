using DataModels;

namespace Commands;

public class AddShoppingCartCommand
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string AuthenticationUid { get; set; } = string.Empty;
    public string ProductId { get; set;}
    public int Quantity { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedDate { get; set; }

    public ShoppingCart ToEntity()
    {
        var cartId = Guid.NewGuid();
        return new ShoppingCart
        {
            Id = cartId,
            AuthenticationUid = AuthenticationUid,
            ProductId = ProductId,
            Quantity = Quantity
        };
    }
}
