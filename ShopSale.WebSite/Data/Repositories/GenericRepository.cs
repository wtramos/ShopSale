namespace ShopSale.WebSite.Data.Repositories
{
	using System.Linq;
	using System.Threading.Tasks;
	using Microsoft.EntityFrameworkCore;
	using Entities;
	using Interfaces;

	public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity
	{
		private readonly DataContext _context;

		public GenericRepository(DataContext context)
		{
			this._context = context;
		}

		public IQueryable<T> GetAll()
		{
			return this._context.Set<T>().AsNoTracking();
		}

		public async Task<T> GetByIdAsync(int id)
		{
			return await this._context.Set<T>()
				.AsNoTracking()
				.FirstOrDefaultAsync(e => e.Id == id);
		}

		public async Task<T> CreateAsync(T entity)
		{
			await this._context.Set<T>().AddAsync(entity);
			await SaveAllAsync();
			return entity;
		}

		public async Task<T> UpdateAsync(T entity)
		{
			this._context.Set<T>().Update(entity);
			await SaveAllAsync();
			return entity;
		}

		public async Task DeleteAsync(T entity)
		{
			this._context.Set<T>().Remove(entity);
			await SaveAllAsync();
		}

		public async Task<bool> ExistAsync(int id)
		{
			return await this._context.Set<T>().AnyAsync(e => e.Id == id);

		}
		public async Task<bool> SaveAllAsync()
		{
			return await this._context.SaveChangesAsync() > 0;
		}
	}
}
