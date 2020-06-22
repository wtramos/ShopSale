namespace ShopSale.WebSite.Data.Interfaces
{
    using System.Linq;
    using Entities;

    public interface IProductRepository : IGenericRepository<Product>
    {
        IQueryable GetAllWithUsers();
    }
}
