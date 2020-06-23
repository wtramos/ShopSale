namespace ShopSale.WebSite.Controllers.API
{
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Authentication.JwtBearer;
	using Microsoft.AspNetCore.Mvc;
    using Data.Interfaces;
    

    [Route("api/[Controller]")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class ProductsController : Controller
	{
		private readonly IProductRepository _productRepository;

		public ProductsController(IProductRepository productRepository)
		{
			this._productRepository = productRepository;
		}

		[HttpGet]
		public IActionResult GetProducts()
		{
			return this.Ok(this._productRepository.GetAllWithUsers());
		}
	}
}