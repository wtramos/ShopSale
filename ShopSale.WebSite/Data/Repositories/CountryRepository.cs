namespace ShopSale.WebSite.Data.Repositories
{
    using Entities;
    using Interfaces;

    public class CountryRepository : GenericRepository<Country>, ICountryRepository
	{
		public CountryRepository(DataContext context) : base(context)
		{
		}
	}
}
