using Entities.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models;
using Repositories.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Base
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {

        protected HanFishDbContext CurrentDbContext { get; set; }

        protected string ConnectionString
        {
            get
            {
                var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
                return builder.Build().GetConnectionString("HanFish");
            }
        }

        public IUnitOfWork UnitOfWork => CurrentDbContext;

        public BaseRepository(HanFishDbContext hanFishDbContext)
        {
            CurrentDbContext = hanFishDbContext;
        }

        #region 添加
        public async Task AddEntitiesAsync(IEnumerable<TEntity> entities)
        {
            await CurrentDbContext.AddRangeAsync(entities);
        }

        public async Task AddEntityAsync(TEntity entity)
        {
            await CurrentDbContext.AddAsync(entity);
        }
        #endregion

        #region 删除
        public void DeleteEntities(IEnumerable<TEntity> entities)
        {
            CurrentDbContext.RemoveRange(entities);
        }

        public void DeleteEntity(TEntity entity)
        {
            CurrentDbContext.Remove(entity);
        }
        #endregion

        #region 修改
        public void UpdateEntities(IEnumerable<TEntity> entities)
        {
            CurrentDbContext.UpdateRange(entities);
        }

        public void UpdateEntity(TEntity entity)
        {
            CurrentDbContext.Update(entity);
        }
        #endregion

        #region 查询
        public async Task<List<TEntity>> GetAllAsync()
        {
            return await CurrentDbContext.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<List<TEntity>> GetEntitiesAsync(Expression<Func<TEntity, bool>> where)
        {
            return await CurrentDbContext.Set<TEntity>().Where(where).ToListAsync();
        }

        public async Task<TEntity> GetEntityAsNoTrackingAsync(Expression<Func<TEntity, bool>> where, params string[] includeKeys)
        {
            var result = CurrentDbContext.Set<TEntity>().AsNoTracking();
            for (var i = 0; i <= includeKeys.Length - 1; i++)
            {
                result = result.Include(includeKeys[i]);
            }
            return await result.FirstOrDefaultAsync(where);
        }

        public async Task<TEntity> GetEntityAsync(Expression<Func<TEntity, bool>> where)
        {
            return await CurrentDbContext.Set<TEntity>().FirstOrDefaultAsync(where);
        }

        public async Task<List<TEntity>> GetListAsNoTrackingAsync(Expression<Func<TEntity, bool>> where, params string[] includeKeys)
        {
            var result = CurrentDbContext.Set<TEntity>().AsNoTracking().Where(where);
            for (var i = 0; i <= includeKeys.Length - 1; i++)
            {
                result = result.Include(includeKeys[i]);
            }
            return await result.ToListAsync();
        }

        public async Task<(List<TEntity>, int)> GetListToPaginationAsync(Expression<Func<TEntity, dynamic>> key, bool isOrderByDescending, int pageIndex, int count, params string[] includeKeys)
        {
            var total = CurrentDbContext.Set<TEntity>().Count();
            var result = CurrentDbContext.Set<TEntity>().AsNoTracking();
            if(isOrderByDescending) result = result.OrderByDescending(key);
            else result = result.OrderBy(key);
            result = result.Skip((pageIndex - 1) * count).Take(count);
            for ( var i = 0; i <= includeKeys.Length - 1; i++)
            {
                result = result.Include(includeKeys[i]);
            }
            return (await result.ToListAsync(), total);
        }

        public async Task<(List<TEntity>, int)> GetListToPaginationAsync(Expression<Func<TEntity, dynamic>> key, bool isOrderByDescending, Expression<Func<TEntity, bool>> where, int pageIndex, int count, params string[] includeKeys)
        {
            var total = CurrentDbContext.Set<TEntity>().Where(where).Count();
            var result = CurrentDbContext.Set<TEntity>().AsNoTracking().Where(where);
            if(isOrderByDescending) result = result.OrderByDescending(key);
            else result = result.OrderBy(key);
            result = result.Skip((pageIndex - 1) * count).Take(count);
            for (var i = 0; i <= includeKeys.Length - 1; i++)
            {
                result = result.Include(includeKeys[i]);
            }
            return (await result.ToListAsync(),total);
        }
        #endregion
    }
}
