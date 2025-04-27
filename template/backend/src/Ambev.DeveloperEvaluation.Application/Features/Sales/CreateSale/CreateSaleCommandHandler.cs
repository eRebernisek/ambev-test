using System;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Features.Sales.CreateSale
{
    public class CreateSaleCommandHandler : IRequestHandler<CreateSaleCommand, Result<Guid>>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMediator _mediator;

        public CreateSaleCommandHandler(ISaleRepository saleRepository, IMediator mediator)
        {
            _saleRepository = saleRepository ?? throw new ArgumentNullException(nameof(saleRepository));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<Result<Guid>> Handle(CreateSaleCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));
            if (request.SaleNumber == null)
                throw new ArgumentNullException(nameof(request.SaleNumber));
            if (request.Customer == null)
                throw new ArgumentNullException(nameof(request.Customer));
            if (request.Branch == null)
                throw new ArgumentNullException(nameof(request.Branch));
            if (request.Items == null)
                throw new ArgumentNullException(nameof(request.Items));

            var sale = new Sale(request.SaleNumber, request.Customer, request.Branch);

            foreach (var item in request.Items)
            {
                if (item == null)
                    throw new ArgumentNullException("Item cannot be null");
                if (item.Product == null)
                    throw new ArgumentNullException("Item.Product cannot be null");

                sale.AddItem(item.Product, item.Quantity, item.UnitPrice);
            }

            await _saleRepository.AddAsync(sale);

            // Publish domain event
            await _mediator.Publish(new SaleCreatedEvent(sale.SaleNumber!, sale.SaleDate, sale.TotalAmount), cancellationToken);

            return Result<Guid>.Success(sale.Id);
        }
    }
} 