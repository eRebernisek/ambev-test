using System;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Features.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SalesController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost]
        public async Task<IActionResult> CreateSale([FromBody] CreateSaleCommand command)
        {
            if (command == null)
                return BadRequest("Command cannot be null");

            var result = await _mediator.Send(command);
            
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return CreatedAtAction(nameof(GetSale), new { id = result.Data }, result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSale(Guid id)
        {
            // TODO: Implement get sale by id
            await Task.CompletedTask;
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSales()
        {
            // TODO: Implement get all sales
            await Task.CompletedTask;
            return Ok();
        }

        [HttpPut("{id}/cancel")]
        public async Task<IActionResult> CancelSale(Guid id)
        {
            // TODO: Implement cancel sale
            await Task.CompletedTask;
            return Ok();
        }

        [HttpPut("{id}/items/{productId}/cancel")]
        public async Task<IActionResult> CancelItem(Guid id, string productId)
        {
            if (string.IsNullOrEmpty(productId))
                return BadRequest("Product ID cannot be null or empty");

            // TODO: Implement cancel item
            await Task.CompletedTask;
            return Ok();
        }
    }
} 