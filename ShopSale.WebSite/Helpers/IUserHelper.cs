﻿namespace ShopSale.WebSite.Helpers
{
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Identity;
	using Data.Entities;
	using Models;

    public interface IUserHelper
	{
		Task<User> GetUserByEmailAsync(string email);

		Task<IdentityResult> AddUserAsync(User user, string password);

		Task<SignInResult> LoginAsync(LoginViewModel model);

		Task LogoutAsync();
	}
}
