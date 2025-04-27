using System;
using System.Collections.Generic;
using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Features.Sales.CreateSale
{
    public class CreateSaleCommand : IRequest<Result<Guid>>
    {
        public string? SaleNumber { get; set; }
        public CustomerInfo? Customer { get; set; }
        public BranchInfo? Branch { get; set; }
        public List<SaleItemDto>? Items { get; set; }

        public class SaleItemDto
        {
            public ProductInfo? Product { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
        }
    }
} 