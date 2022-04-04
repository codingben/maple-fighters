using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Authenticator.Domain.Aggregates.User;

namespace Authenticator.Infrastructure.Repository
{
    public class InMemoryAccountRepository : IAccountRepository
    {
        private readonly List<IAccount> collection;
        private readonly static object locker = new();

        public InMemoryAccountRepository()
        {
            collection = new List<IAccount>();
        }

        public void Create(IAccount entity)
        {
            lock (locker)
            {
                if (entity.Id == null)
                {
                    entity.Id = Guid.NewGuid().ToString();
                }

                collection.Add(entity);
            }
        }

        public void Delete(string id)
        {
            lock (locker)
            {
                var account = collection.Find(x => x.Id == id);
                if (account != null)
                {
                    collection.Remove(account);
                }
            }
        }

        public void Update(IAccount entity)
        {
            lock (locker)
            {
                var index = collection.FindIndex(x => x.Id == entity.Id);
                if (index != -1)
                {
                    collection[index] = entity;
                }
            }
        }

        public IAccount Read(Expression<Func<IAccount, bool>> predicate)
        {
            lock (locker)
            {
                return collection.AsQueryable().SingleOrDefault(predicate);
            }
        }

        public IEnumerable<IAccount> ReadMany(Expression<Func<IAccount, bool>> predicate)
        {
            lock (locker)
            {
                return collection.AsQueryable().Where(predicate);
            }
        }
    }
}