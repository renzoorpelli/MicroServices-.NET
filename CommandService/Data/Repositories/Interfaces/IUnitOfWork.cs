namespace CommandService.Data.Repositories.Interfaces;

public interface IUnitOfWork
{ 
    /// <summary>
    /// 
    /// </summary>
    ApplicationDbContext Context { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    bool Commit();

    /// <summary>
    /// 
    /// </summary>
    void Dispose();
}