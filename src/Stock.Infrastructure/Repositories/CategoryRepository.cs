using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using Stock.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Infrastructure.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Category> GetWithSubCategoriesAsync(int id)
        {
            return await _context.Categories
                .AsNoTracking()
                .Include(c => c.SubCategories)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
        }

        public async Task<IEnumerable<Category>> GetParentCategoriesAsync()
        {
            return await _context.Categories
                .AsNoTracking()
                .Where(c => c.ParentCategoryId == null && !c.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetSubCategoriesAsync(int parentId)
        {
            return await _context.Categories
                .AsNoTracking()
                .Where(c => c.ParentCategoryId == parentId && !c.IsDeleted)
                .ToListAsync();
        }

        public async Task<Category> GetWithProductsAsync(int id)
        {
            return await _context.Categories
                .AsNoTracking()
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
        }

        public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
        {
            return await _context.Categories
                .AsNoTracking()
                .Where(c => c.IsActive && !c.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetCategoriesWithParentAsync()
        {
            return await _context.Categories
                .Include(c => c.ParentCategory)
                .Where(c => !c.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetTopLevelCategoriesAsync()
        {
            return await _context.Categories
                .Where(c => c.ParentCategoryId == null && !c.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetSubcategoriesAsync(int parentCategoryId)
        {
            return await _context.Categories
                .Where(c => c.ParentCategoryId == parentCategoryId && !c.IsDeleted)
                .ToListAsync();
        }

        public async Task<(IEnumerable<Category> Categories, int TotalCount)> GetPaginatedCategoriesAsync(
            int pageNumber,
            int pageSize,
            string nameFilter = null,
            int? parentCategoryIdFilter = null,
            bool? isActiveFilter = null,
            string sortBy = "Name",
            bool sortAscending = true)
        {
            var query = _context.Categories.AsQueryable();

            // Filtreleri uygula
            if (!string.IsNullOrEmpty(nameFilter))
            {
                query = query.Where(c => c.Name.Contains(nameFilter));
            }

            if (parentCategoryIdFilter.HasValue)
            {
                query = query.Where(c => c.ParentCategoryId == parentCategoryIdFilter.Value);
            }

            if (isActiveFilter.HasValue)
            {
                query = query.Where(c => c.IsActive == isActiveFilter.Value);
            }

            // Silinen kategorileri hariç tut
            query = query.Where(c => !c.IsDeleted);

            // Toplam kayıt sayısını al
            var totalCount = await query.CountAsync();

            // Sıralama uygula
            query = ApplySorting(query, sortBy, sortAscending);

            // Sayfalama uygula
            var categories = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(c => c.ParentCategory)
                .ToListAsync();

            return (categories, totalCount);
        }

        private IQueryable<Category> ApplySorting(IQueryable<Category> query, string sortBy, bool ascending)
        {
            return sortBy.ToLower() switch
            {
                "name" => ascending
                    ? query.OrderBy(c => c.Name)
                    : query.OrderByDescending(c => c.Name),
                "description" => ascending
                    ? query.OrderBy(c => c.Description)
                    : query.OrderByDescending(c => c.Description),
                "parentcategory" => ascending
                    ? query.OrderBy(c => c.ParentCategory.Name)
                    : query.OrderByDescending(c => c.ParentCategory.Name),
                "createdat" => ascending
                    ? query.OrderBy(c => c.CreatedAt)
                    : query.OrderByDescending(c => c.CreatedAt),
                _ => ascending
                    ? query.OrderBy(c => c.Name)
                    : query.OrderByDescending(c => c.Name)
            };
        }
    }
} 