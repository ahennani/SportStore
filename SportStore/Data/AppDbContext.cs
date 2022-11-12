namespace SportStore.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) :base(options)
    {}

    public DbSet<User> Users { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(user =>
        {
            user.HasMany(u => u.Oders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId);
        });

        modelBuilder.Entity<Category>(category =>
        {
            category.HasMany(c => c.Products)
                    .WithOne(p => p.Category)
                    .HasForeignKey(p => p.CategoryId);

            category.HasIndex(c => c.Name).IsUnique();
        });


        //// TODO: Fix the relationship between Order & Product
        modelBuilder.Entity<Order>(order =>
        {
            order.HasMany(o => o.Products)
                 .WithOne(p => p.Order)
                 .HasForeignKey(o => o.OrderId)
                 .IsRequired(false);

            order.HasOne(o => o.User);
        });

        modelBuilder.Entity<Product>(product =>
        {
            product.HasIndex(p => p.SKU).IsUnique();
            product.Property(p => p.Price)
                   .HasPrecision(10, 2);
        });
    }
}
