using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SecureFileSharingSystem.Persistence.Entities;

namespace SecureFileSharingSystem.Persistence;
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    // DbSets for your custom entities
    public DbSet<FileRecord> FileRecords { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Example configuration
        builder.Entity<FileRecord>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FileName).IsRequired().HasMaxLength(255);
            entity.Property(e => e.StoredFileName).IsRequired();
            entity.Property(e => e.UploadedAt).HasDefaultValueSql("GETUTCDATE()");
        });

        builder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Action).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Timestamp).HasDefaultValueSql("GETUTCDATE()");
        });
    }
}