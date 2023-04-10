using CommandService.Data.Repositories.Interfaces;

namespace CommandService.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    public UnitOfWork(ApplicationDbContext context)
    {
        Context = context;
    }
    
    public ApplicationDbContext Context { get; }
    
    public bool Commit()
    {
        if (Context.ChangeTracker.HasChanges())
        {
            Context.SaveChanges();
            return true;
        }

        return false;
    }

    public void Dispose()
    {
        Context.Dispose();
    }
}