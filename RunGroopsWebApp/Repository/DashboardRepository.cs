using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Repository;

public class DashboardRepository : IDashboardRepository
{
  private readonly ApplicationDbContext _context;
  private readonly IHttpContextAccessor _httpContextAccessor;

  public DashboardRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
  {
    _context = context;
    _httpContextAccessor = httpContextAccessor;
  }

  public async Task<List<Club>> GetAllUserClubs()
  {
    var curUserId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
    var userClubs = _context.Clubs.Where(r => r.AppUserId == curUserId);
    return userClubs.ToList();
  }

 
  public async Task<List<Race>> GetAllUserRaces()
  {
    var curUserId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier).Value;
    var userRaces = _context.Races.Where(r => r.AppUserId == curUserId);
    return userRaces.ToList();
  }
  
  public async Task<AppUser> GetUserById(string id)
  {
    return await _context.Users.FindAsync(id);
  }

  public async Task<AppUser> GetByIdNoTracking(string id)
  {
    return await _context.Users.Where(u => u.Id == id).AsNoTracking().FirstOrDefaultAsync();
  }

  public bool Update(AppUser user)
  {
    _context.Update(user);
    return Save();
  }

  public bool Save()
  {
    var saved = _context.SaveChanges();
    return saved > 0 ? true : false;
  }
}