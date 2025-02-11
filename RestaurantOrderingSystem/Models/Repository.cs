using Microsoft.EntityFrameworkCore;
using RestaurantOrderingSystem.Data;
using System.Reflection.PortableExecutable;

namespace RestaurantOrderingSystem.Models
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected ApplicationDbContext _context {  get; set; }
        private DbSet<T> _dbSet {  get; set; }

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }







        public Task AddAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id, QueryOptions<T> options)
        {
            IQueryable<T> query = _dbSet;
            
            if(options.HasWhere)
            {
                query = query.Where(options.Where);
            }
            if(options.HasOrderBy)
            {
                query = query.OrderBy(options.OrderBy);
            }
            foreach(string include in options.GetIncludes())
            {
                query = query.Include(include);
            }



            //Find the primary key property name dynamically.
            var key = _context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.FirstOrDefault();

            //Extracting the Primary Key Name
            string primaryKeyName = key?.Name;

            //Retrieve the matching record by primary key.
            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, primaryKeyName) == id);
        }

        public Task UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
