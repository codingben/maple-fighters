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

        public InMemoryAccountRepository()
        {
            collection = new List<Account>();
        }

        public void Create(Account entity)
        {
            collection.Add(entity);
        }

        public void Delete(string id)
        {
            var account = collection.Find(x => x.Id == id);
            if (account != null)
            {
                collection.Remove(account);
            }
        }

        public void Update(Account entity)
        {
            var index = collection.FindIndex(x => x.Id == entity.Id);
            collection[index] = entity;
        }

        public Account Read(Expression<Func<Account, bool>> predicate)
        {
            return collection.AsQueryable().SingleOrDefault(predicate);
        }

        public IEnumerable<Account> ReadMany(
            Expression<Func<Account, bool>> predicate)
        {
            return collection.AsQueryable().Where(predicate);
        }
    }
}