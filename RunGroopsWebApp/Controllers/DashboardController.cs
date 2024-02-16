using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers;

public class DashboardController : Controller
{
  private readonly IDashboardRepository _dashboardRepository;

  public DashboardController(IDashboardRepository dashboardRepository)
  {
    _dashboardRepository = dashboardRepository;
  }
  public async Task<IActionResult> Index()
  {
    var userRaces = _dashboardRepository.GetAllUserRaces();
    var userClubs = _dashboardRepository.GetAllUserClubs();
    var dashboardViewModel = new DashboardViewModel()
    {
      Races = await userRaces,
      Clubs = await userClubs,
    };
    return View(dashboardViewModel);
  }
}