using System.Reflection;
using CommandService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandService.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        :base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    
    public DbSet<Command> Commands { get; set; }      
    public DbSet<Platform> Platforms { get; set; }    
}