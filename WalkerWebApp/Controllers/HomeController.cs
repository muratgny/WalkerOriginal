using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WalkerWebApp.Data;
using WalkerWebApp.Helpers;
using WalkerWebApp.Interfaces;
using WalkerWebApp.Models;
using WalkerWebApp.ViewModels;
using System.Diagnostics;
using System.Globalization;
using System.Net;

namespace WalkerWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IClubRepository _clubRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILocationService _locationService;
        private readonly IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, IClubRepository clubRepository,
            UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ILocationService locationService, IConfiguration config)
        {
            _logger = logger;
            _clubRepository = clubRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _locationService = locationService;
            _config = config;
        }

        public async Task<IActionResult> Index()
        {
            var ipInfo = new IPInfo();
            var homeViewModel = new HomeViewModel();
            try
            {
                string url = "https://ipinfo.io?token=" + _config.GetValue<string>("IPInfoToken");
                var info = new WebClient().DownloadString(url);
                ipInfo = JsonConvert.DeserializeObject<IPInfo>(info);
                RegionInfo myRI1 = new RegionInfo(ipInfo.Country);
                ipInfo.Country = myRI1.EnglishName;
                homeViewModel.City = ipInfo.City;
                homeViewModel.Country = ipInfo.Region;
                if (homeViewModel.City != null)
                {
                    homeViewModel.Clubs = await _clubRepository.GetClubByCity(homeViewModel.City);
                }
                return View(homeViewModel);
            }
            catch (Exception)
            {
                homeViewModel.Clubs = null;
            }

            return View(homeViewModel);
        }

        public IActionResult Register()
        {
            var model = new HomeViewModel
            {
                Register = new HomeUserCreateViewModel(),
                // Initialize other HomeViewModel properties if needed
            };
            return View("Index", model); // Render the Index view
        }
        [HttpPost]
        public async Task<IActionResult> Register(HomeViewModel homeViewModel)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }

                var model = new HomeViewModel
                {
                    Register = homeViewModel.Register,
                    // Add other properties if needed (e.g., Clubs, City, etc.)
                };
                return View("Index", model); // Render the Index view with the model
            }

            var user = await _userManager.FindByEmailAsync(homeViewModel.Register.EmailAddress);
            if (user != null)
            {
                ModelState.AddModelError("", "This email address is already in use.");
                return View("Index", homeViewModel);
            }

            var newUser = new AppUser()
            {
                Email = homeViewModel.Register.EmailAddress,
                UserName = homeViewModel.Register.EmailAddress
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, homeViewModel.Register.Password);

            if (!newUserResponse.Succeeded)
            {
                foreach (var error in newUserResponse.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("Index", homeViewModel);
            }

            var roleResult = await _userManager.AddToRoleAsync(newUser, UserRoles.User);
            if (!roleResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to assign user role.");
                return View("Index", homeViewModel);
            }


            // Automatically sign in the user after registration
            await _signInManager.SignInAsync(newUser, isPersistent: false);

            // Redirect to Dashboard
            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}