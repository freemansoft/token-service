using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using TokenService.Core.Exception;
using TokenService.Core.Repository;
using TokenService.Model.Dto;
using TokenService.Model.Entity;

namespace TokenService.InMemory.Repository
{
    /// <summary>
    /// Used for token persistence
    /// </summary>
    public class TokenInMemRepository : IRepository<TokenEntity>
    {

        /// <summary>
        /// injected
        /// </summary>
        private readonly ILogger<TokenInMemRepository> _logger;

        IDictionary<string, TokenEntity> fakeStore = new Dictionary<string, TokenEntity>();

        /// <summary>
        /// Constructor with aut-injected logger
        /// </summary>
        /// <param name="logger"></param>
        public TokenInMemRepository(ILogger<TokenInMemRepository> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Create a document with the identifier in the entity.
        /// Throws exception if object already exists.
        /// </summary>
        /// <param name="entity"></param>
        public void Create(TokenEntity entity)
        {
            if (entity == null)
            {
                throw new BadArgumentException("Can't create token with null entity", new TokenResponse());
            }
            if (entity.Id == null)
            {
                throw new BadArgumentException("Can't create token without primary key", new TokenResponse());
            }
            fakeStore.Add(entity.Id, entity);
            _logger.LogDebug("Token store created : {0}", entity.Id);
        }
        /// <summary>
        /// Delete a Document
        /// </summary>
        /// <param name="entity"></param>
        public void Delete(TokenEntity entity)
        {
            if (entity == null)
            {
                return;
            }
            if (entity.Id == null)
            {
                throw new BadArgumentException("Can't create token without primary key", new TokenResponse());
            }
            fakeStore.Remove(entity.Id);
            _logger.LogDebug("Token store deleted : {0}", entity.Id);
        }
        /// <summary>
        /// Get
        /// </summary>
        /// <param name="id">unique identifier</param>
        /// <returns></returns>
        public TokenEntity GetById(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                throw new BadArgumentException("Can't retrieve token without primary key", new TokenResponse());
            }
            if (fakeStore.ContainsKey(id))
            {
                _logger.LogDebug("Token store found : {0}", id);
                return fakeStore[id];
            }
            else
            {
                _logger.LogDebug("Token store not found : {0}", id);
                return null;
            }
        }
        /// <summary>
        /// Update a document,really "replace" if this is a document DB.
        /// Accepts document that does not already exist without exception
        /// </summary>
        /// <param name="entity"></param>
        public void Update(TokenEntity entity)
        {
            if (entity == null)
            {
                throw new BadArgumentException("Can't Update token with null entity", new TokenResponse());
            }
            if (entity.Id == null)
            {
                throw new BadArgumentException("Can't Update token without primary key", new TokenResponse());
            }
            if (fakeStore.ContainsKey(entity.Id))
            {
                fakeStore.Remove(entity.Id);
            }
            fakeStore.Add(entity.Id, entity);
            _logger.LogDebug("Token store updated : {0}", entity.Id);
        }
    }
}
