using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers;

public class DashboardController : Controller
{
  private readonly IDashboardRepository _dashboardRepository;
  private readonly IHttpContextAccessor _httpContextAccessor;
  private readonly IPhotoService _photoService;

  public DashboardController(
    IDashboardRepository dashboardRepository,
    IHttpContextAccessor httpContextAccessor,
    IPhotoService photoService)
  {
    _dashboardRepository = dashboardRepository;
    _httpContextAccessor = httpContextAccessor;
    _photoService = photoService;
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

  public async Task<IActionResult> EditUserProfil()
  {
    var curUserId = _httpContextAccessor.HttpContext.User.GetUserId();
    var user = await _dashboardRepository.GetUserById(curUserId);
    if (user == null) return View("Error");
    var editUserViewModel = new EditUserDashboardViewModel()
    {
      Id = curUserId,
      UserName = user.UserName,
      Pace = user.Pace,
      Mileage = user.Mileage,
      ProfileImageUrl = user.ProfileImageUrl,
      City = user.City,
      State = user.State,
    };
    return View(editUserViewModel);
  }
  
  [HttpPost]
  public async Task<IActionResult> EditUserProfile(EditUserDashboardViewModel editVM)
  {
    if (!ModelState.IsValid)
    {
      ModelState.AddModelError("", "Failed to edit profile");
      return View("EditUserProfil", editVM);

    }
    return RedirectToAction("Index");
  }
  
  
}