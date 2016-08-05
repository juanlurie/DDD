using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Iris.Persistence
{
    public interface IDatabaseQuery
    {
        IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class;
        IEnumerable<T> SqlQuery<T>(string sql, params SqlParameter[] parameters);
    }
}