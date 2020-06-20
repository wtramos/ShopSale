namespace ShopSale.WebSite.Data
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
	using ShopSale.WebSite.Data.Entities;

    public class DataContext : IdentityDbContext<User>
	{
		public DbSet<Product> Products { get; set; }
        public DbSet<Country> Countries { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
		{
		}
	}
}
