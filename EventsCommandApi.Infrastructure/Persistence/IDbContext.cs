namespace EventsCommandApi.Infrastructure.Persistence
{
    public interface IDbContext : IDisposable
    {
        Task<T?> FindByIdAsync<T>(string collectionName, string id, CancellationToken ct = default) where T : class;
        Task<IReadOnlyList<T>> FindAllAsync<T>(string collectionName, CancellationToken ct = default) where T : class;
        Task<string> InsertAsync<T>(string collectionName, T entity, CancellationToken ct = default) where T : class;
        Task UpdateAsync<T>(string collectionName, string id, T entity, CancellationToken ct = default) where T : class;
        Task DeleteAsync<T>(string collectionName, string id, CancellationToken ct = default) where T : class;
    }
}

