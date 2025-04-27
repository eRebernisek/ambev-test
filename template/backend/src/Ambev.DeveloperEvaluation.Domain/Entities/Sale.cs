using System;
using System.Collections.Generic;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.ValueObjects;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale : Entity
    {
        public string? SaleNumber { get; private set; }
        public DateTime SaleDate { get; private set; }
        public CustomerInfo? Customer { get; private set; }
        public decimal TotalAmount { get; private set; }
        public BranchInfo? Branch { get; private set; }
        public bool IsCancelled { get; private set; }
        private readonly List<SaleItem> _items = new();
        public IReadOnlyCollection<SaleItem> Items => _items.AsReadOnly();

        protected Sale() { }

        public Sale(string saleNumber, CustomerInfo customer, BranchInfo branch)
        {
            SaleNumber = saleNumber ?? throw new ArgumentNullException(nameof(saleNumber));
            SaleDate = DateTime.UtcNow;
            Customer = customer ?? throw new ArgumentNullException(nameof(customer));
            Branch = branch ?? throw new ArgumentNullException(nameof(branch));
            IsCancelled = false;
        }

        public void AddItem(ProductInfo product, int quantity, decimal unitPrice)
        {
            if (quantity > 20)
                throw new InvalidOperationException("Cannot add more than 20 items of the same product.");

            var discount = CalculateDiscount(quantity);
            var item = new SaleItem(product, quantity, unitPrice, discount);
            _items.Add(item);
            UpdateTotalAmount();
        }

        public void Cancel()
        {
            if (IsCancelled)
                throw new InvalidOperationException("Sale is already cancelled.");

            IsCancelled = true;
        }

        public void CancelItem(ProductInfo product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
            if (product.Id == null)
                throw new ArgumentNullException(nameof(product.Id));

            var item = _items.FirstOrDefault(i => i.Product?.Id == product.Id);
            if (item == null)
                throw new InvalidOperationException("Item not found in sale.");

            item.Cancel();
            UpdateTotalAmount();
        }

        private decimal CalculateDiscount(int quantity)
        {
            if (quantity < 4)
                return 0;

            if (quantity >= 10 && quantity <= 20)
                return 0.20m;

            if (quantity >= 4)
                return 0.10m;

            return 0;
        }

        private void UpdateTotalAmount()
        {
            TotalAmount = _items.Sum(item => item.TotalAmount);
        }
    }
} 