namespace ShopSale.WebSite.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Identity;
    using Entities;
    using Helpers;
    
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private Random _random;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            this._context = context;
            this._userHelper = userHelper;
            this._random = new Random();
        }
        public async Task SeedAsync()
        {
            await this._context.Database.EnsureCreatedAsync();

            await this._userHelper.CheckRoleAsync("Admin");
            await this._userHelper.CheckRoleAsync("Customer");

            if (!this._context.Countries.Any())
            {
                var cities = new List<City>();
                cities.Add(new City { Name = "Lima" });
                cities.Add(new City { Name = "Ica" });
                cities.Add(new City { Name = "Huancayo" });
                cities.Add(new City { Name = "Piura" });

                this._context.Countries.Add(new Country
                {
                    Cities = cities,
                    Name = "Perú"
                });

                await this._context.SaveChangesAsync();
            }

            var user = await this._userHelper.GetUserByEmailAsync("walter.torres.ramos@gmail.com");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Walter",
                    LastName = "Torres",
                    Email = "walter.torres.ramos@gmail.com",
                    UserName = "walter.torres.ramos@gmail.com",
                    PhoneNumber = "938252115",
                    Address = "Calle Santa Carmela 118 - Surco",
                    CityId = this._context.Countries.FirstOrDefault().Cities.FirstOrDefault().Id,
                    City = this._context.Countries.FirstOrDefault().Cities.FirstOrDefault()

                };

                var result = await this._userHelper.AddUserAsync(user, "123456");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }
                await this._userHelper.AddUserToRoleAsync(user, "Admin");
                var token = await this._userHelper.GenerateEmailConfirmationTokenAsync(user);
                await this._userHelper.ConfirmEmailAsync(user, token);
            }

            var isInRole = await this._userHelper.IsUserInRoleAsync(user, "Admin");
            if (!isInRole)
            {
                await this._userHelper.AddUserToRoleAsync(user, "Admin");
            }


            if (!this._context.Products.Any())
            {
                this.AddProduct("First Product", user);
                this.AddProduct("Second Product", user);
                this.AddProduct("Third Product", user);
                await this._context.SaveChangesAsync();
            }
        }

        private void AddProduct(string name, User user)
        {
            this._context.Products.Add(new Product
            {
                Name = name,
                Price = this._random.Next(100),
                IsAvailabe = true,
                Stock = this._random.Next(100),
                User = user
            });
        }
    }
}
