// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WalkerWebApp.Data;
using WalkerWebApp.Interfaces;
using WalkerWebApp.Repository;
using WalkerWebApp.Scraper.Interfaces;
using WalkerWebApp.Scraper.Services;



MeetupScraper scraper = new MeetupScraper();
scraper.Run();

//AtraScraper scraper = new AtraScraper();


//scraper.Run();