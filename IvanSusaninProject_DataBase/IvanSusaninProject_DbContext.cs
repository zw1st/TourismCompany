using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IvanSusaninProject_Contracts.Infrastructure;
using System.ComponentModel;
using IvanSusaninProject_Database.Models;
using IvanSusaninProject_DataBase.Models;


namespace IvanSusaninProject_Database;

public class IvanSusaninProject_DbContext : DbContext
{
    private readonly IConfigurationDatabase? _configurationDatabase;

    public IvanSusaninProject_DbContext(IConfigurationDatabase configurationDatabase)
    {
        _configurationDatabase =
    configurationDatabase;
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder
    optionsBuilder)
    {
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=IvanDb;Username=postgres;Password=postgres;", o
        => o.SetPostgresVersion(12, 2));
        base.OnConfiguring(optionsBuilder);

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Excursion>().HasIndex(x => x.Name).IsUnique();
        modelBuilder.Entity<Excursion>()
        .HasOne(p => p.Guide)
        .WithMany()
        .HasForeignKey(p => p.GuideId)
        .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<User>().HasIndex(x => new { x.Login, x.PasswordHash }).IsUnique();
        modelBuilder.Entity<Tour>().HasIndex(x => x.Name).IsUnique();
        modelBuilder.Entity<TourExcursion>().HasKey(x => new { x.TourId, x.ExcursionId });
        modelBuilder.Entity<TourGroup>().HasKey(x => new { x.TourId, x.GroupId });
        modelBuilder.Entity<Guide>().HasIndex(e => new { e.Fio }).IsUnique();
        modelBuilder.Entity<Place>().HasIndex(x => new { x.Name }).IsUnique();
        modelBuilder.Entity<Place>()
        .HasOne(p => p.Group)
        .WithMany()
        .HasForeignKey(p => p.GroupId)
        .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<TripGuide>().HasKey(x => new { x.TripId, x.GuideId });
        modelBuilder.Entity<TripPlace>().HasKey(x => new { x.TripId, x.PlaceId });
    }

    public DbSet<Excursion> Excursions { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Tour> Tours { get; set; }
    public DbSet<TourExcursion> TourExcursions { get; set; }
    public DbSet<TourGroup> TourGroups { get; set; }
    public DbSet<Guide> Guides { get; set; }
    public DbSet<Trip> Trips { get; set; }
    public DbSet<Place> Places { get; set; }
    public DbSet<TripGuide> TripGuides { get; set; }
    public DbSet<TripPlace> TripPlaces { get; set; }
}
