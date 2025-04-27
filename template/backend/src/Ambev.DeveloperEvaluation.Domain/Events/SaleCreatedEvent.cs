using System;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    public class SaleCreatedEvent : DomainEvent
    {
        public string SaleNumber { get; }
        public DateTime SaleDate { get; }
        public decimal TotalAmount { get; }

        public SaleCreatedEvent(string saleNumber, DateTime saleDate, decimal totalAmount)
        {
            SaleNumber = saleNumber;
            SaleDate = saleDate;
            TotalAmount = totalAmount;
        }
    }
} 