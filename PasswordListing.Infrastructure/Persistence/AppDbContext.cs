using System;
using Microsoft.EntityFrameworkCore;
using PasswordListing.Domain.Entities;

namespace PasswordListing.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Item> Items { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().HasKey(el=> el.Id);
        modelBuilder.Entity<Item>().HasKey(el=> el.Id);
    }
}
