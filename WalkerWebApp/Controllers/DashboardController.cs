﻿using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalkerWebApp.Interfaces;
using WalkerWebApp.Models;
using WalkerWebApp.ViewModels;

namespace WalkerWebApp.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRespository;
        private readonly IPhotoService _photoService;

        public DashboardController(IDashboardRepository dashboardRespository, IPhotoService photoService)
        {
            _dashboardRespository = dashboardRespository;
            _photoService = photoService;
        }

        public async Task<IActionResult> Index()
        {
            var userRaces = await _dashboardRespository.GetAllUserWalks();
            var userClubs = await _dashboardRespository.GetAllUserClubs();
            var dashboardViewModel = new DashboardViewModel()
            {
                Walks = userRaces,
                Clubs = userClubs
            };
            return View(dashboardViewModel);
        }
    }
}