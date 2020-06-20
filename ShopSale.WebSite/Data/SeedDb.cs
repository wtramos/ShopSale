namespace ShopSale.WebSite.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
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

            var user = await this._userHelper.GetUserByEmailAsync("walter.torres.ramos@gmail.com");
            if (user == null)
            {
                user = new User
                {
                    FirstName = "Walter",
                    LastName = "Torres",
                    Email = "walter.torres.ramos@gmail.com",
                    UserName = "walter.torres",
                    PhoneNumber = "938252115"
                };

                var result = await this._userHelper.AddUserAsync(user, "123456");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create the user in seeder");
                }
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
