using Microsoft.EntityFrameworkCore;
using CivicAction.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CivicAction.Data;

public class CivicActionContext : IdentityDbContext<AppUser>
{
    
    public CivicActionContext(DbContextOptions<CivicActionContext> options)
        : base(options) { }

    public DbSet<Project> Projects { get; set; }
    public DbSet<Update> Updates { get; set; }
    public DbSet<Verification> Verifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<AppUser>().ToTable("Account");
        modelBuilder.Entity<Project>().ToTable("Project");
        modelBuilder.Entity<Update>().ToTable("Update");
        modelBuilder.Entity<Verification>().ToTable("Verification");

        // Verification.AdminID is a FK to Account, but Account also has
        // a Projects collection - so we need to tell EF which FK goes where
        modelBuilder.Entity<Verification>()
            .HasOne(v => v.Admin)
            .WithMany()
            .HasForeignKey(v => v.AdminID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Verification>()
            .HasOne(v => v.Project)
            .WithMany(p => p.Verifications)
            .HasForeignKey(v => v.ProjectID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Project>()
            .HasOne(p => p.Student)
            .WithMany(a => a.Projects)
            .HasForeignKey(p => p.StudentID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Update>()
            .HasOne(u => u.Project)
            .WithMany(p => p.Updates)
            .HasForeignKey(u => u.ProjectID)
            .OnDelete(DeleteBehavior.Cascade);
    }
}