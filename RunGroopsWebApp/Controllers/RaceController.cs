using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers;

public class RaceController : Controller
{
    private readonly IRaceRepository _raceRepository;
    private readonly IPhotoService _photoService;

    public RaceController(IRaceRepository raceRepository, IPhotoService photoService)
    {
        _raceRepository = raceRepository;
        _photoService = photoService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        IEnumerable<Race> races = await _raceRepository.GetAll();
        return View(races);
    }

    public async Task<IActionResult> Detail(int id)
    {
        Race race = await _raceRepository.GetbyIdAsync(id);
        return View(race);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateRaceViewModel raceVm)
    {
        if (ModelState.IsValid)
        {
            var result = await _photoService.AddPhotoAsync(raceVm.Image);
            var race = new Race()
            {
                Title = raceVm.Title,
                Description = raceVm.Description,
                Image = result.Url.ToString(),
                Address = new Address()
                {
                    Street = raceVm.Address.Street,
                    City = raceVm.Address.City,
                    State = raceVm.Address.State
                }
            };
            _raceRepository.Add(race);
            return RedirectToAction("Index");
        }
        else
        {
            ModelState.AddModelError("","Photo upload failed");
        }
        return View(raceVm);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var race = await _raceRepository.GetbyIdAsync(id);
        if (race == null) return View("Error");
        var raceVM = new EditRaceViewModel
        {
            Title = race.Title,
            Description = race.Description,
            Address = race.Address,
            URL = race.Image,
            RaceCategory = race.RaceCategory
        };
        return View(raceVM);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, EditRaceViewModel raceVM)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Failed to edit race");
            return View("Edit", raceVM);
        }

        var userRace = await _raceRepository.GetbyIdAsyncNoTracking(id);
        if (userRace != null)
        {
            try
            {
                await _photoService.DeletePhotoAsync(userRace.Image);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Cloudinary could not delete image");
                return View(raceVM);
            }

            var result = await _photoService.AddPhotoAsync(raceVM.Image);
            var race = new Race()
            {
                Id = id,
                Title = raceVM.Title,
                Description = raceVM.Description,
                Image = result.Url.ToString(),
                AddressId = raceVM.AddressId,
                Address = new Address()
                {
                    Street = raceVM.Address.Street,
                    City = raceVM.Address.City,
                    State = raceVM.Address.State,
                }
            };

            _raceRepository.Update(race);
            return RedirectToAction("Index");
        }
        return View(raceVM);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var raceDetail = await _raceRepository.GetbyIdAsync(id);
        if (raceDetail == null) return View("Error");
        return View(raceDetail);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteRace(int id)
    {
        var race = await _raceRepository.GetbyIdAsync(id);
        if (race == null) return View("Error");
        _raceRepository.Delete(race);
        return RedirectToAction("Index");
    }
}