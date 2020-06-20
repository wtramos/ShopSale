namespace ShopSale.WebSite.Data.Repositories
{
	using Entities;
	using Interfaces;

	public class ProductRepository : GenericRepository<Product>, IProductRepository
	{
		public ProductRepository(DataContext context) : base(context)
		{
		}
	}
}