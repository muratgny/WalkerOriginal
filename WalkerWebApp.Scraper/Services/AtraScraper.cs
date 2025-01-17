using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using WalkerWebApp.Data;
using WalkerWebApp.Data.Enum;
using WalkerWebApp.Interfaces;
using WalkerWebApp.Models;
using WalkerWebApp.Repository;
using WalkerWebApp.Scraper.Data;
using WalkerWebApp.Scraper.Extensions;
using WalkerWebApp.Scraper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WalkerWebApp.Scraper.Services
{
    public class AtraScraper : IAtraScraper
    {
        private IWalkRepository _raceRepo;
        private IWebDriver _driver;
        public AtraScraper()
        {
            _driver = new ChromeDriver();
        }

        public void Run()
        {
            IterateOverRaceElements();
        }

        public IReadOnlyCollection<IWebElement> GetElements()
        {
            return _driver.FindElements(By.CssSelector("span[itemprop='name']"));
        }

        public void IterateOverRaceElements()
        {
            _driver.Navigate().GoToUrl("https://trailrunner.com/race-calendar/");
            var pageNumbers = _driver.FindElements(By.CssSelector("a[class='page-numbers']"));
            for (int i = 0; i < pageNumbers.Count; i++)
            {
                    if(i >= 1)
                    {
                        pageNumbers = _driver.FindElements(By.CssSelector("a[class='page-numbers']"));
                        pageNumbers.ElementAt(i).Click();
                    }
                    var pageElements = GetElements();
                    for (int j = 0; j < pageElements.Count; j++)
                    {
                        try
                        {
                            pageElements = _driver.FindElements(By.CssSelector("span[itemprop='name']"));
                            var element = pageElements.ElementAt(j);
                            element.Click();
                            var race = new Walk()
                            {
                                Title = _driver.FindElement(By.CssSelector("h1[class='event-title']")).Text ?? "",
                                Description = _driver.FindElement(By.CssSelector("div[class='content-desc']")).Text.Replace("Description:", "") ?? "",
                                StartTime = _driver.FindElement(By.CssSelector("meta[itemprop='startDate']")).GetAttribute("content").ToDate() ?? DateTime.MinValue,
                                Contact = _driver.FindElement(By.LinkText("Contact Race Director")).GetAttribute("href") ?? "",
                                Address = new Address()
                                {
                                    City = _driver.FindElement(By.CssSelector("span[itemprop='addressLocality']")).Text ?? "",
                                    Country = _driver.FindElement(By.CssSelector("span[itemprop='addressRegion']")).Text ?? "",
                                    Street = _driver.FindElement(By.CssSelector("div[itemprop='address']")).Text ?? "",
                                },
                                WalkCategory = WalkCategory.OneHour
                            };
                            using (var context = new ScraperDBContext())
                            {
                                if (!context.Races.Any(r => r.Title == race.Title))
                                {
                                    context.Races.Add(race);
                                    context.SaveChanges();
                                }
                            }
                            _driver.Navigate().Back();
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine(ex.Message);
                            _driver.Navigate().Back();
                        }
                }
            }
        }
    }
}
