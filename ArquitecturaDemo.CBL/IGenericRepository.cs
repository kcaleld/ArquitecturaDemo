using FluentValidation.Results;
using System.Linq.Expressions;

namespace ArquitecturaDemo.CBL
{
    public interface IGenericRepository<TEntity, TOutput> where TEntity : class where TOutput : class
    {
        Task<TOutput> GetByIdAsync(int id);
        Task<List<TOutput>> GetAllAsync();
        Task<TOutput> FindSingleAsync(Expression<Func<TEntity, bool>> expression);
        Task<List<TOutput>> FindListAsync(Expression<Func<TEntity, bool>> expression);
        Task<TOutput> AddAsync(TOutput entity);
        Task<bool> Remove(TOutput entity);
        Task<TOutput?> Update(TOutput entity);
        Task<ValidationResult> ValidateDtoAsync(TOutput entity);
        Task<bool> ValidateIfExistAsync(Expression<Func<TEntity, bool>> expression);
        Task<string> BulkInsertAsync(List<TOutput> entities);
        Task<string> BulkDeleteAsync(List<TOutput> entities);
        Task<string> BulkUpdateAsync(List<TOutput> entities);
        Task<string> BulkInsertOrUpdateAsync(List<TOutput> entities);
    }
}