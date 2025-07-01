using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using Stock.Application.Common.Interfaces;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Threading;

namespace Stock.Infrastructure.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _cache;
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _database;
        private readonly JsonSerializerOptions _serializerOptions;

        public CacheService(IDistributedCache cache, IConnectionMultiplexer redis)
        {
            _cache = cache;
            _redis = redis;
            _database = redis.GetDatabase();
            _serializerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, TimeSpan? absoluteExpirationRelativeToNow = null, TimeSpan? slidingExpiration = null, CancellationToken cancellationToken = default)
        {
            var cachedValue = await _cache.GetStringAsync(key, cancellationToken);
            if (!string.IsNullOrEmpty(cachedValue))
            {
                return JsonSerializer.Deserialize<T>(cachedValue, _serializerOptions);
            }

            var value = await factory();
            if (value == null)
            {
                return default;
            }

            var options = new DistributedCacheEntryOptions();
            if (absoluteExpirationRelativeToNow.HasValue)
            {
                options.SetAbsoluteExpiration(absoluteExpirationRelativeToNow.Value);
            }
            else if (slidingExpiration.HasValue)
            {
                options.SetSlidingExpiration(slidingExpiration.Value);
            }
            else
            {
                // Default expiration if none provided
                options.SetSlidingExpiration(TimeSpan.FromMinutes(30));
            }

            var serializedValue = JsonSerializer.Serialize(value, _serializerOptions);
            await _cache.SetStringAsync(key, serializedValue, options, cancellationToken);

            return value;
        }

        public async Task RemoveAsync(string key)
        {
            await _cache.RemoveAsync(key);
        }

        public async Task RemoveByPrefixAsync(string prefix)
        {
            // Bu sunucu endpoint'lerini alırken dikkatli olun, uygulamanızın dağıtımına bağlı olarak değişebilir.
            var endpoints = _redis.GetEndPoints();
            foreach (var endpoint in endpoints)
            {
                var server = _redis.GetServer(endpoint);
                // Redis 'KEYS' komutu dikkatli kullanılmalıdır. Büyük veri setlerinde performansı etkileyebilir.
                // Bizim durumumuzda, belirli bir prefix ile çalıştığımız için bu kabul edilebilir.
                var keys = server.Keys(pattern: $"{prefix}*").ToArray();
                if (keys.Length > 0)
                {
                    await _database.KeyDeleteAsync(keys);
                }
            }
        }
    }
} 