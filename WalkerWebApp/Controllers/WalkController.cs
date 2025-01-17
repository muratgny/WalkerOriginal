using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WalkerWebApp.Data;
using WalkerWebApp.Data.Enum;
using WalkerWebApp.Interfaces;
using WalkerWebApp.Models;
using WalkerWebApp.ViewModels;

namespace WalkerWebApp.Controllers
{
    public class WalkController : Controller
    {
        private readonly IWalkRepository _walkRepository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WalkController(IWalkRepository walkRepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {
            _walkRepository = walkRepository;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
        }


        [HttpGet]
        public async Task<IActionResult> Index(int category = -1, int page = 1, int pageSize = 6)
        {
            if (page < 1 || pageSize < 1)
            {
                return NotFound();
            }

            // if category is -1 (All) dont filter else filter by selected category
            var races = category switch
            {
                -1 => await _walkRepository.GetSliceAsync((page - 1) * pageSize, pageSize),
                _ => await _walkRepository.GetWalksByCategoryAndSliceAsync((WalkCategory)category, (page - 1) * pageSize, pageSize),
            };

            var count = category switch
            {
                -1 => await _walkRepository.GetCountAsync(),
                _ => await _walkRepository.GetCountByCategoryAsync((WalkCategory)category),
            };

            var viewModel = new IndexWalkViewModel
            {
                Walks = races,
                Page = page,
                PageSize = pageSize,
                TotalWalks = count,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize),
                Category = category,
            };

            return View(viewModel);
        }

        [HttpGet]
        [Route("event/{runningWalk}/{id}")]
        public async Task<IActionResult> DetailRace(int id, string runningWalk)
        {
            var walk = await _walkRepository.GetByIdAsync(id);
            return walk == null ? NotFound() : View(walk);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var curUserID = _httpContextAccessor.HttpContext?.User.GetUserId();
            var createRaceViewModel = new CreateWalkViewModel { AppUserId = curUserID };
            return View(createRaceViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateWalkViewModel walkVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(walkVM.Image);

                var walk = new Walk
                {
                    Title = walkVM.Title,
                    Description = walkVM.Description,
                    Image = result.Url.ToString(),
                    AppUserId = walkVM.AppUserId,
                    WalkCategory = walkVM.WalkCategory,
                    Address = new Address
                    {
                        Street = walkVM.Address.Street,
                        City = walkVM.Address.City,
                        Country = walkVM.Address.Country,
                    }
                };
                _walkRepository.Add(walk);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }

            return View(walkVM);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var walk = await _walkRepository.GetByIdAsync(id);
            if (walk == null) return View("Error");
            var walkVM = new EditWalkViewModel
            {
                Title = walk.Title,
                Description = walk.Description,
                AddressId = walk.AddressId,
                Address = walk.Address,
                URL = walk.Image,
                WalkCategory = walk.WalkCategory
            };
            return View(walkVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditWalkViewModel walkVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View(walkVM);
            }

            var userWalk = await _walkRepository.GetByIdAsyncNoTracking(id);

            if (userWalk == null)
            {
                return View("Error");
            }

            var photoResult = await _photoService.AddPhotoAsync(walkVM.Image);

            if (photoResult.Error != null)
            {
                ModelState.AddModelError("Image", "Photo upload failed");
                return View(walkVM);
            }

            if (!string.IsNullOrEmpty(userWalk.Image))
            {
                _ = _photoService.DeletePhotoAsync(userWalk.Image);
            }

            var walk = new Walk
            {
                Id = id,
                Title = walkVM.Title,
                Description = walkVM.Description,
                Image = photoResult.Url.ToString(),
                AddressId = walkVM.AddressId,
                Address = walkVM.Address,
            };

            _walkRepository.Update(walk);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var clubDetails = await _walkRepository.GetByIdAsync(id);
            if (clubDetails == null) return View("Error");
            return View(clubDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var walkDetails = await _walkRepository.GetByIdAsync(id);

            if (walkDetails == null)
            {
                return View("Error");
            }

            if (!string.IsNullOrEmpty(walkDetails.Image))
            {
                _ = _photoService.DeletePhotoAsync(walkDetails.Image);
            }

            _walkRepository.Delete(walkDetails);
            return RedirectToAction("Index");
        }
    }
}