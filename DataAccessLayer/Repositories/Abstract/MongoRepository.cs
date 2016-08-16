
using DataAccessLayer.Contexts;
using DataAccessLayer.Extensions;
using Models.Abstract;
using Models.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Abstract
{
    public class MongoRepository<TDocument, TKey> : IMongoRepository<TDocument, TKey> where TDocument : IDocument<TKey>
    {
        #region PROPERTIES
        /// <summary>
        /// MongoDB Collection
        /// </summary>
        public MongoDBContext DbContext { get; private set; }

        public IMongoCollection<TDocument> Collection { get; private set; }

        /// <summary>
        /// Type-safe API for building up both simple and complex MongoDB queries
        /// </summary>
        public FilterDefinitionBuilder<TDocument> FilterBuilder
        {
            get
            {
                return Builders<TDocument>.Filter;
            }
        }

        /// <summary>
        /// Update Builder for collection
        /// </summary>
        public UpdateDefinitionBuilder<TDocument> UpdateBuilder
        {
            get
            {
                return Builders<TDocument>.Update;
            }
        }
        #endregion

        #region QUERY OBJECTS
        /// <summary>
        /// Fluent interface for find
        /// </summary>
        /// <param name="filter">Expression used to filter results</param>
        /// <returns></returns>
        private IFindFluent<TDocument, TDocument> Query(Expression<Func<TDocument, bool>> filter)
        {
            return Collection.Find(filter);
        }

        /// <summary>
        /// Fluent interface for find
        /// </summary>
        /// <returns></returns>
        private IFindFluent<TDocument, TDocument> Query()
        {
            return Collection.Find(FilterBuilder.Empty);
        }

        /// <summary>
        /// Returns the collection as an IQueryable object
        /// </summary>
        /// <returns></returns>
        private IQueryable<TDocument> Queryable()
        {
            return Collection.AsQueryable();
        }

        /// <summary>
        /// Returns the collection as a filtered IQueryable object
        /// </summary>
        /// <returns></returns>
        private IQueryable<TDocument> Queryable(string filter)
        {
            return Collection.AsQueryable().Where(filter);
        }

        /// <summary>
        /// Returns the collection as a filtered IQueryable object
        /// </summary>
        /// <returns></returns>
        private IQueryable<TDocument> Queryable(Expression<Func<TDocument, bool>> filter)
        {
            return Collection.AsQueryable().Where(filter);
        }
        #endregion

        #region CONSTRUCTORS
        public MongoRepository(MongoDBContext context) : this(context, GetCollectionName<TDocument>())
        { }

        public MongoRepository(MongoDBContext context, string collectionName)
        {
            DbContext = context;
            Collection = DbContext.Database.GetCollection<TDocument>(collectionName);
        }
        #endregion

        #region QUERIES
        #region GET BY ID
        /// <summary>
        /// Gets a single record using the document's unique identifier as <<TKey>>
        /// </summary>
        /// <param name="id">Unique identifier the document</param>
        /// <returns></returns>
        public virtual TDocument GetById(TKey id)
        {
            return Find(i => i.Id.Equals(id as string)).FirstOrDefault();
        }
        #endregion

        #region TO LIST
        /// <summary>
        /// Gets all entities from the collection
        /// </summary>
        /// <returns>An IEnumerable list of all entities in the collection</returns>
        public virtual IEnumerable<TDocument> ToList()
        {
            return Query().ToList();
        }

        /// <summary>
        /// Gets all records from the collection
        /// </summary>
        /// <returns>An IEnumerable list of all entities in the collection</returns>
        public virtual async Task<IEnumerable<TDocument>> ToListAsync()
        {
            return await Query().ToListAsync();
        }
        #endregion

        #region FIND
        /// <summary>
        /// Gets all records from the collection that match the specified filter criteria
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>An IEnumerable list of all entities in the collection that match the specified filter criteria</returns>
        public virtual IEnumerable<TDocument> Find(Expression<Func<TDocument, bool>> filter)
        {
            return Query(filter).ToList();
        }

        /// <summary>
        /// Gets all records from the collection that match the specified filter & order criteria
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public IEnumerable<TDocument> Find(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> order, bool isDescending)
        {
            return Find(filter).Sort(order, isDescending);
        }

        /// <summary>
        /// Gets all records from the collection that match the specified filter criteria
        /// </summary>
        /// <param name="filter">A string containing the filter(s) for which results should match (E.g. "productName.Equals(\"DODO\") and categoryName.Contains("\"ABC\")"</param>
        /// <returns></returns>
        public IEnumerable<TDocument> Find(string filter)
        {
            return Queryable(filter);
        }

        /// <summary>
        /// Gets all records from the collection that match the specified filter & order criteria
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public IEnumerable<TDocument> Find(string filter, string order)
        {
            return Find(filter).Sort(order);
        }

        /// <summary>
        /// Gets all records from the collection that match the specified filter criteria
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>An IEnumerable list of all entities in the collection that match the specified filter criteria</returns>
        public virtual async Task<IEnumerable<TDocument>> FindAsync(Expression<Func<TDocument, bool>> filter)
        {
            return await Query(filter).ToListAsync();
        }

        /// <summary>
        /// Gets all records from the collection that match the specified filter & order criteria
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TDocument>> FindAsync(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, object>> order, bool isDescending)
        {
            return await FindAsync(filter).ContinueWith((qry) => { return qry.Result.Sort(order, isDescending); });
            //return result.Sort(order, isDescending);
        }

        /// <summary>
        /// Gets all records from the collection that match the specified filter criteria
        /// </summary>
        /// <param name="filter">A string containing the filter(s) for which results should match (E.g. "productName.Equals(\"DODO\") and categoryName.Contains("\"ABC\")"</param>
        /// <returns></returns>
        public async Task<IEnumerable<TDocument>> FindAsync(string filter)
        {
            return await Query().ToListAsync().ContinueWith((qry) => { return qry.Result.Where(filter); });
        }

        /// <summary>
        /// Gets all records from the collection that match the specified filter criteria
        /// </summary>
        /// <param name="filter">A string containing the filter(s) for which results should match (E.g. "productName.Equals(\"DODO\") and categoryName.Contains("\"ABC\")"</param>
        /// <param name="order">A string containg the properties and direction in which to sort the results (E.g. "productName asc, categoryName desc")</param>
        /// <returns></returns>
        public async Task<IEnumerable<TDocument>> FindAsync(string filter, string order)
        {
            return await FindAsync(filter).ContinueWith((qry) => { return qry.Result.Sort(order); });
        }
        #endregion

        #region FIRST OR DEFAULT
        /// <summary>
        /// gets the first record in the collection
        /// </summary>
        /// <returns></returns>
        public virtual TDocument FirstOrDefault()
        {
            return Query().FirstOrDefault();
        }

        /// <summary>
        /// Gets the first document matching the specified predicate
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public virtual TDocument FirstOrDefault(Expression<Func<TDocument, bool>> filter)
        {
            return Query(filter).FirstOrDefault();
        }

        /// <summary>
        /// gets the first record in the collection
        /// </summary>
        /// <returns></returns>
        public async Task<TDocument> FirstOrDefaultAsync()
        {
            return await Query().FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets the first document matching the specified predicate
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<TDocument> FirstOrDefaultAsync(Expression<Func<TDocument, bool>> filter)
        {
            return await Query(filter).FirstOrDefaultAsync();
        }
        #endregion

        #region LAST
        /// <summary>
        /// Gets the last document
        /// </summary>
        /// <returns></returns>
        public TDocument Last()
        {
            return Page(1, 1, (i => i.Id), true).FirstOrDefault();
        }

        /// <summary>
        /// Gets the last document that meets the specified filter criteria
        /// </summary>
        /// <param name="filter">The predicate in which to filter the collection</param>
        /// <returns></returns>
        public TDocument Last(Expression<Func<TDocument, bool>> filter)
        {
            return Page(1, 1, (i => i.Id), true, filter).FirstOrDefault();
        }

        /// <summary>
        /// Gets the last document that meets the specified filter criteria
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public TDocument Last(string filter)
        {
            return Page(1, 1, "id desc", filter).FirstOrDefault();
        }

        public TDocument Last(Expression<Func<TDocument, object>> order)
        {
            return Page(1, 1, order, true).FirstOrDefault();
        }
        #endregion

        #region PAGE
        /// <summary>
        /// Pages the collection
        /// </summary>
        /// <param name="pageNo"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IEnumerable<TDocument> Page(int pageNo, int pageSize)
        {
            return ToList().Page(pageNo, pageSize);
        }

        /// <summary>
        /// Pages the collection
        /// </summary>
        /// <param name="pageNo">The page number to return</param>
        /// <param name="pageSize">The number of results to return</param>
        /// <param name="order">The property in which to sort the results</param>
        /// <param name="isDescending">Determines if results should be sorted in descending order</param>
        /// <returns></returns>
        public IEnumerable<TDocument> Page(int pageNo, int pageSize, Expression<Func<TDocument, object>> order, bool isDescending)
        {
            return ToList().Sort(order, isDescending).Page(pageNo, pageSize);
        }

        /// <summary>
        /// Pages the collection
        /// </summary>
        /// <param name="pageNo">The page number to return</param>
        /// <param name="pageSize">The number of results to return</param>
        /// <param name="order">The property in which to sort the results</param>
        /// <param name="isDescending">Determines if results should be sorted in descending order</param>
        /// <param name="filter">The filter expression that results should match</param>
        /// <returns></returns>
        public IEnumerable<TDocument> Page(int pageNo, int pageSize, Expression<Func<TDocument, object>> order, bool isDescending, Expression<Func<TDocument, bool>> filter)
        {
            return Find(filter).Sort(order, isDescending).Page(pageNo, pageSize);
        }

        /// <summary>
        /// Pages the collection
        /// </summary>
        /// <param name="pageNo">The page number to return</param>
        /// <param name="pageSize">The number of results to return</param>
        /// <param name="order">A string containg the properties and direction in which to sort the results (E.g. "productName asc, categoryName desc")</param>
        /// <returns></returns>
        public IEnumerable<TDocument> Page(int pageNo, int pageSize, string order)
        {
            return Queryable().Sort(order).Page(pageNo, pageSize);
        }

        /// <summary>
        /// Pages the collection
        /// </summary>
        /// <param name="pageNo">The page number to return</param>
        /// <param name="pageSize">The number of results to return</param>
        /// <param name="order">A string containg the properties and direction in which to sort the results (E.g. "productName asc, categoryName desc")</param>
        /// <param name="filter">A string containing the filter(s) for which results should match (E.g. "productName.Equals(\"DODO\") and categoryName.Contains("\"ABC\")"</param>
        /// <returns></returns>
        public IEnumerable<TDocument> Page(int pageNo, int pageSize, string order, string filter)
        {
            return Find(filter).Sort(order).Page(pageNo, pageSize);
        }
        #endregion
        #endregion

        #region PERSISTENCE
        #region ADD
        /// <summary>
        /// Inserts a single document
        /// </summary>
        /// <param name="document"></param>
        /// <param name="options"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public virtual TDocument InsertOne(TDocument document, InsertOneOptions options = null, CancellationToken ct = default(CancellationToken))
        {
            Collection.InsertOne(document, options, ct);
            return document;
        }

        /// <summary>
        /// Inserts a single document async
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="options"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public virtual IEnumerable<TDocument> InsertMany(IEnumerable<TDocument> entities, InsertManyOptions options = null, CancellationToken ct = default(CancellationToken))
        {
            Collection.InsertMany(entities, options, ct);
            return entities;
        }

        /// <summary>
        /// Inserts many documents
        /// </summary>
        /// <param name="document"></param>
        /// <param name="options"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public virtual async Task<TDocument> InsertOneAsync(TDocument document, InsertOneOptions options = null, CancellationToken ct = default(CancellationToken))
        {
            await Collection.InsertOneAsync(document, options, ct);
            return document;
        }

        /// <summary>
        /// Inserts many documents async
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="options"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TDocument>> InsertManyAsync(IEnumerable<TDocument> entities, InsertManyOptions options = null, CancellationToken ct = default(CancellationToken))
        {
            await Collection.InsertManyAsync(entities, options, ct);
            return entities;
        }
        #endregion

        #region UPDATE
        /// <summary>
        /// Updates specified field(s) for a document
        /// </summary>
        /// <param name="id">Unique Id for the document to be updated</param>
        /// <param name="updates">Specified field(s) to be updated</param>
        /// <returns>True if update was successful, false otherwise</returns>
        public virtual bool Update(TKey id, params UpdateDefinition<TDocument>[] updates)
        {
            return Update(FilterBuilder.Eq(i => i.Id, id), updates);
        }

        /// <summary>
        /// Updates specified field(s) for one or more documents as specified by the filter definition
        /// </summary>
        /// <param name="filterDefinition">The filter definition that determines which documents to update</param>
        /// <param name="updates">Specified field(s) to be updated</param>
        /// <returns></returns>
        public virtual bool Update(FilterDefinition<TDocument> filterDefinition, params UpdateDefinition<TDocument>[] updates)
        {
            var update = UpdateBuilder.Combine(updates).CurrentDate(i => i.ModifiedOn);
            return Collection.UpdateMany(filterDefinition, update).IsAcknowledged;
        }

        /// <summary>
        /// Updates specified field(s) for none or more documents as specified by the filter
        /// </summary>
        /// <param name="filter">The filter that determines which documents to update</param>
        /// <param name="updates">Specified field(s) to be updated</param>
        /// <returns></returns>
        public virtual bool Update(Expression<Func<TDocument, bool>> filter, params UpdateDefinition<TDocument>[] updates)
        {
            var update = UpdateBuilder.Combine(updates).CurrentDate(i => i.ModifiedOn);
            return Collection.UpdateMany(filter, update).IsAcknowledged;
        }

        /// <summary>
        /// Updates a specific field for none or more documents as specified by the filter
        /// </summary>
        /// <typeparam name="TField">The type of the field to be updated</typeparam>
        /// <param name="filter">The filter that determines which documents to update</param>
        /// <param name="field">The field to be updated</param>
        /// <param name="value">The value to update with</param>
        /// <returns></returns>
        public virtual bool Update<TField>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value)
        {
            return Update(filter, UpdateBuilder.Set(field, value));
        }

        /// <summary>
        /// Updates specified field(s) for a document
        /// </summary>
        /// <param name="id">Unique Id for the document to be updated</param>
        /// <param name="updates">Specified field(s) to be updated</param>
        /// <returns>True if update was successful, false otherwise</returns>
        public virtual async Task<bool> UpdateAsync(TKey id, params UpdateDefinition<TDocument>[] updates)
        {
            return await UpdateAsync(FilterBuilder.Eq(i => i.Id, id), updates);
        }

        /// <summary>
        /// Updates specified field(s) for one or more documents as specified by the filter definition
        /// </summary>
        /// <param name="filterDefinition">The filter definition that determines which documents to update</param>
        /// <param name="updates">Specified field(s) to be updated</param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateAsync(FilterDefinition<TDocument> filterDefinition, params UpdateDefinition<TDocument>[] updates)
        {
            var update = UpdateBuilder.Combine(updates).CurrentDate(i => i.ModifiedOn);
            var result = await Collection.UpdateManyAsync(filterDefinition, update);
            return result.IsAcknowledged;
        }

        /// <summary>
        /// Updates specified field(s) for none or more documents as specified by the filter
        /// </summary>
        /// <param name="filter">The filter that determines which documents to update</param>
        /// <param name="updates">Specified field(s) to be updated</param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateAsync(Expression<Func<TDocument, bool>> filter, params UpdateDefinition<TDocument>[] updates)
        {
            var update = UpdateBuilder.Combine(updates).CurrentDate(i => i.ModifiedOn);
            var result = await Collection.UpdateManyAsync(filter, update);
            return result.IsAcknowledged;
        }

        /// <summary>
        /// Updates a specific field for none or more documents as specified by the filter
        /// </summary>
        /// <typeparam name="TField">The type of the field to be updated</typeparam>
        /// <param name="filter">The filter that determines which documents to update</param>
        /// <param name="field">The field to be updated</param>
        /// <param name="value">The value to update with</param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateAsync<TField>(Expression<Func<TDocument, bool>> filter, Expression<Func<TDocument, TField>> field, TField value)
        {
            return await UpdateAsync(filter, UpdateBuilder.Set(field, value));
        }
        #endregion

        #region REPLACE
        /// <summary>
        /// Replaces the entire document
        /// </summary>
        /// <param name="document">The document to replace</param>
        /// <returns>True if the document was replaced successfully, false otherwise</returns>       
        public virtual bool ReplaceOne(TDocument document)
        {
            return ReplaceOne(FilterBuilder.Eq(i => i.Id, document.Id), document);
        }

        /// <summary>
        /// Replaces the entire document
        /// </summary>
        /// <param name="filterDefinition">The filter definition on which to retrieve the document</param>
        /// <param name="document">The document to replace</param>
        /// <returns>True if the document was replaced successfully, false otherwise</returns>
        private bool ReplaceOne(FilterDefinition<TDocument> filterDefinition, TDocument document)
        {
            return Collection.ReplaceOne(filterDefinition, document).IsAcknowledged;
        }

        /// <summary>
        /// Replaces the entire document
        /// </summary>
        /// <param name="document">The document to replace</param>
        /// <returns>True if the document was replaced successfully, false otherwise</returns>
        public virtual async Task<bool> ReplaceOneAsync(TDocument document)
        {
            return await ReplaceOneAsync(FilterBuilder.Eq(i => i.Id, document.Id), document);
        }

        /// <summary>
        /// Replaces the entire document
        /// </summary>
        /// <param name="filterDefinition">The filter definition on which to retrieve the document</param>
        /// <param name="document">The document to replace</param>
        /// <returns>True if the document was replaced successfully, false otherwise</returns>
        private async Task<bool> ReplaceOneAsync(FilterDefinition<TDocument> filterDefinition, TDocument document)
        {
            var result = await Collection.ReplaceOneAsync(filterDefinition, document);
            return result.IsAcknowledged;
        }
        #endregion

        #region DELETE
        /// <summary>
        /// Deletes a single document from the collection
        /// </summary>
        /// <param name="document"></param>
        public virtual void Delete(TDocument document)
        {
            Delete(document.Id);
        }

        /// <summary>
        /// Deletes a single document from the collection
        /// </summary>
        /// <param name="id"></param>
        public virtual void Delete(TKey id)
        {
            Collection.DeleteOne(i => i.Id.Equals(id));
        }

        /// <summary>
        /// Deletes documents from the collection as specified by the filter
        /// </summary>
        /// <param name="filter"></param>
        public virtual void DeleteMany(Expression<Func<TDocument, bool>> filter)
        {
            Collection.DeleteMany(filter);
        }

        /// <summary>
        /// Deletes a single document from the collection async
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(TDocument document)
        {
            await DeleteAsync(document.Id);
        }

        /// <summary>
        /// Deletes a single document from the collection async
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(TKey id)
        {
            await Collection.DeleteOneAsync(i => i.Id.Equals(id));
        }

        /// <summary>
        /// Deletes documents from the collection async as specified by the filter
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public virtual async Task DeleteManyAsync(Expression<Func<TDocument, bool>> filter)
        {
            await Collection.DeleteManyAsync(filter);
        }
        #endregion
        #endregion

        #region AGGREGATES
        #region COUNT
        /// <summary>
        /// Gets a count of all documents in the collection
        /// </summary>
        /// <returns>int32</returns>
        public virtual int Count()
        {
            return Queryable().Count();
        }

        /// <summary>
        /// Gets a count of all documents in the collection that match the specified criteria
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>int32</returns>
        public virtual int Count(Expression<Func<TDocument, bool>> filter)
        {
            return Queryable(filter).Count();
        }

        /// <summary>
        /// Gets a count of all documents in the collection
        /// </summary>
        /// <returns>long</returns>
        public virtual long LongCount()
        {
            return Queryable().LongCount();
        }

        /// <summary>
        /// Gets a count of all documents in the collection that match the specified criteria
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>long</returns>
        public virtual long LongCount(Expression<Func<TDocument, bool>> filter)
        {
            return Queryable(filter).LongCount();
        }
        #endregion
        #endregion

        #region HELPERS
        /// <summary>
        /// Gets the name of the collection from the 'CollectionName' attribute on the class OR the class name itself.
        /// </summary>
        /// <typeparam name="TDocument"></typeparam>
        /// <returns></returns>
        private static string GetCollectionName<T>() where T : IDocument<TKey>
        {
            var type = typeof(TDocument);

            // Check if this class is annotated with the 'CollectionNameAttribute'
            var attr = Attribute.GetCustomAttribute(type, typeof(CollectionNameAttribute));
            // If it is annotated, return the specifed collection name
            if (attr != null)
            {
                return ((CollectionNameAttribute)attr).Name;
            }
            else
            {
                // Just return the class name
                return type.Name;
            }
        }
        #endregion
    }

    /// <summary>
    /// MongoRepository that implements TKey of type string
    /// </summary>
    /// <typeparam name="TDocument"></typeparam>
    public class MongoRepository<TDocument> : MongoRepository<TDocument, string> where TDocument : IDocument<string>
    {
        #region CONSTRUCTORS
        public MongoRepository(MongoDBContext context) : base(context)
        { }

        public MongoRepository(MongoDBContext context, string collectionName) : base(context, collectionName)
        { }
        #endregion
    }
}
