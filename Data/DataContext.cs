using DataModels;
using Microsoft.EntityFrameworkCore;

namespace Data;

public class DataContext : DbContext
{
    public DbSet<Recipe> Recipe => Set<Recipe>();
    public DbSet<UserDetails> UserDetails => Set<UserDetails>();
    public DbSet<RecipeIngredients> RecipeIngredients => Set<RecipeIngredients>();
    public DbSet<IngredientBase> IngredientBase { get; set; }
    public DbSet<Ingredient> Ingredient { get; set; }
    
    public DbSet<AmazonProduct> AmazonProducts { get; set; }
    public DbSet<WalmartProduct> WalmartProducts { get; set; }
    public DbSet<AllStoresProducts> AllStoresProducts { get; set; }
    public DbSet<ShoppingCart> ShoppingCart => Set<ShoppingCart>();
    public DbSet<OrderedItem> OrderedItems { get; set; }
    public DbSet<OrderHistory> OrderHistories { get; set; }
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    public DataContext() { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Recipe>()
            .HasIndex(r => new { r.Name, r.AuthenticationUid })
            .IsUnique();

        modelBuilder.Entity<Recipe>()
            .HasMany(r => r.Ingredients)
            .WithOne(i => i.Recipe)
            .HasForeignKey(i => i.RecipeId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<IngredientBase>()
            .HasIndex(i => i.Name)
            .IsUnique();
        
        modelBuilder.Entity<RecipeIngredients>()
            .HasOne(i => i.BaseName)
            .WithMany(b => b.Recipes)
            .HasForeignKey(i => i.BaseNameId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<AmazonProduct>().ToTable("AmazonProducts");
        
        modelBuilder.Entity<WalmartProduct>().ToTable("WalmartProducts");
        
        modelBuilder.Entity<AllStoresProducts>().ToTable("AllStoresProducts");
        
        modelBuilder.Entity<ShoppingCart>()
            .HasIndex(r => new { r.ProductId, r.AuthenticationUid })
            .IsUnique();
    }
    
    public override int SaveChanges()
    {
        UpdateAuditFields();
        UpdateCartAuditFields();
        UpdateOrderedItemsFields();
        UpdateOrderHistoryFields();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditFields()
    {
        var entries = ChangeTracker.Entries<Recipe>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            entry.Entity.UpdatedDate = DateTime.UtcNow;

            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedDate = DateTime.UtcNow;
            }
        }
    }
    
    private void UpdateCartAuditFields()
    {
        var entries = ChangeTracker.Entries<ShoppingCart>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            entry.Entity.UpdatedDate = DateTime.UtcNow;

            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedDate = DateTime.UtcNow;
            }
        }
    }
    
    private void UpdateOrderHistoryFields()
    {
        var entries = ChangeTracker.Entries<OrderHistory>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            entry.Entity.UpdatedDateTime = DateTime.UtcNow;

            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedDateTime = DateTime.UtcNow;
            }
        }
    }
    
    private void UpdateOrderedItemsFields()
    {
        var entries = ChangeTracker.Entries<OrderedItem>()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            entry.Entity.UpdatedDateTime = DateTime.UtcNow;

            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedDateTime = DateTime.UtcNow;
            }
        }
    }
}