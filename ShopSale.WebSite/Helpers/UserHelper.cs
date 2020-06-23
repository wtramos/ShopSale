namespace ShopSale.WebSite.Helpers
{
	using System.Threading.Tasks;
	using Data.Entities;
	using Microsoft.AspNetCore.Identity;
    using ShopSale.WebSite.Models;

    public class UserHelper : IUserHelper
	{
		private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserHelper(UserManager<User> userManager, 
			              SignInManager<User> signInManager,
						  RoleManager<IdentityRole> roleManager)
		{
			this._userManager = userManager;
            this._signInManager = signInManager;
            this._roleManager = roleManager;
        }

		public async Task<IdentityResult> AddUserAsync(User user, string password)
		{
			return await this._userManager.CreateAsync(user, password);
		}

        public async Task<User> GetUserByEmailAsync(string email)
		{
			var user = await this._userManager.FindByEmailAsync(email);
			return user;
		}

        public async Task<SignInResult> LoginAsync(LoginViewModel model)
        {
			return await this._signInManager.PasswordSignInAsync(
					model.Username,
					model.Password,
					model.RememberMe,
					false);
		}

		public async Task LogoutAsync()
        {
			await this._signInManager.SignOutAsync();
		}

		public async Task<IdentityResult> UpdateUserAsync(User user)
		{
			return await this._userManager.UpdateAsync(user);
		}

		public async Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword)
		{
			return await this._userManager.ChangePasswordAsync(user, oldPassword, newPassword);
		}

        public async Task<SignInResult> ValidatePasswordAsync(User user, string password)
        {
			return await this._signInManager.CheckPasswordSignInAsync(
					user,
					password,
					false);

		}


	}
}
