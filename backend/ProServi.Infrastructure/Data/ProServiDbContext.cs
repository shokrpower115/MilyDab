using Microsoft.EntityFrameworkCore;
using ProServi.Domain.Entities;
using ProServi.Domain.Enums;

namespace ProServi.Infrastructure.Data;

public class ProServiDbContext : DbContext
{
    public ProServiDbContext(DbContextOptions<ProServiDbContext> options) 
        : base(options) { }

    // DbSets para todas las entidades
    public DbSet<User> Users { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Professional> Professionals { get; set; }
    public DbSet<ContactRequest> ContactRequests { get; set; }
    public DbSet<Budget> Budgets { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Certification> Certifications { get; set; }
    public DbSet<Specialty> Specialties { get; set; }
    public DbSet<SavedCard> SavedCards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.Phone).IsUnique();
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
            entity.Property(e => e.PasswordHash).IsRequired();
            entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Status).HasConversion<int>();
            entity.Property(e => e.Role).HasConversion<int>();
        });

        // Configuración de Customer
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.User)
                .WithOne(u => u.Customer)
                .HasForeignKey<Customer>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.Property(e => e.City).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Country).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Latitude).HasPrecision(10, 8);
            entity.Property(e => e.Longitude).HasPrecision(11, 8);
        });

        // Configuración de Professional
        modelBuilder.Entity<Professional>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.User)
                .WithOne(u => u.Professional)
                .HasForeignKey<Professional>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Specialty)
                .WithMany(s => s.Professionals)
                .HasForeignKey(e => e.SpecialtyId);
            entity.Property(e => e.City).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Country).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Latitude).HasPrecision(10, 8);
            entity.Property(e => e.Longitude).HasPrecision(11, 8);
            entity.Property(e => e.Bio).HasMaxLength(500);
        });

        // Seeding de datos iniciales
        SeedSpecialties(modelBuilder);
    }

    private void SeedSpecialties(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Specialty>().HasData(
            new Specialty { Id = 1, Name = "Plomería", Description = "Servicios de reparación y instalación de tuberías" },
            new Specialty { Id = 2, Name = "Electricidad", Description = "Trabajos eléctricos y reparación" },
            new Specialty { Id = 3, Name = "Pintura", Description = "Servicios de pintura interior y exterior" },
            new Specialty { Id = 4, Name = "Carpintería", Description = "Trabajos de carpintería y muebles" },
            new Specialty { Id = 5, Name = "Limpieza", Description = "Servicios de limpieza profesional" },
            new Specialty { Id = 6, Name = "Reparación General", Description = "Reparaciones varias en el hogar" }
        );
    }
}
