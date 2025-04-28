using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;

/// <summary>
/// Endpoint for retrieving sale details
/// </summary>
[ApiController]
[Route("api/sales")]
[Produces("application/json")]
public class GetSaleEndpoint : ControllerBase
{
    private readonly ISaleRepository _saleRepository;

    /// <summary>
    /// Initializes a new instance of the GetSaleEndpoint class
    /// </summary>
    public GetSaleEndpoint(ISaleRepository saleRepository)
    {
        _saleRepository = saleRepository;
    }

    /// <summary>
    /// Gets a sale by its ID
    /// </summary>
    /// <param name="id">The sale ID</param>
    /// <returns>The sale details</returns>
    /// <response code="200">Returns the sale details</response>
    /// <response code="404">If the sale is not found</response>
    [HttpGet("{id:guid}", Name = "GetSale")]
    [Authorize]
    [ProducesResponseType(typeof(Sale), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromRoute] Guid id)
    {
        var sale = await _saleRepository.GetByIdAsync(id);
        if (sale == null)
        {
            return NotFound(new ProblemDetails
            {
                Title = "Not Found",
                Detail = $"Sale with ID {id} not found",
                Status = StatusCodes.Status404NotFound
            });
        }

        return Ok(sale);
    }
} 