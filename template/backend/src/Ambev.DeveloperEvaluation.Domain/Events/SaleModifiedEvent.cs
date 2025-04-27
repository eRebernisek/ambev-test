using System;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleModifiedEvent : DomainEvent
    {
        public string SaleNumber { get; }
        public decimal NewTotalAmount { get; }

        public SaleModifiedEvent(string saleNumber, decimal newTotalAmount)
        {
            SaleNumber = saleNumber;
            NewTotalAmount = newTotalAmount;
        }
    }
} 