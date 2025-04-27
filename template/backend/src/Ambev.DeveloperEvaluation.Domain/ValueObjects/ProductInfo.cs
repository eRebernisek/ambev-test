namespace Ambev.DeveloperEvaluation.Domain.ValueObjects
{
    public class ProductInfo
    {
        public string? Id { get; private set; }
        public string? Name { get; private set; }
        public string? Description { get; private set; }

        protected ProductInfo() { }

        public ProductInfo(string id, string name, string description)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? throw new ArgumentNullException(nameof(description));
        }
    }
} 