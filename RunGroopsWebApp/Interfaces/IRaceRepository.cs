using RunGroopWebApp.Models;

namespace RunGroopWebApp.Interfaces;

public interface IRaceRepository
{
    Task<IEnumerable<Race>> GetAll();
    Task<Race> GetbyIdAsync(int id);
    Task<Race> GetbyIdAsyncNoTracking(int id);
    Task<IEnumerable<Race>> GetRaceByCity(string race);
    bool Add(Race race);
    bool Update(Race race);
    bool Delete(Race race);
    bool Save();


}