﻿using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using WalkerWebApp.Data;
using WalkerWebApp.Data.Enum;
using WalkerWebApp.Models;
using WalkerWebApp.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace WalkerWebApp.Tests.Repository
{
    public class ClubRepositoryTests
    {
        private async Task<ApplicationDbContext> GetDbContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            var databaseContext = new ApplicationDbContext(options);
            databaseContext.Database.EnsureCreated();
            if(await databaseContext.Clubs.CountAsync() < 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    databaseContext.Clubs.Add(
                      new Club()
                      {
                          Title = "Walking Club 1",
                          Image = "https://images.pexels.com/photos/631986/pexels-photo-631986.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2",
                          Description = "This is the description of the first",
                          ClubCategory = ClubCategory.City,
                          Address = new Address()
                          {
                              Street = "123 Main St",
                              City = "Brooklyn",
                              Country = "USA"
                          }
                      });
                    await databaseContext.SaveChangesAsync();
                }
            }
            return databaseContext;
        }

        [Fact]
        public async void ClubRepository_Add_ReturnsBool()
        {
            //Arrange
            var club = new Club()
            {
                Title = "Walking Club 1",
                Image = "https://images.pexels.com/photos/631986/pexels-photo-631986.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2",
                Description = "This is the description of the first",
                ClubCategory = ClubCategory.City,
                Address = new Address()
                {
                    Street = "123 Main St",
                    City = "Brooklyn",
                    Country = "USA"
                }
            };
            var dbContext = await GetDbContext();
            var clubRepository = new ClubRepository(dbContext);

            //Act
            var result = clubRepository.Add(club);

            //Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async void ClubRepository_GetByIdAsync_ReturnsClub()
        {
            //Arrange
            var id = 1;
            var dbContext = await GetDbContext();
            var clubRepository = new ClubRepository(dbContext);

            //Act
            var result = clubRepository.GetByIdAsync(id);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<Task<Club>>();
        }

        [Fact]
        public async void ClubRepository_GetAll_ReturnsList()
        {
            //Arrange
            var dbContext = await GetDbContext();
            var clubRepository = new ClubRepository(dbContext);

            //Act
            var result = await clubRepository.GetAll();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<Club>>();
        }

        [Fact]
        public async void ClubRepository_SuccessfulDelete_ReturnsTrue()
        {
            //Arrange
            var club = new Club()
            {
                Title = "Walking Club 1",
                Image = "https://images.pexels.com/photos/631986/pexels-photo-631986.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2",
                Description = "This is the description of the first",
                ClubCategory = ClubCategory.City,
                Address = new Address()
                {
                    Street = "123 Main St",
                    City = "Brooklyn",
                    Country = "USA"
                }
            };
            var dbContext = await GetDbContext();
            var clubRepository = new ClubRepository(dbContext);

            //Act
            clubRepository.Add(club);
            var result = clubRepository.Delete(club);
            var count = await clubRepository.GetCountAsync();

            //Assert
            result.Should().BeTrue();
            count.Should().Be(0);
        }

        [Fact]
        public async void ClubRepository_GetCountAsync_ReturnsInt()
        {
            //Arrange
            var club = new Club()
            {
                Title = "Walking Club 1",
                Image = "https://images.pexels.com/photos/631986/pexels-photo-631986.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2",
                Description = "This is the description of the first",
                ClubCategory = ClubCategory.City,
                Address = new Address()
                {
                    Street = "123 Main St",
                    City = "Brooklyn",
                    Country = "USA"
                }
            };
            var dbContext = await GetDbContext();
            var clubRepository = new ClubRepository(dbContext);

            //Act
            clubRepository.Add(club);
            var result = await clubRepository.GetCountAsync();

            //Assert
            result.Should().Be(1);
        }

        [Fact]
        public async void ClubRepository_GetAllStates_ReturnsList()
        {
            //Arrange
            var dbContext = await GetDbContext();
            var clubRepository = new ClubRepository(dbContext);

            //Act
            var result = await clubRepository.GetAllCountries();

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<Country>>();
        }

        [Fact]
        public async void ClubRepository_GetClubsByState_ReturnsList()
        {
            //Arrange
            var state = "USA";
            var club = new Club()
            {
                Title = "Walking Club 1",
                Image = "https://images.pexels.com/photos/631986/pexels-photo-631986.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2",
                Description = "This is the description of the first",
                ClubCategory = ClubCategory.City,
                Address = new Address()
                {
                    Street = "123 Main St",
                    City = "Brooklyn",
                    Country = "USA"
                }
            };
            var dbContext = await GetDbContext();
            var clubRepository = new ClubRepository(dbContext);

            //Act
            clubRepository.Add(club);
            var result = await clubRepository.GetClubsByCountry(state);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<List<Club>>();
            result.First().Title.Should().Be("Walking Club 1");
        }
    }
}
