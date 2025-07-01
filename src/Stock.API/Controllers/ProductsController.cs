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
using Stock.Application.Features.Products.Queries.GetProductsSummary;

using Stock.Domain.Common; // Result türü için eklendi

namespace Stock.API.Controllers
{
    /// <summary>
    /// Manages product-related operations such as listing, retrieving, creating, updating, and deleting products.
    /// All operations are handled via the CQRS pattern using MediatR.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    // [Authorize] // Uncomment to enforce authorization for the entire controller
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductsController"/> class.
        /// </summary>
        /// <param name="mediator">The MediatR instance for handling CQRS commands and queries.</param>
        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves a paginated list of all products.
        /// </summary>
        /// <remarks>
        /// Supports filtering by name and category, as well as sorting and pagination.
        /// Example: GET /api/v1/products?pageNumber=1&amp;pageSize=20&amp;sortBy=Name&amp;sortDirection=asc&amp;nameFilter=laptop&amp;categoryId=1
        /// </remarks>
        /// <param name="pageNumber">The page number to retrieve (default: 1).</param>
        /// <param name="pageSize">The number of items per page (default: 10, max: 100).</param>
        /// <param name="sortBy">The field to sort by (e.g., Name, CategoryName).</param>
        /// <param name="sortDirection">The sort direction ('asc' or 'desc').</param>
        /// <param name="nameFilter">A filter to apply to the product name.</param>
        /// <param name="categoryId">A filter to retrieve products by a specific category ID.</param>
        /// <response code="200">Returns the paginated list of products.</response>
        /// <response code="400">If the query parameters are invalid.</response>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResponse<ProductListItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PagedResponse<ProductListItemDto>>> GetAllProducts(
            [FromQuery] int? pageNumber = null,
            [FromQuery] int? pageSize = null,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? sortDirection = null,
            [FromQuery] string? nameFilter = null,
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
        /// Gets a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to retrieve.</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/products/1
        ///
        /// </remarks>
        /// <response code="200">Returns the product with the specified ID.</response>
        /// <response code="404">If the product with the specified ID was not found.</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDto>> GetById(int id)
        {
            var query = new GetProductByIdQuery(id);
            var result = await _mediator.Send(query);
            return result != null ? Ok(result) : NotFound();
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="command">The command to create a new product.</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/products
        ///     {
        ///        "name": "New Product",
        ///        "price": 10.0,
        ///        "categoryId": 1
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns the newly created product.</response>
        /// <response code="400">If the command is invalid.</response>
        [HttpPost]
        //[Authorize(Policy = "RequireAdminRole")]
        [ProducesResponseType(typeof(ProductDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductDto>> Create(CreateProductCommand command)
        {
            Result<ProductDto> result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Error);
            }

            var productDto = result.Value;
            return CreatedAtAction(nameof(GetById), new { id = productDto.Id }, productDto);
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="id">The ID of the product to update.</param>
        /// <param name="command">The command to update the product.</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/products/1
        ///     {
        ///        "name": "Updated Product",
        ///        "price": 15.0,
        ///        "categoryId": 1
        ///     }
        ///
        /// </remarks>
        /// <response code="204">If the product was successfully updated.</response>
        /// <response code="400">If the command is invalid or the ID does not match.</response>
        /// <response code="404">If the product with the specified ID was not found.</response>
        [HttpPut("{id:int}")]
        //[Authorize(Policy = "RequireAdminRole")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, UpdateProductCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/products/1
        ///
        /// </remarks>
        /// <response code="204">If the product was successfully deleted.</response>
        /// <response code="404">If the product with the specified ID was not found.</response>
        [HttpDelete("{id:int}")]
        //[Authorize(Policy = "RequireAdminRole")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteProductCommand(id));
            return NoContent();
        }

        /// <summary>
        /// Retrieves a paginated summary list of products.
        /// </summary>
        /// <remarks>
        /// This endpoint returns a lightweight version of product data, suitable for quick overviews or lists.
        /// </remarks>
        /// <param name="query">The query parameters for pagination and filtering.</param>
        /// <response code="200">Returns the paginated list of product summaries.</response>
        /// <response code="400">If the query parameters are invalid.</response>
        [HttpGet("summary")]
        [ProducesResponseType(typeof(PagedResponse<ProductSummaryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetSummary([FromQuery] GetProductsSummaryQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
} 