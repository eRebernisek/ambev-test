namespace Ambev.DeveloperEvaluation.Domain.ValueObjects
{
    public class CustomerInfo
    {
        public string? Id { get; private set; }
        public string? Name { get; private set; }
        public string? Email { get; private set; }

        protected CustomerInfo() { }

        public CustomerInfo(string id, string name, string email)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Email = email ?? throw new ArgumentNullException(nameof(email));
        }
    }
} 