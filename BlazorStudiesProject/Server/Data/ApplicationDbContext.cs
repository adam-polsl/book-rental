using BlazorStudiesProject.Server.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorStudiesProject.Server.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{
    public virtual DbSet<BookEntity> Books { get; set; }
    public virtual DbSet<AdditionalInfoEntity> AdditionalInfos { get; set; }
    public virtual DbSet<BookUserInfoEntity> BookUserInfos { get; set; }
    public virtual DbSet<ContentEntity> Contents { get; set; }
    public virtual DbSet<ActivePurchaseEntity> ActivePurchases { get; set; }
    public virtual DbSet<ArchivePurchaseEntity> ArchivePurchases { get; set; }
    public virtual DbSet<CodeEntity> Codes { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder
            .Entity<BookUserInfoEntity>()
            .HasKey(sc => new { sc.UserId, sc.BookId });

        builder
            .Entity<BookUserInfoEntity>()
            .HasOne<BookEntity>(bu => bu.Book)
            .WithMany(ui => ui.BookUserInfos)
            .HasForeignKey(bu => bu.BookId);

        builder
            .Entity<BookUserInfoEntity>()
            .HasOne<ApplicationUser>(u => u.User)
            .WithMany(ui => ui.BookUserInfos)
            .HasForeignKey(bu => bu.UserId);

        builder
            .Entity<ActivePurchaseEntity>()
            .ToTable("Active_Purchases");

        builder
            .Entity<ArchivePurchaseEntity>()
            .ToTable("Archive_Purchases");

        builder
            .Entity<CodeEntity>()
            .HasIndex(c => c.Code)
            .IsUnique(true);
    }
}
