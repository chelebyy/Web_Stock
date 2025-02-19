using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockAPI.Data;
using StockAPI.Models;
using StockAPI.Services;

namespace StockAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IAuditLogService _auditLogService;
    private readonly ILogger<ProductsController> _logger;

    private static readonly Action<ILogger, string, Exception?> _logProductUpdateError =
        LoggerMessage.Define<string>(
            LogLevel.Error,
            new EventId(1, nameof(UpdateProduct)),
            "Ürün güncellenirken hata oluştu - Name: {ProductName}");

    public ProductsController(
        ApplicationDbContext context,
        IAuditLogService auditLogService,
        ILogger<ProductsController> logger)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _auditLogService = auditLogService ?? throw new ArgumentNullException(nameof(auditLogService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    // GET: api/Products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        _logger.LogInformation("Tüm ürünler getiriliyor");
        return await _context.Products
            .Include(p => p.Category)
            .Where(p => !p.IsDeleted)
            .ToListAsync();
    }

    // GET: api/Products/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        _logger.LogInformation("Ürün getiriliyor, ID: {Id}", id);
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

        if (product == null)
        {
            _logger.LogWarning("Ürün bulunamadı, ID: {Id}", id);
            return NotFound();
        }

        return product;
    }

    // POST: api/Products
    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        _logger.LogInformation("Yeni ürün oluşturuluyor");
        
        if (!await _context.Categories.AnyAsync(c => c.Id == product.CategoryId && !c.IsDeleted))
        {
            _logger.LogWarning("Geçersiz kategori ID: {CategoryId}", product.CategoryId);
            return BadRequest("Geçersiz kategori ID");
        }

        product.CreatedAt = DateTime.UtcNow;
        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    // PUT: api/Products/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        if (id != product.Id)
        {
            return BadRequest();
        }

        try
        {
            var existingProduct = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id)
                .ConfigureAwait(false);

            if (existingProduct == null)
            {
                return NotFound();
            }

            _context.Entry(product).State = EntityState.Modified;
            await _context.SaveChangesAsync().ConfigureAwait(false);

            return NoContent();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            if (!await ProductExists(id).ConfigureAwait(false))
            {
                return NotFound();
            }

            _logProductUpdateError(_logger, product.Name, ex);
            throw;
        }
    }

    // DELETE: api/Products/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        _logger.LogInformation("Ürün siliniyor, ID: {Id}", id);
        var product = await _context.Products.FindAsync(id);
        
        if (product == null || product.IsDeleted)
        {
            _logger.LogWarning("Silinecek ürün bulunamadı, ID: {Id}", id);
            return NotFound();
        }

        product.IsDeleted = true;
        product.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> ProductExists(int id)
    {
        return await _context.Products.AnyAsync(e => e.Id == id).ConfigureAwait(false);
    }
}
