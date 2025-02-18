﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WalkerWebApp.Data;
using WalkerWebApp.Interfaces;
using WalkerWebApp.Models;
using WalkerWebApp.ViewModels;

namespace WalkerWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly ILocationService _locationService;

        public AccountController(UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager, 
            ApplicationDbContext context,
            ILocationService locationService)
        {
            _context = context;
            _locationService = locationService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) return View(loginViewModel);

            var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);

            if (user != null)
            {
                //User is found, check password
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
                if (passwordCheck)
                {
                    //Password correct, sign in
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                }
                //Password is incorrect
                ModelState.AddModelError("", "Wrong credentials. Please try again");
                return View(loginViewModel);
            }
            //User not found
            ModelState.AddModelError("", "Wrong credentials. Please try again");
            return View(loginViewModel);
        }

        [HttpGet]
        public IActionResult Register()
        {
            var response = new RegisterViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (registerViewModel == null)
            {
                return BadRequest("Invalid registration request.");
            }

            if (!ModelState.IsValid) return View(registerViewModel);

            var user = await _userManager.FindByEmailAsync(registerViewModel.EmailAddress);
            if (user != null)
            {
                ModelState.AddModelError("", "This email address is already in use.");
                return View(registerViewModel);
            }

            var newUser = new AppUser()
            {
                Email = registerViewModel.EmailAddress,
                UserName = registerViewModel.EmailAddress
            };

            var newUserResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);
            if (!newUserResponse.Succeeded)
            {
                foreach (var error in newUserResponse.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(registerViewModel);
            }

            var roleResult = await _userManager.AddToRoleAsync(newUser, UserRoles.User);
            if (!roleResult.Succeeded)
            {
                ModelState.AddModelError("", "Failed to assign user role.");
                return View(registerViewModel);
            }

            // Automatically sign in the user after registration
            await _signInManager.SignInAsync(newUser, isPersistent: false);

            // Redirect to Dashboard
            return RedirectToAction("Index", "Dashboard");
        }

        

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Race");
        }

        [HttpGet]
        [Route("Account/Welcome")]
        public async Task<IActionResult> Welcome(int page = 0)
        {
            if(page == 0)
            {
                return View();
            }
            return View();
            
        }

        [HttpGet]
        public async Task<IActionResult> GetLocation(string location)
        {
            if(location == null)
            {
                return Json("Not found");
            }
            var locationResult = await _locationService.GetLocationSearch(location);
            return Json(locationResult);
        }


    }
}