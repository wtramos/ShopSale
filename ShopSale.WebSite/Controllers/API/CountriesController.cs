namespace ShopSale.WebSite.Controllers.API
{
	using Data.Interfaces;
	using Microsoft.AspNetCore.Mvc;

	[Route("api/[Controller]")]
	public class CountriesController : Controller
	{
		private readonly ICountryRepository countryRepository;

		public CountriesController(ICountryRepository countryRepository)
		{
			this.countryRepository = countryRepository;
		}

		[HttpGet]
		public IActionResult GetCountries()
		{
			return Ok(this.countryRepository.GetCountriesWithCities());
		}
	}
}
