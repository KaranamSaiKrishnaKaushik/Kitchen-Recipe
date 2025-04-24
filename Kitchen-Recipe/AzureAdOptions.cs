namespace Kitchen_Recipe;

public record AzureAdOptions
{
    public string Instance { get; init; } = string.Empty;
    public string TenantId { get; init; } = string.Empty;
    public string ClientId { get; init; } = string.Empty;
}