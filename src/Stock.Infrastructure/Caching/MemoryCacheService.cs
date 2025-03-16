using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Stock.Application.Common.Interfaces;

namespace Stock.Infrastructure.Caching
{
    /// <summary>
    /// Memory Cache kullanarak önbellek işlemlerini gerçekleştiren servis
    /// </summary>
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        /// <inheritdoc />
        public T Get<T>(string key)
        {
            return _memoryCache.TryGetValue(key, out T value) ? value : default;
        }

        /// <inheritdoc />
        public Task<T> GetAsync<T>(string key)
        {
            return Task.FromResult(Get<T>(key));
        }

        /// <inheritdoc />
        public void Set<T>(string key, T value, int expirationTime = 60)
        {
            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expirationTime)
            };

            _memoryCache.Set(key, value, options);
        }

        /// <inheritdoc />
        public Task SetAsync<T>(string key, T value, int expirationTime = 60)
        {
            Set(key, value, expirationTime);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        /// <inheritdoc />
        public Task RemoveAsync(string key)
        {
            Remove(key);
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public bool Exists(string key)
        {
            return _memoryCache.TryGetValue(key, out _);
        }

        /// <inheritdoc />
        public Task<bool> ExistsAsync(string key)
        {
            return Task.FromResult(Exists(key));
        }

        /// <inheritdoc />
        public void Clear()
        {
            // MemoryCache'de doğrudan tüm önbelleği temizleme metodu bulunmamaktadır.
            // Bu nedenle, bu işlev MemoryCache için uygulanamamaktadır.
            // Gerçek bir temizleme işlemi için, daha gelişmiş bir önbellek çözümü kullanılmalıdır.
            throw new NotImplementedException("MemoryCache'de doğrudan tüm önbelleği temizleme işlemi desteklenmemektedir.");
        }

        /// <inheritdoc />
        public Task ClearAsync()
        {
            Clear();
            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public T GetOrSet<T>(string key, Func<T> factory, int expirationTime = 60)
        {
            if (!_memoryCache.TryGetValue(key, out T value))
            {
                value = factory();
                Set(key, value, expirationTime);
            }

            return value;
        }

        /// <inheritdoc />
        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, int expirationTime = 60)
        {
            if (!_memoryCache.TryGetValue(key, out T value))
            {
                value = await factory();
                await SetAsync(key, value, expirationTime);
            }

            return value;
        }
    }
} 