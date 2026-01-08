using Microsoft.EntityFrameworkCore;
using EnterpriseTaskManager.API.Models.Entities;

namespace EnterpriseTaskManager.API.Data;

/// <summary>
/// Application database context with all entity configurations
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<AIUsageLog> AIUsageLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Role).HasConversion<string>();
        });

        // TaskItem configuration
        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.Priority);
            entity.HasIndex(e => e.DueDate);
            entity.HasIndex(e => e.CreatedAt);

            entity.Property(e => e.Status).HasConversion<string>();
            entity.Property(e => e.Priority).HasConversion<string>();
            entity.Property(e => e.SentimentScore).HasConversion<string>();

            // Relationships
            entity.HasOne(e => e.CreatedBy)
                .WithMany(u => u.CreatedTasks)
                .HasForeignKey(e => e.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.AssignedTo)
                .WithMany(u => u.AssignedTasks)
                .HasForeignKey(e => e.AssignedToId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.Category)
                .WithMany(c => c.Tasks)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Comment configuration
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasIndex(e => e.TaskId);
            entity.HasIndex(e => e.CreatedAt);
        });

        // RefreshToken configuration
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasIndex(e => e.Token).IsUnique();
            entity.HasIndex(e => e.ExpiresAt);
        });

        // AIUsageLog configuration
        modelBuilder.Entity<AIUsageLog>(entity =>
        {
            entity.HasIndex(e => e.FeatureType);
            entity.HasIndex(e => e.CreatedAt);
        });

        // Seed data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Create admin user (password: Admin123!)
        var adminId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var adminUser = new User
        {
            Id = adminId,
            Name = "Admin User",
            Email = "admin@taskmanager.com",
            // BCrypt hash for "Admin123!"
            PasswordHash = "$2a$11$J5kXvf3YxqLzH.xB8Z9X0.F8Y5gZ6qK2tJ9pN1wL5vH3xR2mQ8dKe",
            Role = UserRole.Admin,
            Department = "IT",
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        // Create default categories
        var categories = new[]
        {
            new Category
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000001"),
                Name = "Bug",
                Description = "Software bugs and defects",
                Color = "#EF4444",
                CreatedAt = DateTime.UtcNow
            },
            new Category
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000002"),
                Name = "Feature",
                Description = "New feature requests",
                Color = "#8B5CF6",
                CreatedAt = DateTime.UtcNow
            },
            new Category
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000003"),
                Name = "Enhancement",
                Description = "Improvements to existing features",
                Color = "#06B6D4",
                CreatedAt = DateTime.UtcNow
            },
            new Category
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000004"),
                Name = "Documentation",
                Description = "Documentation updates",
                Color = "#10B981",
                CreatedAt = DateTime.UtcNow
            },
            new Category
            {
                Id = Guid.Parse("10000000-0000-0000-0000-000000000005"),
                Name = "Support",
                Description = "Customer support tickets",
                Color = "#F59E0B",
                CreatedAt = DateTime.UtcNow
            }
        };

        modelBuilder.Entity<User>().HasData(adminUser);
        modelBuilder.Entity<Category>().HasData(categories);
    }

    /// <summary>
    /// Override SaveChanges to automatically update UpdatedAt timestamp
    /// </summary>
    public override int SaveChanges()
    {
        UpdateTimestamps();
        return base.SaveChanges();
    }

    /// <summary>
    /// Override SaveChangesAsync to automatically update UpdatedAt timestamp
    /// </summary>
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is TaskItem && e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            ((TaskItem)entry.Entity).UpdatedAt = DateTime.UtcNow;
        }
    }
}
