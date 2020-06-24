namespace ShopSale.WebSite.Data.Interfaces
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Entities;
    

    public interface IProductRepository : IGenericRepository<Product>
    {
        IQueryable GetAllWithUsers();

        IEnumerable<SelectListItem> GetComboProducts();
    }
}
