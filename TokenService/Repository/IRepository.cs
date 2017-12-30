using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TokenService.Repository
{
    /// <summary>
    /// used to persist to our document store
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T>
    {
        /// <summary>
        /// Create a document with the identifier in the entity
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
        /// Update a document,really "replace" if this is a document DB
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);
    }
}
