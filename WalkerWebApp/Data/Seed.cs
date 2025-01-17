using Microsoft.AspNetCore.Identity;
using WalkerWebApp.Data.Enum;
using WalkerWebApp.Models;

namespace WalkerWebApp.Data
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                context.Database.EnsureCreated();

                if (!context.Clubs.Any())
                {
                    context.Clubs.AddRange(new List<Club>()
                    {
                        new Club()
                        {
                            Title = "Running Club 1",
                            Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
                            Description = "This is the description of the first cinema",
                            ClubCategory = ClubCategory.City,
                            Address = new Address()
                            {
                                Street = "Peppareds Torg 17",
                                City = "Mölndal",
                                Country = "Sweden"
                            }
                         },
                        new Club()
                        {
                            Title = "Running Club 2",
                            Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
                            Description = "This is the description of the first cinema",
                            ClubCategory = ClubCategory.Endurance,
                            Address = new Address()
                            {
                                Street = "Peppareds Torg 17",
                                City = "Mölndal",
                                Country = "Sweden"
                            }
                        },
                        new Club()
                        {
                            Title = "Running Club 3",
                            Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
                            Description = "This is the description of the first club",
                            ClubCategory = ClubCategory.Trail,
                            Address = new Address()
                            {
                                Street = "Peppareds Torg 17",
                                City = "Mölndal",
                                Country = "Sweden"
                            }
                        },
                        new Club()
                        {
                            Title = "Running Club 4",
                            Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
                            Description = "This is the description of the first club",
                            ClubCategory = ClubCategory.City,
                            Address = new Address()
                            {
                                Street = "Peppareds Torg 17",
                                City = "Mölndal",
                                Country = "Sweden"
                            }
                        }
                    });
                    context.SaveChanges();
                }
                //Races
                if (!context.Walks.Any())
                {
                    context.Walks.AddRange(new List<Walk>()
                    {
                        new Walk()
                        {
                            Title = "Running Race 1",
                            Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
                            Description = "This is the description of the first race",
                            WalkCategory = WalkCategory.OneHour,
                            Address = new Address()
                            {
                                Street = "Peppareds Torg 17",
                                City = "Mölndal",
                                Country = "Sweden"
                            }
                        },
                        new Walk()
                        {
                            Title = "Running Race 2",
                            Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
                            Description = "This is the description of the first race",
                            WalkCategory = WalkCategory.Unspecified,
                            AddressId = 5,
                            Address = new Address()
                            {
                                Street = "Peppareds Torg 17",
                                City = "Mölndal",
                                Country = "Sweden"
                            }
                        },
                        new Walk()
                        {
                            Title = "Running Race 3",
                            Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
                            Description = "This is the description of the first race",
                            WalkCategory = WalkCategory.Short,
                            AddressId = 5,
                            Address = new Address()
                            {
                                Street = "Peppareds Torg 17",
                                City = "Mölndal",
                                Country = "Sweden"
                            }
                        },
                        new Walk()
                        {
                            Title = "Running Race 4",
                            Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
                            Description = "This is the description of the first race",
                            WalkCategory = WalkCategory.TwoHour,
                            AddressId = 5,
                            Address = new Address()
                            {
                                Street = "Peppareds Torg 17",
                                City = "Mölndal",
                                Country = "Sweden"
                            }
                        }
                    });
                    context.SaveChanges();
                }
            }
        }

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                string adminUserEmail = "muratgny06@gmail.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new AppUser()
                    {
                        UserName = "muratgny",
                        Email = adminUserEmail,
                        EmailConfirmed = true,
                        Address = new Address()
                        {
                            Street = "Peppareds Torg 17",
                            City = "Mölndal",
                            Country = "Sweden"
                        }
                    };
                    await userManager.CreateAsync(newAdminUser, "123456");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

                string appUserEmail = "murat@gmail.com";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new AppUser()
                    {
                        UserName = "app-user",
                        Email = appUserEmail,
                        EmailConfirmed = true,
                        Address = new Address()
                        {
                            Street = "Peppareds Torg 17",
                            City = "Mölndal",
                            Country = "Sweden"
                        }
                    };
                    await userManager.CreateAsync(newAppUser, "123456");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }
            }
        }
    }
}