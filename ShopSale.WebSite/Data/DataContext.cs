namespace ShopSale.WebSite.Data
{
	using Microsoft.EntityFrameworkCore;
    using ShopSale.WebSite.Data.Entities;

    public class DataContext : DbContext
	{
		public DbSet<Product> Products { get; set; }

		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{
		}
	}

}
