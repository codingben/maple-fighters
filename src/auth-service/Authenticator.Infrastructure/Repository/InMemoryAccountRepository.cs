using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Authenticator.Domain.Aggregates.User;

namespace Authenticator.Infrastructure.Repository
{
    public class InMemoryAccountRepository : IAccountRepository
    {
        private readonly List<Account> collection;
        private readonly static object locker = new object();

        public InMemoryAccountRepository()
        {
            collection = new List<Account>();
        }

        public void Create(Account entity)
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

        public void Update(Account entity)
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

        public Account Read(Expression<Func<Account, bool>> predicate)
        {
            lock (locker)
            {
                return collection.AsQueryable().SingleOrDefault(predicate);
            }
        }

        public IEnumerable<Account> ReadMany(
            Expression<Func<Account, bool>> predicate)
        {
            lock (locker)
            {
                return collection.AsQueryable().Where(predicate);
            }
        }
    }
}