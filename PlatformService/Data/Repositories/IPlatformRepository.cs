using PlatformService.Models;

namespace PlatformService.Data.Repositories;

public interface IPlatformRepository
{
    /// <summary>
    /// Metodo encargado de realizar el commit a la base de datos
    /// </summary>
    /// <returns></returns>
    bool Commit();

    /// <summary>
    /// metodo encargado de retornar todos los platform
    /// </summary>
    /// <returns></returns>
    IEnumerable<Platform> GetAll();

    /// <summary>
    /// metodo encargado de obtener un platform especifico
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Platform? GetById(Guid id);

    bool Create(Platform platform);

    bool Any();
}