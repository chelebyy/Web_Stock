using Microsoft.Extensions.Caching.Memory;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using Stock.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stock.Infrastructure.Repositories
{
    public class CachedRoleRepository : RoleRepository
    {
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(10);

        private const string ALL_ROLES_CACHE_KEY = "AllRoles";
        private const string ROLE_BY_ID_CACHE_KEY = "Role_{0}"; // Role_1, Role_2, etc.
        private const string ROLE_BY_NAME_CACHE_KEY = "Role_Name_{0}"; // Role_Name_Admin, etc.

        public CachedRoleRepository(ApplicationDbContext context, IMemoryCache cache) 
            : base(context)
        {
            _cache = cache;
        }

        public override async Task<IEnumerable<Role>> GetAllAsync()
        {
            if (!_cache.TryGetValue(ALL_ROLES_CACHE_KEY, out IEnumerable<Role> roles))
            {
                roles = await base.GetAllAsync();
                
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(_cacheDuration);
                
                _cache.Set(ALL_ROLES_CACHE_KEY, roles, cacheOptions);
            }
            
            return roles;
        }

        public override async Task<Role?> GetByIdAsync(int id)
        {
            string cacheKey = string.Format(ROLE_BY_ID_CACHE_KEY, id);
            
            if (!_cache.TryGetValue(cacheKey, out Role? role))
            {
                role = await base.GetByIdAsync(id);
                
                if (role != null)
                {
                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(_cacheDuration);
                    
                    _cache.Set(cacheKey, role, cacheOptions);
                }
            }
            
            return role;
        }

        public override async Task<Role?> GetByNameAsync(string name)
        {
            string cacheKey = string.Format(ROLE_BY_NAME_CACHE_KEY, name);
            
            if (!_cache.TryGetValue(cacheKey, out Role? role))
            {
                role = await base.GetByNameAsync(name);
                
                if (role != null)
                {
                    var cacheOptions = new MemoryCacheEntryOptions()
                        .SetAbsoluteExpiration(_cacheDuration);
                    
                    _cache.Set(cacheKey, role, cacheOptions);
                }
            }
            
            return role;
        }

        public override async Task<IEnumerable<Role>> GetRolesWithUsersAsync()
        {
            // Bu metot için önbellek kullanmıyoruz çünkü kullanıcı bilgileri sık değişebilir
            return await base.GetRolesWithUsersAsync();
        }

        public override async Task AddAsync(Role entity)
        {
            await base.AddAsync(entity);
            InvalidateCache();
        }

        public override async Task UpdateAsync(Role entity)
        {
            await base.UpdateAsync(entity);
            InvalidateCache();
        }

        public override async Task DeleteAsync(Role entity)
        {
            await base.DeleteAsync(entity);
            InvalidateCache();
        }

        private void InvalidateCache()
        {
            _cache.Remove(ALL_ROLES_CACHE_KEY);
        }
    }
} 