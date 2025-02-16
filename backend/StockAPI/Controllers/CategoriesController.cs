using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockAPI.Data;
using StockAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace StockAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly StockContext _context;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(StockContext context, ILogger<CategoriesController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: api/Categories
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        _logger.LogInformation("Tüm kategoriler getiriliyor");
        return await _context.Categories
            .Include(c => c.SubCategories)
            .Where(c => !c.IsDeleted)
            .ToListAsync();
    }

    // GET: api/Categories/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
        _logger.LogInformation("Kategori getiriliyor, ID: {Id}", id);
        var category = await _context.Categories
            .Include(c => c.SubCategories)
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

        if (category == null)
        {
            _logger.LogWarning("Kategori bulunamadı, ID: {Id}", id);
            return NotFound();
        }

        return category;
    }

    // POST: api/Categories
    [HttpPost]
    public async Task<ActionResult<Category>> PostCategory(Category category)
    {
        _logger.LogInformation("Yeni kategori oluşturuluyor");
        
        if (category.ParentCategoryId.HasValue)
        {
            var parentExists = await _context.Categories.AnyAsync(c => c.Id == category.ParentCategoryId && !c.IsDeleted);
            if (!parentExists)
            {
                _logger.LogWarning("Üst kategori bulunamadı, ID: {ParentId}", category.ParentCategoryId);
                return BadRequest("Belirtilen üst kategori bulunamadı.");
            }
        }

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Yeni kategori oluşturuldu, ID: {Id}", category.Id);
        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
    }

    // PUT: api/Categories/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCategory(int id, Category category)
    {
        if (id != category.Id)
        {
            _logger.LogWarning("Geçersiz kategori ID'si");
            return BadRequest();
        }

        if (category.ParentCategoryId.HasValue)
        {
            var parentExists = await _context.Categories.AnyAsync(c => c.Id == category.ParentCategoryId && !c.IsDeleted);
            if (!parentExists)
            {
                _logger.LogWarning("Üst kategori bulunamadı, ID: {ParentId}", category.ParentCategoryId);
                return BadRequest("Belirtilen üst kategori bulunamadı.");
            }
        }

        _context.Entry(category).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Kategori güncellendi, ID: {Id}", id);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CategoryExists(id))
            {
                _logger.LogWarning("Kategori bulunamadı, ID: {Id}", id);
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/Categories/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _context.Categories
            .Include(c => c.SubCategories)
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

        if (category == null)
        {
            _logger.LogWarning("Kategori bulunamadı, ID: {Id}", id);
            return NotFound();
        }

        // Soft delete
        category.IsDeleted = true;
        foreach (var subCategory in category.SubCategories)
        {
            subCategory.IsDeleted = true;
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Kategori silindi, ID: {Id}", id);

        return NoContent();
    }

    private bool CategoryExists(int id)
    {
        return _context.Categories.Any(e => e.Id == id && !e.IsDeleted);
    }
}
