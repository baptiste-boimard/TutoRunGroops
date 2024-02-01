using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Repository;

public class RaceRepository : IRaceRepository
{
    private readonly ApplicationDbContext _context;

    public RaceRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Race>> GetAll()
    {
        return await _context.Races.ToListAsync();
    }

    public async Task<Race> GetbyIdAsync(int id)
    {
        return await _context.Races.Include(a => a.Address).FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<IEnumerable<Race>> GetRaceByCity(string race)
    {
        return await _context.Races.Where(c => c.Address.City.Contains(race)).ToListAsync();
    }

    public bool Add(Race race)
    {
        _context.Add(race);
        return Save();
    }

    public bool Update(Race race)
    {
        _context.Update(race);
        return Save();
    }

    public bool Delete(Race race)
    {
        _context.Remove(race);
        return Save();
    }

    public bool Save()
    {
        var saved = _context.SaveChanges();
        return saved > 0 ? true : false;
    }
}