using System;

namespace Ambev.DeveloperEvaluation.Domain.Common
{
    public abstract class DomainEvent
    {
        public DateTime OccurredOn { get; }

        protected DomainEvent()
        {
            OccurredOn = DateTime.UtcNow;
        }
    }
} 