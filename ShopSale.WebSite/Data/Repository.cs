namespace ShopSale.WebSite.Data
{
    using Entities;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class Repository : IRepository
    {
        private readonly DataContext _context;

        public Repository(DataContext context)
        {
            this._context = context;
        }

        public IEnumerable<Product> GetProducts()
        {
            return this._context.Products.OrderBy(p => p.Name);
        }

        public Product GetProduct(int id)
        {
            return this._context.Products.Find(id);
        }

        public void AddProduct(Product product)
        {
            this._context.Products.Add(product);
        }

        public void UpdateProduct(Product product)
        {
            this._context.Update(product);
        }

        public void RemoveProduct(Product product)
        {
            this._context.Products.Remove(product);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await this._context.SaveChangesAsync() > 0;
        }

        public bool ProductExists(int id)
        {
            return this._context.Products.Any(p => p.Id == id);
        }
    }
}