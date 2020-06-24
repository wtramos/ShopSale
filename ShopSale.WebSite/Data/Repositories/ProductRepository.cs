namespace ShopSale.WebSite.Data.Repositories
{
	using System.Linq;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.AspNetCore.Mvc.Rendering;
	using System.Collections.Generic;
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
			return this._context.Products.Include(p => p.User);
		}

		public IEnumerable<SelectListItem> GetComboProducts()
		{
			var list = this._context.Products.Select(p => new SelectListItem
			{
				Text = p.Name,
				Value = p.Id.ToString()
			}).ToList();

			list.Insert(0, new SelectListItem
			{
				Text = "[Select a product.]",
				Value = "0"
			});

			return list;
		}
	}
}