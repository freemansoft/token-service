using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokenService.Model.Entity;

namespace TokenService.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthorizationRepository: IRepository<AuthorizationEntity>
    {

        /// <summary>
        /// Create a document with the identifier in the entity
        /// </summary>
        /// <param name="entity"></param>
        public void Create(AuthorizationEntity entity)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Delete a Document
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(AuthorizationEntity entity)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Get
        /// </summary>
        /// <param name="id">unique identifier</param>
        /// <returns></returns>
        public AuthorizationEntity GetById(string id)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Update a document,really "replace" if this is a document DB
        /// </summary>
        /// <param name="entity"></param>
        public void Update(AuthorizationEntity entity)
        {
            throw new NotImplementedException();
        }

    }
}
