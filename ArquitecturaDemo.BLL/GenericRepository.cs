using ArquitecturaDemo.CBL;
using ArquitecturaDemo.DAL;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using EFCore.BulkExtensions;
using ArquitecturaDemo.Shared.Helpers;
using Microsoft.Extensions.Logging;
using static ArquitecturaDemo.Shared.Helpers.Enum;

namespace ArquitecturaDemo.BLL
{
    public class GenericRepository<TEntity, TOutput> : IGenericRepository<TEntity, TOutput> where TEntity : class where TOutput : class
    {
        protected readonly UsersContext _context;
        protected readonly IMapper _mapper;
        private readonly IValidator<TOutput> _validator;
        private readonly ILogger _logger;
        private readonly string _entityName;

        public GenericRepository(UsersContext context, IMapper mapper, IValidator<TOutput> validator, ILogger<GenericRepository<TEntity, TOutput>> logger)
        {
            _context = context;
            _mapper = mapper;
            _validator = validator;
            _logger = logger;
            _entityName = typeof(TEntity).Name;
        }

        public async Task<TOutput> AddAsync(TOutput entity)
        {
            _logger.LogInformation(Const.StartOperationLog);
            var entityMapped = _mapper.Map<TEntity>(entity);

            _logger.LogInformation(Const.InformationLog, Operation.Creacion, _entityName);

            await _context.Set<TEntity>().AddAsync(entityMapped);

            await _context.SaveChangesAsync();

            _logger.LogInformation(Const.EndOperationLog);
            return _mapper.Map<TOutput>(entityMapped);
        }

        public async Task<string> BulkDeleteAsync(List<TOutput> entities)
        {
            _logger.LogInformation(Const.StartOperationLog);
            var tran = await _context.Database.BeginTransactionAsync();
            try
            {
                var listMapped = _mapper.Map<List<TEntity>>(entities);

                _logger.LogInformation(Const.BulkInformationLog, Operation.Eliminacion, _entityName);

                await _context.BulkDeleteAsync(listMapped);
                await _context.SaveChangesAsync();
                await tran.CommitAsync();

                _logger.LogInformation(Const.EndOperationLog);
                return Const.StringEmpty;
            }
            catch (Exception ex) 
            {
                _logger.LogError(Const.ErrorOperationLog, ex.Message);
                await tran.RollbackAsync(); 
                return ex.Message; 
            }
        }

        public async Task<string> BulkInsertAsync(List<TOutput> entities)
        {
            _logger.LogInformation(Const.StartOperationLog);
            var tran = await _context.Database.BeginTransactionAsync();
            try
            {
                var listMapped = _mapper.Map<List<TEntity>>(entities);

                _logger.LogInformation(Const.BulkInformationLog, Operation.Creacion, _entityName);

                await _context.BulkInsertAsync(listMapped);
                await _context.SaveChangesAsync();
                await tran.CommitAsync();

                _logger.LogInformation(Const.EndOperationLog);
                return Const.StringEmpty;
            }
            catch (Exception ex) 
            {
                _logger.LogError(Const.ErrorOperationLog, ex.Message);
                await tran.RollbackAsync();
                return ex.Message;
            }
        }

        public async Task<string> BulkInsertOrUpdateAsync(List<TOutput> entities)
        {
            _logger.LogInformation(Const.StartOperationLog);
            var tran = await _context.Database.BeginTransactionAsync();
            try
            {
                var listMapped = _mapper.Map<List<TEntity>>(entities);

                _logger.LogInformation(Const.BulkInformationLog, $"{Operation.Creacion}/{Operation.Actualizacion}", _entityName);

                await _context.BulkInsertOrUpdateAsync(listMapped);
                await _context.SaveChangesAsync();
                await tran.CommitAsync();

                _logger.LogInformation(Const.EndOperationLog);
                return Const.StringEmpty;
            }
            catch (Exception ex) 
            {
                _logger.LogError(Const.ErrorOperationLog, ex.Message);
                await tran.RollbackAsync();
                return ex.Message;
            }
        }

