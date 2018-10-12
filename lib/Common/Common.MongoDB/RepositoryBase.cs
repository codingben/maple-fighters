using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Common.ComponentModel;
using Common.Repository;
using MongoDB.Driver;

namespace Common.MongoDB
{
    public abstract class MongoRepositoryBase<TEntity> : ComponentBase, 
                                                         IRepository<TEntity, string>
        where TEntity : IMongoEntity
    {
        protected IMongoCollection<TEntity> Collection => GetCollection();

        protected abstract IMongoCollection<TEntity> GetCollection();

        public async Task CreateAsync(TEntity entity)
        {
            await Collection.InsertOneAsync(entity);
        }

        public async Task DeleteAsync(string id)
        {
            await Collection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            await Collection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
        }

        public async Task<TEntity> ReadAsync(
            Expression<Func<TEntity, bool>> predicate)
        {
            return (await Collection.FindAsync(predicate)).FirstOrDefault();
        }

        public async Task<IEnumerable<TEntity>> ReadManyAsync(
            Expression<Func<TEntity, bool>> predicate)
        {
            return await Collection.Find(predicate).ToListAsync();
        }
    }
}