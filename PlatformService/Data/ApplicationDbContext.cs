using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using PlatformService.Models;

namespace PlatformService.Data;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    :base(options)
    {
        
    }
    
    public DbSet<Platform> Platforms { get; set; }
}