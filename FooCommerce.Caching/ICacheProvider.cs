namespace FooCommerce.Caching;

public interface ICacheProvider
{
    void Flush();

    Task FlushAsync(CancellationToken cancellationToken = default);

    Task<TModel> GetOrCreateAsync<TModel>(string key, Func<CancellationToken, Task<TModel>> factory, CacheOptions options = null, CancellationToken cancellationToken = default) where TModel : class;
    Task<TModel> GetOrCreateAsync<TModel>(string key, Func<CancellationToken, Task<TModel>> factory, CancellationToken cancellationToken = default) where TModel : class;
}