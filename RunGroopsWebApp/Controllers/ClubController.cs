using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp.Data;
using RunGroopWebApp.Data.Enum;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.Repository;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers;

public class ClubController : Controller
{
    private readonly IClubRepository _clubRepository;
    private readonly IPhotoService _photoService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ClubController(IClubRepository clubRepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
    {
        _clubRepository = clubRepository;
        _photoService = photoService;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<IActionResult> Index()
    {
        IEnumerable<Club> clubs = await _clubRepository.GetAll();
        return View(clubs);
    }

    public async Task<IActionResult> Detail(int id)
    {
        Club club = await _clubRepository.GetByIdAsync(id);
        return View(club);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var curUser = _httpContextAccessor.HttpContext.User.GetUserId();
        var createClubViewModel = new CreateClubViewModel
        {
            AppUserId = curUser,
        };
        return View(createClubViewModel);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CreateClubViewModel clubVm)
    {
        if (ModelState.IsValid)
        {
            var result = await _photoService.AddPhotoAsync(clubVm.Image);
            var club = new Club()
            {
                Title = clubVm.Title,
                Description = clubVm.Description,
                Image = result.Url.ToString(),
                AppUserId = clubVm.AppUserId,
                Address = new Address
                {
                    Street = clubVm.Address.Street,
                    City = clubVm.Address.City,
                    State = clubVm.Address.State
                }
            };
            _clubRepository.Add(club);
            return RedirectToAction("Index");
        }
        else
        {
            ModelState.AddModelError("","Photo upload failed");
        }
        return View(clubVm);
    } 

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var club = await _clubRepository.GetByIdAsync(id);
        if (club == null) return View("Error");
        var clubVM = new EditClubViewModel
        {
            Title = club.Title,
            Description = club.Description,
            AddressId = club.AddressId,
            Address = club.Address,
            URL = club.Image,
            ClubCategory = club.ClubCategory

        };
        return View(clubVM);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, EditClubViewModel clubVM)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Failed to edit club");
            return View("Edit", clubVM);
        }
        var userClub = await _clubRepository.GetByIdAsyncNoTracking(id);
        if (userClub != null)
        {
            try
            {
                await _photoService.DeletePhotoAsync(userClub.Image);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("","Cloudinary could not delete photo");
                return View(clubVM);
            }
            
            var result = await _photoService.AddPhotoAsync(clubVM.Image);
            var club = new Club
            {
                Id = id,
                Title = clubVM.Title,
                Description = clubVM.Description,
                Image = result.Url.ToString(),
                AddressId = clubVM.AddressId,
                Address = new Address
                {
                    Street = clubVM.Address.Street,
                    City = clubVM.Address.City,
                    State = clubVM.Address.State,
                }
            };
            
            _clubRepository.Update(club);
            return RedirectToAction("Index");
        }
        return View(clubVM);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var clubDetail = await _clubRepository.GetByIdAsync(id);
        if (clubDetail == null) return View("Error");
        return View(clubDetail);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteClub(int id)
    {
        var club = await _clubRepository.GetByIdAsync(id);
        if (club == null) return View("Error");
        _clubRepository.Delete(club);
        return RedirectToAction("Index");
    }
}