        public async Task<string> BulkUpdateAsync(List<TOutput> entities)
        {
            _logger.LogInformation(Const.StartOperationLog);
            var tran = await _context.Database.BeginTransactionAsync();
            try
            {
                var listMapped = _mapper.Map<List<TEntity>>(entities);

                _logger.LogInformation(Const.BulkInformationLog, Operation.Actualizacion, _entityName);

                await _context.BulkUpdateAsync(listMapped);
                await _context.SaveChangesAsync();
                await tran.CommitAsync();

                _logger.LogInformation(Const.EndOperationLog);
                return Const.StringEmpty;
            }
            catch (Exception ex)
            {
                _logger.LogError(Const.ErrorOperationLog, ex.Message);
                await tran.RollbackAsync();
                return ex.Message;
            }
        }

        public async Task<List<TOutput>> FindListAsync(Expression<Func<TEntity, bool>> expression)
        {
            _logger.LogInformation(Const.StartOperationLog);
            _logger.LogInformation(Const.InformationLog, Operation.Lectura, _entityName);

            var result = await _context.Set<TEntity>().AsNoTracking().Where(expression).ToListAsync();

            _logger.LogInformation(Const.EndOperationLog);
            return _mapper.Map<List<TOutput>>(result);
        }

        public async Task<TOutput> FindSingleAsync(Expression<Func<TEntity, bool>> expression)
        {
            _logger.LogInformation(Const.StartOperationLog);
            _logger.LogInformation(Const.InformationLog, Operation.Lectura, _entityName);

            var result = await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(expression);

            _logger.LogInformation(Const.EndOperationLog);
            return _mapper.Map<TOutput>(result);
        }

        public async Task<List<TOutput>> GetAllAsync()
        {
            _logger.LogInformation(Const.StartOperationLog);
            _logger.LogInformation(Const.InformationLog, Operation.Lectura, _entityName);

            var result = await _context
                .Set<TEntity>()
                .AsNoTracking()
                .ToListAsync();

            _logger.LogInformation(Const.EndOperationLog);
            return _mapper.Map<List<TOutput>>(result);
        }

        public async Task<TOutput> GetByIdAsync(int id)
        {
            _logger.LogInformation(Const.StartOperationLog);
            _logger.LogInformation(Const.InformationLog, Operation.Lectura, _entityName);

            var result = await _context.Set<TEntity>().FindAsync(id);

            _logger.LogInformation(Const.EndOperationLog);
            return _mapper.Map<TOutput>(result);
        }

        public async Task<bool> Remove(TOutput entity)
        {
            _logger.LogInformation(Const.StartOperationLog);
            var tran = await _context.Database.BeginTransactionAsync();
            try
            {
                var result = _mapper.Map<TEntity>(entity);

                _logger.LogInformation(Const.InformationLog, Operation.Eliminacion, _entityName);

                _context.Set<TEntity>().Remove(result);
                await _context.SaveChangesAsync();
                await tran.CommitAsync();

                _logger.LogInformation(Const.EndOperationLog);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(Const.ErrorOperationLog, ex.Message);
                await tran.RollbackAsync();
                return false;
            }
        }

        public async Task<TOutput?> Update(TOutput entity)
        {
            _logger.LogInformation(Const.StartOperationLog);
            var tran = await _context.Database.BeginTransactionAsync();
            try
            {
                var result = _mapper.Map<TEntity>(entity);

                _logger.LogInformation(Const.InformationLog, Operation.Actualizacion, _entityName);

                _context.Set<TEntity>().Update(result);
                await _context.SaveChangesAsync();
                await tran.CommitAsync();

                _logger.LogInformation(Const.EndOperationLog);
                return _mapper.Map<TOutput>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(Const.ErrorOperationLog, ex.Message);
                await tran.RollbackAsync();
                return null;
            }
        }

        public async Task<ValidationResult> ValidateDtoAsync(TOutput entity)
        {
            _logger.LogInformation(Const.StartOperationLog);
            _logger.LogInformation(Const.InformationLog, Operation.Validacion, _entityName);

            var result = await _validator.ValidateAsync(entity);

            _logger.LogInformation(Const.EndOperationLog);
            return result;
        }

        public async Task<bool> ValidateIfExistAsync(Expression<Func<TEntity, bool>> expression)
        {
            _logger.LogInformation(Const.StartOperationLog);
            _logger.LogInformation(Const.InformationLog, Operation.Validacion, _entityName);

            var result = await _context.Set<TEntity>().AsNoTracking().AnyAsync(expression);

            _logger.LogInformation(Const.EndOperationLog);
            return result;
        }
    }
}