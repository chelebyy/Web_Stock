using System;
using System.Threading.Tasks;

namespace Stock.Application.Common.Interfaces
{
    /// <summary>
    /// Önbellek işlemleri için arayüz
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Önbellekten veri getirme
        /// </summary>
        /// <typeparam name="T">Veri tipi</typeparam>
        /// <param name="key">Önbellek anahtarı</param>
        /// <returns>Önbellekteki veri veya default değer</returns>
        T Get<T>(string key);
        
        /// <summary>
        /// Önbellekten asenkron veri getirme
        /// </summary>
        /// <typeparam name="T">Veri tipi</typeparam>
        /// <param name="key">Önbellek anahtarı</param>
        /// <returns>Önbellekteki veri veya default değer</returns>
        Task<T> GetAsync<T>(string key);
        
        /// <summary>
        /// Önbelleğe veri ekleme
        /// </summary>
        /// <typeparam name="T">Veri tipi</typeparam>
        /// <param name="key">Önbellek anahtarı</param>
        /// <param name="value">Eklenecek veri</param>
        /// <param name="expirationTime">Önbellekte kalma süresi (dakika)</param>
        void Set<T>(string key, T value, int expirationTime = 60);
        
        /// <summary>
        /// Önbelleğe asenkron veri ekleme
        /// </summary>
        /// <typeparam name="T">Veri tipi</typeparam>
        /// <param name="key">Önbellek anahtarı</param>
        /// <param name="value">Eklenecek veri</param>
        /// <param name="expirationTime">Önbellekte kalma süresi (dakika)</param>
        Task SetAsync<T>(string key, T value, int expirationTime = 60);
        
        /// <summary>
        /// Önbellekten veri silme
        /// </summary>
        /// <param name="key">Önbellek anahtarı</param>
        void Remove(string key);
        
        /// <summary>
        /// Önbellekten asenkron veri silme
        /// </summary>
        /// <param name="key">Önbellek anahtarı</param>
        Task RemoveAsync(string key);
        
        /// <summary>
        /// Önbellekte veri var mı kontrolü
        /// </summary>
        /// <param name="key">Önbellek anahtarı</param>
        /// <returns>Veri varsa true, yoksa false</returns>
        bool Exists(string key);
        
        /// <summary>
        /// Önbellekte asenkron veri var mı kontrolü
        /// </summary>
        /// <param name="key">Önbellek anahtarı</param>
        /// <returns>Veri varsa true, yoksa false</returns>
        Task<bool> ExistsAsync(string key);
        
        /// <summary>
        /// Önbelleği temizleme
        /// </summary>
        void Clear();
        
        /// <summary>
        /// Önbelleği asenkron temizleme
        /// </summary>
        Task ClearAsync();
        
        /// <summary>
        /// Önbellekten veri getirme veya yoksa ekleme
        /// </summary>
        /// <typeparam name="T">Veri tipi</typeparam>
        /// <param name="key">Önbellek anahtarı</param>
        /// <param name="factory">Veri yoksa çalışacak fonksiyon</param>
        /// <param name="expirationTime">Önbellekte kalma süresi (dakika)</param>
        /// <returns>Önbellekteki veri veya factory'den dönen veri</returns>
        T GetOrSet<T>(string key, Func<T> factory, int expirationTime = 60);
        
        /// <summary>
        /// Önbellekten asenkron veri getirme veya yoksa ekleme
        /// </summary>
        /// <typeparam name="T">Veri tipi</typeparam>
        /// <param name="key">Önbellek anahtarı</param>
        /// <param name="factory">Veri yoksa çalışacak asenkron fonksiyon</param>
        /// <param name="expirationTime">Önbellekte kalma süresi (dakika)</param>
        /// <returns>Önbellekteki veri veya factory'den dönen veri</returns>
        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> factory, int expirationTime = 60);
    }
} 