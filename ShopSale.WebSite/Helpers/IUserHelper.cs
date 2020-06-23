namespace ShopSale.WebSite.Helpers
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

		Task<IdentityResult> UpdateUserAsync(User user);

		Task<IdentityResult> ChangePasswordAsync(User user, string oldPassword, string newPassword);

		Task<SignInResult> ValidatePasswordAsync(User user, string password);

		Task CheckRoleAsync(string roleName);

		Task AddUserToRoleAsync(User user, string roleName);

		Task<bool> IsUserInRoleAsync(User user, string roleName);
	}
}
