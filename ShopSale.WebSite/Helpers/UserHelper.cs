namespace ShopSale.WebSite.Helpers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
	using System.Linq;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.EntityFrameworkCore;
	using Data.Entities;
    using Models;
    

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

		public async Task CheckRoleAsync(string roleName)
		{
			var roleExists = await this._roleManager.RoleExistsAsync(roleName);
			if (!roleExists)
			{
				await this._roleManager.CreateAsync(new IdentityRole
				{
					Name = roleName
				});
			}
		}

		public async Task AddUserToRoleAsync(User user, string roleName)
		{
			await this._userManager.AddToRoleAsync(user, roleName);
		}

		public async Task<bool> IsUserInRoleAsync(User user, string roleName)
		{
			return await this._userManager.IsInRoleAsync(user, roleName);
		}

		public async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
		{
			return await this._userManager.ConfirmEmailAsync(user, token);
		}

		public async Task<string> GenerateEmailConfirmationTokenAsync(User user)
		{
			return await this._userManager.GenerateEmailConfirmationTokenAsync(user);
		}

		public async Task<User> GetUserByIdAsync(string userId)
		{
			return await this._userManager.FindByIdAsync(userId);
		}

		public async Task<string> GeneratePasswordResetTokenAsync(User user)
		{
			return await this._userManager.GeneratePasswordResetTokenAsync(user);
		}

		public async Task<IdentityResult> ResetPasswordAsync(User user, string token, string password)
		{
			return await this._userManager.ResetPasswordAsync(user, token, password);
		}

		public async Task<List<User>> GetAllUsersAsync()
		{
			return await this._userManager.Users
				.Include(u => u.City)
				.OrderBy(u => u.FirstName)
				.ThenBy(u => u.LastName)
				.ToListAsync();
		}

		public async Task RemoveUserFromRoleAsync(User user, string roleName)
		{
			await this._userManager.RemoveFromRoleAsync(user, roleName);
		}

		public async Task DeleteUserAsync(User user)
		{
			await this._userManager.DeleteAsync(user);
		}

	}
}
