using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SalesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SalesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Creates a new sale.
    /// </summary>
    /// <param name="request">The sale creation request.</param>
    /// <returns>The created sale details.</returns>
    [HttpPost]
    [ProducesResponseType(typeof(CreateSaleResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleRequest request)
    {
        var command = new CreateSaleCommand
        {
            CustomerId = request.CustomerId,
            BranchId = request.BranchId,
            Items = request.Items.Select(item => new CreateSaleItemCommand
            {
                ItemId = item.ItemId,
                Quantity = item.Quantity
            }).ToList()
        };

        var result = await _mediator.Send(command);

        var response = new CreateSaleResponse
        {
            Id = result.Id,
            CustomerId = result.CustomerId,
            BranchId = result.BranchId,
            CreatedAt = result.CreatedAt
        };

        return CreatedAtAction(nameof(CreateSale), new { id = response.Id }, response);
    }
} 