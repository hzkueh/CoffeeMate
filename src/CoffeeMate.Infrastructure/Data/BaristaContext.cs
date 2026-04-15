using CoffeeMate.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CoffeeMate.Infrastructure.Data;

public class BaristaContext(DbContextOptions options) : IdentityDbContext<AppUser>(options)
{
    public DbSet<Coffee> Coffees { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Step> Steps { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<SessionStep> SessionSteps { get; set; }
    public DbSet<Collaborator> Collaborators { get; set; }
    public DbSet<GuestToken> GuestTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Must be called first — configures all Identity tables
        base.OnModelCreating(builder);

        // --- Coffee ---
        // 1:1 with Recipe (Recipe holds the FK)
        builder.Entity<Coffee>()
            .HasOne(c => c.Recipe)
            .WithOne(r => r.Coffee)
            .HasForeignKey<Recipe>(r => r.CoffeeId)
            .OnDelete(DeleteBehavior.Cascade);

        // --- Session ---
        // CreatedByUserId → AppUser (no navigation on Session side)
        builder.Entity<Session>()
            .HasOne<AppUser>()
            .WithMany()
            .HasForeignKey(s => s.CreatedByUserId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        // --- Collaborator ---
        // UserId → AppUser (1:many — same user can join multiple sessions)
        builder.Entity<Collaborator>()
            .HasOne<AppUser>()
            .WithMany(u => u.Collaborations)
            .HasForeignKey(c => c.UserId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.SetNull);

        // --- GuestToken ---
        // Token must be unique — used for guest lookups
        builder.Entity<GuestToken>()
            .HasIndex(g => g.Token)
            .IsUnique();

        // --- Seed Data ---
        builder.Entity<Coffee>().HasData(SeedData.GetCoffees());
        builder.Entity<Recipe>().HasData(SeedData.GetRecipes());
        builder.Entity<Step>().HasData(SeedData.GetSteps());
    }
}
