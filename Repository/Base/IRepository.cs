using Entities.Base;
using Models;
using System.Linq.Expressions;
using static Dapper.SqlMapper;

namespace Repositories.Base
{
    public interface IRepository<T> where T : BaseEntity
    {
        IUnitOfWork UnitOfWork { get; }

        #region 添加
        Task AddEntityAsync(T entity);
        Task AddEntitiesAsync(IEnumerable<T> entities);
        #endregion

        #region 删除
        void DeleteEntity(T entity);
        void DeleteEntities(IEnumerable<T> entities);
        #endregion

        #region 修改
        void UpdateEntity(T entity);
        void UpdateEntities(IEnumerable<T> entities);
        #endregion

        #region 查询
        Task<List<T>> GetAllAsync();

        Task<List<T>> GetListAsNoTrackingAsync(Expression<Func<T, bool>> where, params string[] includeKeys);

        Task<List<T>> GetEntitiesAsync(Expression<Func<T, bool>> where);

        Task<T> GetEntityAsync(Expression<Func<T, bool>> where);

        Task<T> GetEntityAsNoTrackingAsync(Expression<Func<T, bool>> where, params string[] includeKeys);

        Task<(List<T>, int)> GetListToPaginationAsync(Expression<Func<T, dynamic>> key, bool isOrderByDescending, int pageIndex,int count, params string[] includeKeys);

        Task<(List<T>, int)> GetListToPaginationAsync(Expression<Func<T, dynamic>> key, bool isOrderByDescending, Expression<Func<T, bool>> where, int pageIndex,int count, params string[] includeKeys);
        #endregion
    }
}
