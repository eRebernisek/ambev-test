using System;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleCancelledEvent : DomainEvent
    {
        public string SaleNumber { get; }
        public DateTime CancellationDate { get; }

        public SaleCancelledEvent(string saleNumber)
        {
            SaleNumber = saleNumber;
            CancellationDate = DateTime.UtcNow;
        }
    }
} 