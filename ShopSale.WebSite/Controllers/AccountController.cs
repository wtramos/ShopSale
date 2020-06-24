namespace ShopSale.WebSite.Controllers
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using Helpers;
    using Models;
    using Data.Entities;
    using Data.Interfaces;

    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IConfiguration _configuration;
        private readonly ICountryRepository _countryRepository;

        public AccountController(IUserHelper userHelper,
                                 IConfiguration configuration,
                                 ICountryRepository countryRepository)
        {
            this._userHelper = userHelper;
            this._configuration = configuration;
            this._countryRepository = countryRepository;
        }

        public IActionResult Login()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Index", "Home");
            }

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var result = await this._userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (this.Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return this.Redirect(this.Request.Query["ReturnUrl"].First());
                    }

                    return this.RedirectToAction("Index", "Home");
                }
            }

            this.ModelState.AddModelError(string.Empty, "Failed to login.");
            return this.View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await this._userHelper.LogoutAsync();
            return this.RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            var model = new RegisterNewUserViewModel
            {
                Countries = this._countryRepository.GetComboCountries(),
                Cities = this._countryRepository.GetComboCities(0)
            };

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await this._userHelper.GetUserByEmailAsync(model.Username);
                if (user == null)
                {
                    var city = await this._countryRepository.GetCityAsync(model.CityId);

                    user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Username,
                        UserName = model.Username,
                        Address = model.Address,
                        PhoneNumber = model.PhoneNumber,
                        CityId = model.CityId,
                        City = city
                    };

                    var result = await this._userHelper.AddUserAsync(user, model.Password);
                    if (result != IdentityResult.Success)
                    {
                        this.ModelState.AddModelError(string.Empty, "The user couldn't be created.");
                        return this.View(model);
                    }

                    var loginViewModel = new LoginViewModel
                    {
                        Password = model.Password,
                        RememberMe = false,
                        Username = model.Username
                    };

                    var result2 = await this._userHelper.LoginAsync(loginViewModel);
                    if (result2.Succeeded)
                    {
                        return this.RedirectToAction("Index", "Home");
                    }

                    this.ModelState.AddModelError(string.Empty, "The user couldn't be login.");
                    return this.View(model);
                }

                this.ModelState.AddModelError(string.Empty, "The username is already registered.");
            }

            return this.View(model);
        }

        public async Task<IActionResult> ChangeUser()
        {
            var user = await this._userHelper.GetUserByEmailAsync(this.User.Identity.Name);
            var model = new ChangeUserViewModel();

            if (user != null)
            {
                model.FirstName = user.FirstName;
                model.LastName = user.LastName;
                model.Address = user.Address;
                model.PhoneNumber = user.PhoneNumber;

                var city = await this._countryRepository.GetCityAsync(user.CityId);
                if (city != null)
                {
                    var country = await this._countryRepository.GetCountryAsync(city);
                    if (country != null)
                    {
                        model.CountryId = country.Id;
                        model.Cities = this._countryRepository.GetComboCities(country.Id);
                        model.Countries = this._countryRepository.GetComboCountries();
                        model.CityId = user.CityId;
                    }
                }
            }

            model.Cities = this._countryRepository.GetComboCities(model.CountryId);
            model.Countries = this._countryRepository.GetComboCountries();
            return this.View(model);

        }

        [HttpPost]
        public async Task<IActionResult> ChangeUser(ChangeUserViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await this._userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if (user != null)
                {
                    var city = await this._countryRepository.GetCityAsync(model.CityId);

                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.Address = model.Address;
                    user.PhoneNumber = model.PhoneNumber;
                    user.CityId = model.CityId;
                    user.City = city;

                    var respose = await this._userHelper.UpdateUserAsync(user);
                    if (respose.Succeeded)
                    {
                        this.ViewBag.UserMessage = "User updated!";
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, respose.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "User no found.");
                }
            }
            return this.View(model);
        }

        public IActionResult ChangePassword()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await this._userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                if (user != null)
                {
                    var result = await this._userHelper.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return this.RedirectToAction("ChangeUser");
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, result.Errors.FirstOrDefault().Description);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "User no found.");
                }
            }

            return this.View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await this._userHelper.GetUserByEmailAsync(model.Username);
                if (user != null)
                {
                    var result = await this._userHelper.ValidatePasswordAsync(
                        user,
                        model.Password);

                    if (result.Succeeded)
                    {
                        var claims = new[]
                        {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._configuration["Tokens:Key"]));
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            this._configuration["Tokens:Issuer"],
                            this._configuration["Tokens:Audience"],
                            claims,
                            expires: DateTime.UtcNow.AddDays(15),
                            signingCredentials: credentials);
                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        return this.Created(string.Empty, results);
                    }
                }
            }

            return this.BadRequest();
        }

        public IActionResult NotAuthorized()
        {
            return this.View();
        }

        public async Task<JsonResult> GetCitiesAsync(int countryId)
        {
            var country = await this._countryRepository.GetCountryWithCitiesAsync(countryId);
            return this.Json(country.Cities.OrderBy(c => c.Name));
        }

    }
}
