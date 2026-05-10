using Microsoft.EntityFrameworkCore;
using CivicAction.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CivicAction.Data;

public class CivicActionContext : IdentityDbContext<AppUser>
{
    
    public CivicActionContext(DbContextOptions<CivicActionContext> options)
        : base(options) { }

    public DbSet<Project> Projects { get; set; }
    public DbSet<Update> Updates { get; set; }
    public DbSet<Verification> Verifications { get; set; }
    public DbSet<VolunteerOrganization> VolunteerOrganizations { get; set; }
    public DbSet<VolunteerHour> VolunteerHours { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<AppUser>().ToTable("Account");
        modelBuilder.Entity<Project>().ToTable("Project");
        modelBuilder.Entity<Update>().ToTable("Update");
        modelBuilder.Entity<Verification>().ToTable("Verification");
        modelBuilder.Entity<VolunteerOrganization>().ToTable("VolunteerOrganization");
        modelBuilder.Entity<VolunteerHour>().ToTable("VolunteerHour");

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

        modelBuilder.Entity<VolunteerOrganization>()
            .HasOne(v => v.Student)
            .WithMany(a => a.VolunteerOrganizations)
            .HasForeignKey(v => v.StudentID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<VolunteerHour>()
            .HasOne(v => v.VolunteerOrganization)
            .WithMany(o => o.VolunteerHours)
            .HasForeignKey(v => v.VolunteerOrganizationID)
            .OnDelete(DeleteBehavior.Cascade);
    }
}