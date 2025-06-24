using MediatR;
using Microsoft.AspNetCore.Mvc;
using Stock.Application.Features.Categories.Commands;
using Stock.Application.Features.Categories.DTOs;
using Stock.Application.Features.Categories.Queries;
using Stock.Domain.Common; // Result için
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization; // Yetkilendirme için

namespace Stock.API.Controllers
{
    /// <summary>
    /// Kategori yönetimi için API controller sınıfı.
    /// Kategorileri listeleme, detaylarını görme, oluşturma, güncelleme ve silme işlemlerini sağlar.
    /// Tüm işlemler CQRS pattern kullanılarak MediatR üzerinden gerçekleştirilir.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize] // Gerekirse yetkilendirme ekle
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// CategoriesController yapıcı metodu.
        /// MediatR bağımlılığını enjekte eder.
        /// </summary>
        /// <param name="mediator">CQRS komutlarını ve sorgularını işleyecek olan MediatR nesnesi.</param>
        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Tüm kategorileri listeler.
        /// </summary>
        /// <returns>Kategorilerin listesini içeren bir ActionResult.</returns>
        /// <response code="200">Kategoriler başarıyla getirildi.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategories()
        {
            var query = new GetAllCategoriesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Belirtilen ID'ye sahip kategoriyi getirir.
        /// </summary>
        /// <param name="id">Getirilecek kategorinin ID'si.</param>
        /// <returns>Bulunan kategoriyi içeren bir ActionResult.</returns>
        /// <response code="200">Kategori başarıyla getirildi.</response>
        /// <response code="404">Belirtilen ID'ye sahip kategori bulunamadı.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CategoryDto>> GetCategoryById(int id)
        {
            var query = new GetCategoryByIdQuery(id);
            var result = await _mediator.Send(query);
            return result != null ? Ok(result) : NotFound();
        }

        /// <summary>
        /// Yeni bir kategori oluşturur.
        /// </summary>
        /// <param name="command">Oluşturulacak kategorinin bilgilerini içeren komut.</param>
        /// <returns>Oluşturulan kategoriyi içeren bir ActionResult.</returns>
        /// <response code="201">Kategori başarıyla oluşturuldu.</response>
        /// <response code="400">Geçersiz kategori bilgileri.</response>
        [HttpPost]
        [ProducesResponseType(typeof(CategoryDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CreateCategoryCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                // Başarılı ise 201 Created yanıtı ve oluşturulan kaynağın DTO'su
                // GetCategoryById action'ına referansla
                return CreatedAtAction(nameof(GetCategoryById), new { id = result.Value.Id }, result.Value);
            }
            else
            {
                // Başarısız ise 400 Bad Request ve hata mesajı
                return BadRequest(result.Error); 
            }
        }

        /// <summary>
        /// Mevcut bir kategoriyi günceller.
        /// </summary>
        /// <param name="id">Güncellenecek kategorinin ID'si.</param>
        /// <param name="command">Güncellenmiş kategori bilgilerini içeren komut.</param>
        /// <returns>İşlem sonucunu belirten bir IActionResult.</returns>
        /// <response code="204">Kategori başarıyla güncellendi.</response>
        /// <response code="400">Geçersiz kategori bilgileri veya ID uyuşmazlığı.</response>
        /// <response code="404">Belirtilen ID'ye sahip kategori bulunamadı.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)] 
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryCommand command)
        {
            // Gelen ID ile komuttaki ID'nin eşleştiğini kontrol et
            if (id != command.Id)
            {
                return BadRequest("ID mismatch in request URL and body.");
            }

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return NoContent(); // Başarılı ise 204 No Content
            }
            else
            {
                // Hata mesajına göre 404 veya 400 döndür
                // Basit bir kontrol, daha iyisi result.Error'ın türüne bakmak olabilir
                if (result.Error.Contains("not found", StringComparison.OrdinalIgnoreCase))
                    return NotFound(result.Error);
                else
                    return BadRequest(result.Error);
            }
        }

        /// <summary>
        /// Belirtilen ID'ye sahip kategoriyi siler (Soft delete).
        /// </summary>
        /// <param name="id">Silinecek kategorinin ID'si.</param>
        /// <returns>İşlem sonucunu belirten bir IActionResult.</returns>
        /// <response code="204">Kategori başarıyla silindi.</response>
        /// <response code="400">Geçersiz istek veya kategori silinemedi.</response>
        /// <response code="404">Belirtilen ID'ye sahip kategori bulunamadı.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)] 
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var command = new DeleteCategoryCommand(id);
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return NoContent(); // Başarılı ise 204 No Content
            }
            else
            {
                // Hata mesajına göre 404 veya 400 döndür
                 if (result.Error.Contains("not found", StringComparison.OrdinalIgnoreCase))
                    return NotFound(result.Error);
                else
                    return BadRequest(result.Error);
            }
        }
    }
} 