using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StockAPI.Data;
using StockAPI.Models;

namespace StockAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(ApplicationDbContext context, ILogger<ProductsController> logger)
    {
        _context = context;
        _logger = logger;
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
        if (id != product.Id)
        {
            return BadRequest();
        }

        _logger.LogInformation("Ürün güncelleniyor, ID: {Id}", id);
        
        if (!await _context.Categories.AnyAsync(c => c.Id == product.CategoryId && !c.IsDeleted))
        {
            _logger.LogWarning("Geçersiz kategori ID: {CategoryId}", product.CategoryId);
            return BadRequest("Geçersiz kategori ID");
        }

        var existingProduct = await _context.Products.FindAsync(id);
        
        if (existingProduct == null || existingProduct.IsDeleted)
        {
            _logger.LogWarning("Güncellenecek ürün bulunamadı, ID: {Id}", id);
            return NotFound();
        }

        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;
        existingProduct.StockQuantity = product.StockQuantity;
        existingProduct.SKU = product.SKU;
        existingProduct.Barcode = product.Barcode;
        existingProduct.CategoryId = product.CategoryId;
        existingProduct.UpdatedAt = DateTime.UtcNow;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ProductExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
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

    private bool ProductExists(int id)
    {
        return _context.Products.Any(e => e.Id == id && !e.IsDeleted);
    }
}
