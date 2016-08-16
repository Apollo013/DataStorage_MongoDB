using Models.Abstract;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Abstract
{
    public interface IMongoRepository<TDocument, TKey> where TDocument : IDocument<TKey>
    {
        #region PROPERTIES
        IMongoCollection<TDocument> Collection { get; }
        FilterDefinitionBuilder<TDocument> FilterBuilder { get; }
        UpdateDefinitionBuilder<TDocument> UpdateBuilder { get; }
        #endregion

        #region QUERIES
        #region GET BY ID
        TDocument GetById(TKey Id);
        #endregion

        #region TO LIST
        IEnumerable<TDocument> ToList();
        Task<IEnumerable<TDocument>> ToListAsync();
        #endregion

        #region FIRST OR DEFAULT
        TDocument FirstOrDefault();
        TDocument FirstOrDefault(Expression<Func<TDocument, bool>> filter);
        Task<TDocument> FirstOrDefaultAsync();
        Task<TDocument> FirstOrDefaultAsync(Expression<Func<TDocument, bool>> filter);
        #endregion

        #region LAST
        TDocument Last();
        TDocument Last(Expression<Func<TDocument, bool>> filter);
        TDocument Last(string filter);
        TDocument Last(Expression<Func<TDocument, object>> order);
        #endregion

        #region FIND
        IEnumerable<TDocument> Find(Expression<Func<TDocument, bool>> filter);
        IEnumerable<TDocument> Find(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> order, bool isDescending);
        IEnumerable<TDocument> Find(string filter);
        IEnumerable<TDocument> Find(string filter, string order);
        Task<IEnumerable<TDocument>> FindAsync(Expression<Func<TDocument, bool>> filter);
        Task<IEnumerable<TDocument>> FindAsync(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> order, bool isDescending);
        Task<IEnumerable<TDocument>> FindAsync(string filter);
        Task<IEnumerable<TDocument>> FindAsync(string filter, string order);
        #endregion

        #region PAGE
        IEnumerable<TDocument> Page(int pageNo, int pageSize);
        IEnumerable<TDocument> Page(int pageNo, int pageSize, Expression<Func<TDocument, object>> order, bool isDescending);
        IEnumerable<TDocument> Page(int pageNo, int pageSize, Expression<Func<TDocument, object>> order, bool isDescending, Expression<Func<TDocument, bool>> filter);
        IEnumerable<TDocument> Page(int pageNo, int pageSize, string order);
        IEnumerable<TDocument> Page(int pageNo, int pageSize, string order, string filter);
        #endregion
        #endregion

        #region PERSISTENCE
        #region ADD
        TDocument InsertOne(TDocument entity, InsertOneOptions options = null, CancellationToken ct = default(CancellationToken));
        IEnumerable<TDocument> InsertMany(IEnumerable<TDocument> entities, InsertManyOptions options = null, CancellationToken ct = default(CancellationToken));
        Task<TDocument> InsertOneAsync(TDocument entity, InsertOneOptions options = null, CancellationToken ct = default(CancellationToken));
        Task<IEnumerable<TDocument>> InsertManyAsync(IEnumerable<TDocument> entities, InsertManyOptions options = null, CancellationToken ct = default(CancellationToken));
        #endregion

        #region UPDATE
        bool Update(TKey id, params UpdateDefinition<TDocument>[] updates);
        bool Update(FilterDefinition<TDocument> filterDefinition, params UpdateDefinition<TDocument>[] updates);
        bool Update(Expression<Func<TDocument, bool>> filter, params UpdateDefinition<TDocument>[] updates);
        bool Update<TField>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value);
        Task<bool> UpdateAsync(TKey id, params UpdateDefinition<TDocument>[] updates);
        Task<bool> UpdateAsync(FilterDefinition<TDocument> filterDefinition, params UpdateDefinition<TDocument>[] updates);
        Task<bool> UpdateAsync(Expression<Func<TDocument, bool>> filter, params UpdateDefinition<TDocument>[] updates);
        Task<bool> UpdateAsync<TField>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value);
        #endregion

        #region REPLACE
        bool ReplaceOne(TDocument document);
        Task<bool> ReplaceOneAsync(TDocument document);
        #endregion

        #region DELETE
        void Delete(TDocument entity);
        void Delete(TKey id);
        void DeleteMany(Expression<Func<TDocument, bool>> filter);
        Task DeleteAsync(TDocument entity);
        Task DeleteAsync(TKey id);
        Task DeleteManyAsync(Expression<Func<TDocument, bool>> filter);
        #endregion
        #endregion

        #region COUNT
        int Count();
        int Count(Expression<Func<TDocument, bool>> filter);
        long LongCount();
        long LongCount(Expression<Func<TDocument, bool>> filter);
        #endregion
    }
}
