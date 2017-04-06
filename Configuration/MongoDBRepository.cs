using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

///
//   Repository implementation for Azure DocumentDB
///
namespace Tokens.Configuration
{
    public class MongoDBRepository<T> : IRepository<T> where T : class
    {
        private readonly string _databaseId;
        private readonly string _collectionId;
        private readonly MongoClient _client;
        private readonly MongoServer _server;
        private readonly MongoDatabase _db;
        private readonly MongoCollection _col;
        private readonly ILogger<MongoDBRepository<T>> _logger;

        public MongoDBRepository(IOptions<MongoDBOptions> settings, ILogger<MongoDBRepository<T>> logger)
        {
            _logger = logger;
            var options = settings.Value;
            _databaseId = options.Database;
            _collectionId = options.Collection;
            _client = new MongoClient(options.Url);
            _server = _client.GetServer();
            _db = _server.GetDatabase(_databaseId);
            _col = _db.GetCollection<T>(_collectionId);      
        }
        
        public async Task<T> GetItemAsync(string id)
        {
            _logger.LogInformation("MongoDBRepository::GetItemAsync");
            try
            {
                return await _col.FindOneAsync(x => x.uid == id);
            }
            catch (Exception e)
            {
                _logger.LogError("MongoDBRepository::GetItemAsync", e.Message);
                throw;
            }
        }

        public async Task<Document> CreateItemAsync(T item)
        {
            _logger.LogInformation("MongoDBRepository::CreateItemAsync");
            return await _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId), item);
        }

        public async Task DeleteItemAsync(string id)
        {
            await _client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(_databaseId, _collectionId, id));
        }

    }
}