﻿namespace ShopSale.WebSite.Helpers
{
	using System.Threading.Tasks;
	using Data.Entities;
	using Microsoft.AspNetCore.Identity;

	public class UserHelper : IUserHelper
	{
		private readonly UserManager<User> _userManager;

		public UserHelper(UserManager<User> userManager)
		{
			this._userManager = userManager;
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
	}

}
