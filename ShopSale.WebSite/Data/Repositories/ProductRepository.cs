namespace ShopSale.WebSite.Data.Repositories
{
	using System.Linq;
	using Microsoft.EntityFrameworkCore;
	using Entities;
	using Interfaces;
    
    public class ProductRepository : GenericRepository<Product>, IProductRepository
	{
		private readonly DataContext _context;

		public ProductRepository(DataContext context) : base(context)
		{
			this._context = context;
		}

		public IQueryable GetAllWithUsers()
		{
			return this._context.Products.Include(p => p.User); //.OrderBy(p => p.Name);
		}
	}
}