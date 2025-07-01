using System;
using System.Threading.Tasks;

namespace Stock.Application.Common.Interfaces
{
    /// <summary>
    /// Defines the contract for a caching service.
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Gets or creates an item from the cache.
        /// If the item is not found in the cache, the factory function is executed to create it,
        /// and the result is stored in the cache before being returned.
        /// </summary>
        /// <typeparam name="T">The type of the item to be cached.</typeparam>
        /// <param name="key">The cache key.</param>
        /// <param name="factory">The function to create the item if it's not found in the cache.</param>
        /// <param name="absoluteExpirationRelativeToNow">Optional absolute expiration time.</param>
        /// <param name="slidingExpiration">Optional sliding expiration time.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The cached item.</returns>
        Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? absoluteExpirationRelativeToNow = null, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Removes an item from the cache.
        /// </summary>
        /// <param name="key">The cache key of the item to remove.</param>
        Task RemoveAsync(string key);

        /// <summary>
        /// Removes items from the cache based on a prefix.
        /// </summary>
        /// <param name="prefix">The prefix to match against cache keys.</param>
        Task RemoveByPrefixAsync(string prefix);
    }
} 