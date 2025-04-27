namespace Ambev.DeveloperEvaluation.Domain.ValueObjects
{
    public class BranchInfo
    {
        public string? Id { get; private set; }
        public string? Name { get; private set; }
        public string? Address { get; private set; }

        protected BranchInfo() { }

        public BranchInfo(string id, string name, string address)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Address = address ?? throw new ArgumentNullException(nameof(address));
        }
    }
} 