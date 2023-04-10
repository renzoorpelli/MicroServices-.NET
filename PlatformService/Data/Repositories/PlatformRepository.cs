using PlatformService.Models;

namespace PlatformService.Data.Repositories;

public class PlatformRepository : IPlatformRepository
{
    private readonly ApplicationDbContext _context;

    public PlatformRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public bool Commit()
    {
        if (_context.ChangeTracker.HasChanges())
        {
            _context.SaveChanges();
            return true;
        }

        return false;
    }

    public IEnumerable<Platform> GetAll()
    {
        return _context.Platforms.ToList();
    }

    public Platform? GetById(Guid id)
    {
        return _context.Platforms
            .FirstOrDefault(p => p.Id == id);
    }

    public bool Create(Platform? platform)
    {
        if (platform is not null)
        {
            _context.Platforms.Add(platform);
            return true;
        }
        return false;
    }

    public bool Any()
    {
        return _context.Platforms.Any();
    }
}