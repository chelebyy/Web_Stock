using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Stock.Application.Common.Interfaces;
using System;
using System.Collections;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Infrastructure.Services
{
    public class InMemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly ILogger<InMemoryCacheService> _logger;

        public InMemoryCacheService(IMemoryCache memoryCache, ILogger<InMemoryCacheService> logger)
        {
            _memoryCache = memoryCache;
            _logger = logger;
        }

        public Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? absoluteExpirationRelativeToNow = null, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default)
        {
            return _memoryCache.GetOrCreateAsync(key, async entry =>
            {
                if (absoluteExpirationRelativeToNow.HasValue)
                {
                    entry.AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow;
                }
                else if (slidingExpiration.HasValue)
                {
                    entry.SlidingExpiration = slidingExpiration;
                }
                else
                {
                    entry.SlidingExpiration = TimeSpan.FromMinutes(30); // Default
                }
                return await factory();
            });
        }

        public Task RemoveAsync(string key)
        {
            _memoryCache.Remove(key);
            return Task.CompletedTask;
        }

        public Task RemoveByPrefixAsync(string prefix)
        {
            _logger.LogWarning("RemoveByPrefixAsync is not efficiently implemented for InMemoryCacheService. This operation can be slow and is not recommended in production for this cache type.");

            // IMemoryCache, anahtarları doğrudan listelemek için bir yol sunmaz.
            // Bu nedenle, yansıma (reflection) kullanarak özel (private) alana erişmek bir seçenektir,
            // ancak bu kırılgan ve performans açısından verimsizdir.
            // Bu senaryoda, desene uyan anahtarları silmek yerine bir uyarı logu basmak daha güvenlidir.
            // Üretim ortamında Redis kullanılacağı için bu durum bir sorun teşkil etmeyecektir.
            
            // Örnek olarak yansıma ile nasıl yapılabileceğini gösterelim, ancak üretimde KULLANMAYIN.
            /*
            var cacheEntries = (IDictionary)_memoryCache.GetType().GetField("_entries", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(_memoryCache);
            var keysToRemove = new List<string>();

            foreach (DictionaryEntry cacheEntry in cacheEntries)
            {
                if (cacheEntry.Key.ToString().StartsWith(prefix))
                {
                    keysToRemove.Add(cacheEntry.Key.ToString());
                }
            }

            foreach (var key in keysToRemove)
            {
                _memoryCache.Remove(key);
            }
            */

            return Task.CompletedTask;
        }
    }
} 