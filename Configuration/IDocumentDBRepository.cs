using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;

namespace Tokens.Configuration
{
    public interface IRepository<T> where T: class
    {
        Task<T> GetItemAsync(string id);
        Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate);
        Task<Document> CreateItemAsync(T item);
        Task<Document> UpdateItemAsync(string id, T item);
        Task DeleteItemAsync(string id);
    }
}
