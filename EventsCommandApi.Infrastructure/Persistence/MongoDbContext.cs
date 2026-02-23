using EventsCommandApi.Application.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace EventsCommandApi.Infrastructure.Persistence
{
    public sealed class MongoDbContext : IDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoOptions> options)
        {
            var mongo = options.Value;

            var settings = MongoClientSettings.FromConnectionString(mongo.ConnectionString);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            settings.MaxConnectionPoolSize = 100;
            settings.ConnectTimeout = TimeSpan.FromSeconds(30);

            var client = new MongoClient(settings);
            _database = client.GetDatabase(mongo.Database);
        }

        public async Task<T?> FindByIdAsync<T>(string collectionName, string id, CancellationToken ct = default) where T : class
        {
            var collection = _database.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("Id", id);
            return await collection.Find(filter).FirstOrDefaultAsync(ct);
        }

        public async Task<IReadOnlyList<T>> FindAllAsync<T>(string collectionName, CancellationToken ct = default) where T : class
        {
            var collection = _database.GetCollection<T>(collectionName);
            return await collection.Find(FilterDefinition<T>.Empty).ToListAsync(ct);
        }

        public async Task<string> InsertAsync<T>(string collectionName, T entity, CancellationToken ct = default) where T : class
        {
            var collection = _database.GetCollection<T>(collectionName);
            await collection.InsertOneAsync(entity, cancellationToken: ct);
            var idProperty = typeof(T).GetProperty("Id");
            return idProperty?.GetValue(entity)?.ToString() ?? string.Empty;
        }

        public async Task UpdateAsync<T>(string collectionName, string id, T entity, CancellationToken ct = default) where T : class
        {
            var collection = _database.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("Id", id);
            await collection.ReplaceOneAsync(filter, entity, cancellationToken: ct);
        }

        public async Task<long> DeleteAsync<T>(string collectionName, string id, CancellationToken ct = default) where T : class
        {
            var collection = _database.GetCollection<T>(collectionName);
            var filter = Builders<T>.Filter.Eq("_id", id);
            var result = await collection.DeleteOneAsync(filter, ct);
            
            return result.DeletedCount;
        }

        public void Dispose() { }
    }
}