using System;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class ItemCancelledEvent : DomainEvent
    {
        public string SaleNumber { get; }
        public string ProductId { get; }
        public DateTime CancellationDate { get; }

        public ItemCancelledEvent(string saleNumber, string productId)
        {
            SaleNumber = saleNumber;
            ProductId = productId;
            CancellationDate = DateTime.UtcNow;
        }
    }
} 