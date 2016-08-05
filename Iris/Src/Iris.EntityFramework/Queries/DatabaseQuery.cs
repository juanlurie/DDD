using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using Iris.Logging;
using Iris.Persistence;

namespace Iris.EntityFramework.Queries
{
    public class DatabaseQuery : IDatabaseQuery, IDisposable
    {
        internal protected static readonly ILog Logger = LogFactory.BuildLogger(typeof(DatabaseQuery));
        private readonly IContextFactory contextFactory;
        private bool disposed;
        protected DbContext Context;
        
        public DatabaseQuery(IContextFactory contextFactory)
        {
            this.contextFactory = contextFactory;
        }

        public IQueryable<TEntity> GetQueryable<TEntity>() where TEntity : class
        {
            return GetDbContext().Set<TEntity>().AsNoTracking();    
        }

        public IEnumerable<T> SqlQuery<T>(string sql, params SqlParameter[] parameters)
        {
            return GetDbContext().Database.SqlQuery<T>(sql, parameters);
        }

        public DbContext GetDbContext()
        {
            return Context ?? (Context = contextFactory.GetContext(ContextConfiguration.Queryable));
        }

        ~DatabaseQuery()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing && Context != null)
            {
                Context.Dispose();
                Context = null;
            }

            disposed = true;
        }
    }
}