using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LogBookReader.EF
{
    public class Repository<TEntity> where TEntity : class
    {
        protected readonly DbContext _dbContext;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbSet = _dbContext.Set<TEntity>();
        }


        public TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate = null,
                                         Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                return orderBy(query).FirstOrDefault();
            else
                return query.FirstOrDefault();
        }

        public TEntity Find(params object[] keyValues) => _dbSet.Find(keyValues);
    
        public Task<TEntity> FindAsync(params object[] keyValues) => _dbSet.FindAsync(keyValues);

        public int Count(Expression<Func<TEntity, bool>> predicate = null)
        {
            if (predicate == null)
                return _dbSet.Count();
            else
                return _dbSet.Count(predicate);
        }

        public List<TEntity> GetList(Expression<Func<TEntity, bool>> predicate = null,
                                     Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                return orderBy(query).ToList();
            else
                return query.ToList();
        }

        public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate = null,
                                                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                return await orderBy(query).ToListAsync();
            else
                return await query.ToListAsync();
        }

        public async Task<List<TEntity>> GetListTakeAsync(Expression<Func<TEntity, bool>> predicate = null,
                                                          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                          int count = 100)
        {
            IQueryable<TEntity> query = _dbSet;

            if (predicate != null)
                query = query.Where(predicate);

            if (orderBy != null)
                return await Task.Run(
                    () =>
                    {
                        Events.ChangeIsLoadingEventLogEvents.ChangeValue(true);

                        var result = orderBy(query).Take(count)
                                               .ToListAsync();

                        Events.ChangeIsLoadingEventLogEvents.ChangeValue(false);
                        
                        return result;
                    }
                    );
            else
                return await query.Take(count).ToListAsync();
        }
    
        public TResult GetMin<TResult>(Expression<Func<TEntity, TResult>> selector = null) 
        {
            return _dbSet.Min(selector);
        }
    }
}
