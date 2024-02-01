using Microsoft.AspNetCore.Mvc;
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
    
    public ClubController(ApplicationDbContext context, IClubRepository clubRepository)
    {
        _clubRepository = clubRepository;
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
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(Club club)
    {
        if (ModelState.IsValid)
        {
            View(club);
        }

        _clubRepository.Add(club);
        return RedirectToAction("Index");
    } 

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
            Image = club.Image,
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

        var club = new Club
        {
            Id = id,
            Title = clubVM.Title,
            Description = clubVM.Description,
            Image = clubVM.Image,
            AddressId = clubVM.AddressId,
            Address = clubVM.Address,
        };

        _clubRepository.Update(club);
        return RedirectToAction("Index");
    }
}