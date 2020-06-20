namespace ShopSale.WebSite.Controllers.API
{
    using Microsoft.AspNetCore.Mvc;
    using Data.Interfaces;

    [Route("api/[Controller]")]
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
			return this.Ok(this._productRepository.GetAll());
		}
	}
}