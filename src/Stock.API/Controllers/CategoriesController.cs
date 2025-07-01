
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
    /// Manages category-related operations. Allows for listing, retrieving, creating, updating, and deleting categories.
    /// All operations are handled via the CQRS pattern using MediatR.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize] // Uncomment to enforce authorization for the entire controller
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoriesController"/> class.
        /// </summary>
        /// <param name="mediator">The MediatR instance for handling CQRS commands and queries.</param>
        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves a list of all categories.
        /// </summary>
        /// <returns>A list of all categories.</returns>
        /// <response code="200">Returns the list of categories.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CategoryDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategories()
        {
            var query = new GetAllCategoriesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific category by its unique ID.
        /// </summary>
        /// <param name="id">The ID of the category to retrieve.</param>
        /// <returns>The details of the specified category.</returns>
        /// <response code="200">Returns the category details.</response>
        /// <response code="404">If a category with the specified ID is not found.</response>
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
        /// Creates a new category.
        /// </summary>
        /// <param name="command">The command containing the details for the new category.</param>
        /// <returns>The newly created category.</returns>
        /// <response code="201">Returns the newly created category.</response>
        /// <response code="400">If the command contains invalid data (e.g., duplicate name).</response>
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
        /// Updates an existing category.
        /// </summary>
        /// <param name="id">The ID of the category to update.</param>
        /// <param name="command">The command containing the updated category data.</param>
        /// <returns>A success response if the update is successful.</returns>
        /// <response code="204">If the category was successfully updated.</response>
        /// <response code="400">If the command contains invalid data or the ID in the route does not match the ID in the body.</response>
        /// <response code="404">If a category with the specified ID is not found.</response>
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
        /// Deletes a category by its unique ID (soft delete).
        /// </summary>
        /// <param name="id">The ID of the category to delete.</param>
        /// <returns>A success response if the deletion is successful.</returns>
        /// <response code="204">If the category was successfully deleted.</response>
        /// <response code="400">If the request is invalid or the category cannot be deleted.</response>
        /// <response code="404">If a category with the specified ID is not found.</response>
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