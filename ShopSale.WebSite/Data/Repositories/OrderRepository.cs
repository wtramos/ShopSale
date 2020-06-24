namespace ShopSale.WebSite.Data.Repositories
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Entities;
    using Helpers;
    using Interfaces;
    using Models;
    

    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public OrderRepository(DataContext context, IUserHelper userHelper) : base(context)
        {
            this._context = context;
            this._userHelper = userHelper;
        }

        public async Task<IQueryable<Order>> GetOrdersAsync(string userName)
        {
            var user = await this._userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return null;
            }

            if (await this._userHelper.IsUserInRoleAsync(user, "Admin"))
            {
                return this._context.Orders
                    .Include(o => o.User)
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                    .OrderByDescending(o => o.OrderDate);
            }

            return this._context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.User == user)
                .OrderByDescending(o => o.OrderDate);
        }

        public async Task<IQueryable<OrderDetailTemp>> GetDetailTempsAsync(string userName)
        {
            var user = await this._userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return null;
            }

            return this._context.OrderDetailTemps
                .Include(o => o.Product)
                .Where(o => o.User == user)
                .OrderBy(o => o.Product.Name);
        }

        public async Task AddItemToOrderAsync(AddItemViewModel model, string userName)
        {
            var user = await this._userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return;
            }

            var product = await this._context.Products.FindAsync(model.ProductId);
            if (product == null)
            {
                return;
            }

            var orderDetailTemp = await this._context.OrderDetailTemps
                .Where(odt => odt.User == user && odt.Product == product)
                .FirstOrDefaultAsync();
            if (orderDetailTemp == null)
            {
                orderDetailTemp = new OrderDetailTemp
                {
                    Price = product.Price,
                    Product = product,
                    Quantity = model.Quantity,
                    User = user,
                };

                this._context.OrderDetailTemps.Add(orderDetailTemp);
            }
            else
            {
                orderDetailTemp.Quantity += model.Quantity;
                this._context.OrderDetailTemps.Update(orderDetailTemp);
            }

            await this._context.SaveChangesAsync();
        }

        public async Task ModifyOrderDetailTempQuantityAsync(int id, double quantity)
        {
            var orderDetailTemp = await this._context.OrderDetailTemps.FindAsync(id);
            if (orderDetailTemp == null)
            {
                return;
            }

            orderDetailTemp.Quantity += quantity;
            if (orderDetailTemp.Quantity > 0)
            {
                this._context.OrderDetailTemps.Update(orderDetailTemp);
                await this._context.SaveChangesAsync();
            }
        }

        public async Task DeleteDetailTempAsync(int id)
        {
            var orderDetailTemp = await this._context.OrderDetailTemps.FindAsync(id);
            if (orderDetailTemp == null)
            {
                return;
            }

            this._context.OrderDetailTemps.Remove(orderDetailTemp);
            await this._context.SaveChangesAsync();
        }

        public async Task<bool> ConfirmOrderAsync(string userName)
        {
            var user = await this._userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return false;
            }

            var orderTmps = await this._context.OrderDetailTemps
                .Include(o => o.Product)
                .Where(o => o.User == user)
                .ToListAsync();

            if (orderTmps == null || orderTmps.Count == 0)
            {
                return false;
            }

            var details = orderTmps.Select(o => new OrderDetail
            {
                Price = o.Price,
                Product = o.Product,
                Quantity = o.Quantity
            }).ToList();

            var order = new Order
            {
                OrderDate = DateTime.UtcNow,
                User = user,
                Items = details,
            };

            this._context.Orders.Add(order);
            this._context.OrderDetailTemps.RemoveRange(orderTmps);
            await this._context.SaveChangesAsync();
            return true;
        }

        public async Task DeliverOrder(DeliverViewModel model)
        {
            var order = await this._context.Orders.FindAsync(model.Id);
            if (order == null)
            {
                return;
            }

            order.DeliveryDate = model.DeliveryDate;
            this._context.Orders.Update(order);
            await this._context.SaveChangesAsync();
        }

        public async Task<Order> GetOrdersAsync(int id)
        {
            return await this._context.Orders.FindAsync(id);
        }

    }
}
