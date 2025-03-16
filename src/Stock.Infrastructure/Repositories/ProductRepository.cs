using Microsoft.EntityFrameworkCore;
using Stock.Domain.Entities;
using Stock.Domain.Interfaces;
using Stock.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stock.Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Product> GetByCodeAsync(string code)
        {
            return await _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Code == code && !p.IsDeleted);
        }

        public async Task<IEnumerable<Product>> GetByCategoryIdAsync(int categoryId)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.CategoryId == categoryId && !p.IsDeleted)
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.StockQuantity <= threshold && !p.IsDeleted)
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<Product> GetWithVariantsAsync(int id)
        {
            return await _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Variants)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        }

        public async Task<IEnumerable<Product>> GetActiveProductsAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.IsActive && !p.IsDeleted)
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryIdAsync(int categoryId)
        {
            return await _context.Products
                .Where(p => p.CategoryId == categoryId && !p.IsDeleted)
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<(IEnumerable<Product> Products, int TotalCount)> GetPaginatedProductsAsync(
            int pageNumber,
            int pageSize,
            string nameFilter = null,
            string codeFilter = null,
            int? categoryIdFilter = null,
            bool? isActiveFilter = null,
            string sortBy = "Name",
            bool sortAscending = true)
        {
            var query = _context.Products.AsQueryable();

            // Filtreleri uygula
            if (!string.IsNullOrEmpty(nameFilter))
            {
                query = query.Where(p => p.Name.Contains(nameFilter));
            }

            if (!string.IsNullOrEmpty(codeFilter))
            {
                query = query.Where(p => p.Code.Contains(codeFilter));
            }

            if (categoryIdFilter.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryIdFilter.Value);
            }

            if (isActiveFilter.HasValue)
            {
                query = query.Where(p => p.IsActive == isActiveFilter.Value);
            }

            // Silinen ürünleri hariç tut
            query = query.Where(p => !p.IsDeleted);

            // Toplam kayıt sayısını al
            var totalCount = await query.CountAsync();

            // Sıralama uygula
            query = ApplySorting(query, sortBy, sortAscending);

            // Sayfalama uygula
            var products = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(p => p.Category)
                .ToListAsync();

            return (products, totalCount);
        }

        private IQueryable<Product> ApplySorting(IQueryable<Product> query, string sortBy, bool ascending)
        {
            return sortBy.ToLower() switch
            {
                "name" => ascending
                    ? query.OrderBy(p => p.Name)
                    : query.OrderByDescending(p => p.Name),
                "code" => ascending
                    ? query.OrderBy(p => p.Code)
                    : query.OrderByDescending(p => p.Code),
                "price" => ascending
                    ? query.OrderBy(p => p.UnitPrice)
                    : query.OrderByDescending(p => p.UnitPrice),
                "stock" => ascending
                    ? query.OrderBy(p => p.StockQuantity)
                    : query.OrderByDescending(p => p.StockQuantity),
                "category" => ascending
                    ? query.OrderBy(p => p.Category.Name)
                    : query.OrderByDescending(p => p.Category.Name),
                "createdat" => ascending
                    ? query.OrderBy(p => p.CreatedAt)
                    : query.OrderByDescending(p => p.CreatedAt),
                _ => ascending
                    ? query.OrderBy(p => p.Name)
                    : query.OrderByDescending(p => p.Name)
            };
        }
    }
} 