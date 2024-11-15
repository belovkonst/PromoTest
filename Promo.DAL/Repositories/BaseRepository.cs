using Promo.Domain.Interfaces.Repositories;

namespace Promo.DAL.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    private readonly ApplicationDbContext _dbContext;

    public BaseRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
        if (entity == null) 
            throw new ArgumentNullException(nameof(entity), $"{ nameof(entity) } is null.");

        await _dbContext.AddAsync(entity);

        return entity;
    }

    public void Remove(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity), $"{nameof(entity)} is null.");

        _dbContext.Remove(entity);
    }

    public IQueryable<TEntity> GetAll()
    {
        return _dbContext.Set<TEntity>();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _dbContext.SaveChangesAsync();
    }

    public TEntity Update(TEntity entity)
    {
        if (entity == null)
            throw new ArgumentNullException(nameof(entity), $"{nameof(entity)} is null.");

        _dbContext.Update(entity);

        return entity;
    }

    public bool Any()
    {
        return _dbContext.Set<TEntity>().Any();
    }
}
