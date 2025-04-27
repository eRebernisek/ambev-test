using System;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class SaleItem : Entity
    {
        public ProductInfo? Product { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitPrice { get; private set; }
        public decimal Discount { get; private set; }
        public decimal TotalAmount { get; private set; }
        public bool IsCancelled { get; private set; }

        protected SaleItem() { }

        public SaleItem(ProductInfo product, int quantity, decimal unitPrice, decimal discount)
        {
            Product = product ?? throw new ArgumentNullException(nameof(product));
            Quantity = quantity;
            UnitPrice = unitPrice;
            Discount = discount;
            IsCancelled = false;
            CalculateTotalAmount();
        }

        public void Cancel()
        {
            if (IsCancelled)
                throw new InvalidOperationException("Item is already cancelled.");

            IsCancelled = true;
            CalculateTotalAmount();
        }

        private void CalculateTotalAmount()
        {
            var subtotal = Quantity * UnitPrice;
            var discountAmount = subtotal * Discount;
            TotalAmount = subtotal - discountAmount;
        }
    }
} 