using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale;

/// <summary>
/// Endpoint for creating a new sale
/// </summary>
[ApiController]
[Route("api/sales")]
[Produces("application/json")]
public class CreateSaleEndpoint : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the CreateSaleEndpoint class
    /// </summary>
    public CreateSaleEndpoint(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Creates a new sale
    /// </summary>
    /// <param name="command">The sale creation command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The created sale details</returns>
    /// <response code="201">Returns the created sale</response>
    /// <response code="400">If the command is invalid</response>
    /// <response code="404">If any referenced item, customer, or branch is not found</response>
    /// <response code="409">If there is insufficient stock for any item</response>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(CreateSaleResult), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Create([FromBody] CreateSaleCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(
                nameof(Create),
                new { id = result.Id },
                result);
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("not found"))
        {
            return NotFound(new ProblemDetails
            {
                Title = "Not Found",
                Detail = ex.Message,
                Status = StatusCodes.Status404NotFound
            });
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("Insufficient stock"))
        {
            return Conflict(new ProblemDetails
            {
                Title = "Insufficient Stock",
                Detail = ex.Message,
                Status = StatusCodes.Status409Conflict
            });
        }
    }
} 