using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper.DBContext;
using System.Linq.Expressions;
namespace EBS.Query.Service
{
    public abstract class AbstractQueryService
    {
        protected IQuery _query;
        public AbstractQueryService(IQuery query)
        {
            this._query = query;
        }
        public TEntity GetById<TEntity>(int id) where TEntity : class
        {
           return this._query.Find<TEntity>(id); 
        }
        public TEntity GetById<TEntity>(string id) where TEntity : class
        {
            return this._query.Find<TEntity>(id);
        }
        public IEnumerable<TEntity> GetById<TEntity>(int[] ids) where TEntity : class
        {
            return this._query.Find<TEntity>(ids);
        }
        public IEnumerable<TEntity> GetById<TEntity>(string[] ids) where TEntity : class
        {
            return this._query.Find<TEntity>(ids);
        }
        public TEntity GetById<TEntity>(Expression<Func<TEntity, bool>> expression) where TEntity : class
        {
            return this._query.Find<TEntity>(expression);
        }
    }
}
