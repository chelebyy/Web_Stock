using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockAPI.Data;
using StockAPI.Models;
using Microsoft.AspNetCore.Authorization;
using StockAPI.Services;

namespace StockAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<CategoriesController> _logger;

    private static readonly Action<ILogger, string, Exception?> _logCategoryUpdateError =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(1, nameof(PutCategory)),
            "Kategori güncellenirken hata oluştu - Name: {CategoryName}");

    private static readonly Action<ILogger, string, Exception?> _logCategoryCreateError =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(2, nameof(PostCategory)),
            "Kategori oluşturulurken hata oluştu - Name: {CategoryName}");

    public CategoriesController(
        ApplicationDbContext context,
        IAuditLogService auditLogService,
        ILogger<CategoriesController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _auditLogService = auditLogService ?? throw new ArgumentNullException(nameof(auditLogService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
        ArgumentNullException.ThrowIfNull(category);

        try
        {
            await _context.Categories.AddAsync(category).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }
        catch (DbUpdateException ex)
        {
            _logCategoryCreateError(_logger, category.Name, ex);
            return StatusCode(500, new { message = "Kategori oluşturulurken bir hata oluştu" });
        }
    }

    // PUT: api/Categories/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCategory(int id, Category category)
    {
        ArgumentNullException.ThrowIfNull(category);

        if (id != category.Id)
        {
            return BadRequest();
        }

        try
        {
            var existingCategory = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id)
                .ConfigureAwait(false);

            if (existingCategory == null)
            {
                return NotFound();
            }

            _context.Entry(category).State = EntityState.Modified;
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return NoContent();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if (!await CategoryExists(id).ConfigureAwait(false))
            {
                return NotFound();
            }

            _logCategoryUpdateError(_logger, category.Name, ex);
            throw;
        }
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

    private async Task<bool> CategoryExists(int id)
    {
        return await _context.Categories.AnyAsync(e => e.Id == id).ConfigureAwait(false);
    }
}
