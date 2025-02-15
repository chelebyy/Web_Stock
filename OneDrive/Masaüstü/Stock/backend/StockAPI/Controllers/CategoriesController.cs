using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockAPI.Data;
using StockAPI.Models;

namespace StockAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(ApplicationDbContext context, ILogger<CategoriesController> logger)
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
    public async Task<ActionResult<Category>> CreateCategory(Category category)
    {
        _logger.LogInformation("Yeni kategori oluşturuluyor");
        category.CreatedAt = DateTime.UtcNow;
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
    }

    // PUT: api/Categories/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(int id, Category category)
    {
        if (id != category.Id)
        {
            return BadRequest();
        }

        _logger.LogInformation("Kategori güncelleniyor, ID: {Id}", id);
        var existingCategory = await _context.Categories.FindAsync(id);
        
        if (existingCategory == null || existingCategory.IsDeleted)
        {
            _logger.LogWarning("Güncellenecek kategori bulunamadı, ID: {Id}", id);
            return NotFound();
        }

        existingCategory.Name = category.Name;
        existingCategory.Description = category.Description;
        existingCategory.ParentCategoryId = category.ParentCategoryId;
        existingCategory.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CategoryExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    // DELETE: api/Categories/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        _logger.LogInformation("Kategori siliniyor, ID: {Id}", id);
        var category = await _context.Categories.FindAsync(id);
        
        if (category == null || category.IsDeleted)
        {
            _logger.LogWarning("Silinecek kategori bulunamadı, ID: {Id}", id);
            return NotFound();
        }

        category.IsDeleted = true;
        category.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CategoryExists(int id)
    {
        return _context.Categories.Any(e => e.Id == id && !e.IsDeleted);
    }
}
