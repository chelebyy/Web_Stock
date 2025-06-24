using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stock.Application.Features.Products.Commands.CreateProduct;
using Stock.Application.Features.Products.Commands.DeleteProduct;
using Stock.Application.Features.Products.Commands.UpdateProduct;
using Stock.Application.Features.Products.DTOs;
using Stock.Application.Features.Products.Queries.GetAllProducts;
using Stock.Application.Features.Products.Queries.GetProductById;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; // Yetkilendirme için
using Microsoft.AspNetCore.Http; // StatusCodes için eklendi
using System;
using Stock.Application.Constants; // ApiConstants için eklendi
using Stock.Application.Models; // PagedResponse için eklendi

namespace Stock.API.Controllers
{
    /// <summary>
    /// Ürün yönetimi için API controller sınıfı.
    /// Ürünleri listeleme, detaylarını görme, oluşturma, güncelleme ve silme işlemlerini sağlar.
    /// Tüm işlemler CQRS pattern kullanılarak MediatR üzerinden gerçekleştirilir.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize] // Bu controller'a erişim için yetkilendirme gerekliyse açılabilir
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// ProductsController yapıcı metodu.
        /// MediatR bağımlılığını enjekte eder.
        /// </summary>
        /// <param name="mediator">CQRS komutlarını ve sorgularını işleyecek olan MediatR nesnesi.</param>
        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Tüm ürünleri listeler.
        /// Filtre, sıralama ve sayfalama parametreleri destekler.
        /// </summary>
        /// <param name="pageNumber">Sayfa numarası (varsayılan: 1)</param>
        /// <param name="pageSize">Sayfa başına kayıt sayısı (varsayılan: 10, max: 100)</param>
        /// <param name="sortBy">Sıralama alanı (Name, StockLevel, CategoryName vb.)</param>
        /// <param name="sortDirection">Sıralama yönü (asc/desc)</param>
        /// <param name="nameFilter">Ürün adına göre filtreleme</param>
        /// <param name="categoryId">Kategori ID'sine göre filtreleme</param>
        /// <returns>Sayfalanmış ürünlerin listesini içeren bir ActionResult.</returns>
        /// <response code="200">Ürünler başarıyla getirildi.</response>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<ProductListItemDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PagedResponse<ProductListItemDto>>> GetAllProducts(
            [FromQuery] int? pageNumber = null,
            [FromQuery] int? pageSize = null,
            [FromQuery] string sortBy = null,
            [FromQuery] string sortDirection = null,
            [FromQuery] string nameFilter = null,
            [FromQuery] int? categoryId = null)
        {
            var query = new GetAllProductsQuery
            {
                PageNumber = pageNumber ?? ApiConstants.DefaultPageNumber,
                PageSize = Math.Min(pageSize ?? ApiConstants.DefaultPageSize, ApiConstants.MaxPageSize),
                SortBy = string.IsNullOrEmpty(sortBy) ? "Name" : sortBy,
                SortDirection = string.IsNullOrEmpty(sortDirection) ? "asc" : sortDirection.ToLower(),
                NameFilter = nameFilter,
                CategoryId = categoryId
            };

            var products = await _mediator.Send(query);
            return Ok(products);
        }

        /// <summary>
        /// Belirtilen ID'ye sahip ürünü getirir.
        /// </summary>
        /// <param name="id">Getirilecek ürünün ID'si.</param>
        /// <returns>Bulunan ürünü içeren bir ActionResult.</returns>
        /// <response code="200">Ürün başarıyla getirildi.</response>
        /// <response code="404">Belirtilen ID'ye sahip ürün bulunamadı.</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            var query = new GetProductByIdQuery(id);
            var product = await _mediator.Send(query);
            // Handler'dan gelen null durumu kontrol edilmeli veya handler içinde NotFoundException fırlatılmalı.
            // Şimdilik doğrudan Ok dönüyoruz, global exception handler 404'ü yakalayacaktır.
            return Ok(product);
        }

        /// <summary>
        /// Yeni bir ürün oluşturur.
        /// </summary>
        /// <param name="command">Oluşturulacak ürünün bilgilerini içeren komut.</param>
        /// <returns>Oluşturulan ürünü içeren bir ActionResult.</returns>
        /// <response code="201">Ürün başarıyla oluşturuldu.</response>
        /// <response code="400">Geçersiz ürün bilgileri.</response>
        [HttpPost]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductCommand command)
        {
            var product = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        /// <summary>
        /// Mevcut bir ürünü günceller.
        /// </summary>
        /// <param name="id">Güncellenecek ürünün ID'si.</param>
        /// <param name="command">Güncellenmiş ürün bilgilerini içeren komut.</param>
        /// <returns>İşlem sonucunu belirten bir IActionResult.</returns>
        /// <response code="204">Ürün başarıyla güncellendi.</response>
        /// <response code="400">Geçersiz ürün bilgileri veya ID uyuşmazlığı.</response>
        /// <response code="404">Belirtilen ID'ye sahip ürün bulunamadı.</response>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductCommand command)
        {
            // Route ID ile komut içindeki ID'nin eşleştiğini kontrol etmek iyi bir pratiktir.
            if (id != command.Id)
            {
                return BadRequest("ID mismatch between route parameter and command body.");
            }
            await _mediator.Send(command);
            return NoContent(); // Başarılı güncelleme sonrası 204
        }

        /// <summary>
        /// Belirtilen ID'ye sahip ürünü siler (Soft delete).
        /// </summary>
        /// <param name="id">Silinecek ürünün ID'si.</param>
        /// <returns>İşlem sonucunu belirten bir IActionResult.</returns>
        /// <response code="204">Ürün başarıyla silindi.</response>
        /// <response code="404">Belirtilen ID'ye sahip ürün bulunamadı.</response>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var command = new DeleteProductCommand(id);
            await _mediator.Send(command);
            return NoContent(); // Başarılı silme sonrası 204
        }
    }
} 