using System;
using TokenService.Model;

namespace TokenService.Repository
{
    /// <summary>
    /// used to persist to our document store
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : IHasId
    {
        /// <summary>
        /// Create a document with the identifier in the entity.
        /// Throws exception if object already exists.
        /// </summary>
        /// <param name="entity"></param>
        void Create(T entity);
        /// <summary>
        /// Delete a Document
        /// </summary>
        /// <param name="entity"></param>
        void Delete(T entity);
        /// <summary>
        /// Get
        /// </summary>
        /// <param name="id">unique identifier</param>
        /// <returns></returns>
        T GetById(String id);
        /// <summary>
        /// Update a document,really "replace" if this is a document DB.
        /// Accepts document that does not already exist without exception
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);
    }
}
