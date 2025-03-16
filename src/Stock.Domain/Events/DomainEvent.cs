using System;

namespace Stock.Domain.Events
{
    /// <summary>
    /// Domain olayları için temel sınıf
    /// </summary>
    public abstract class DomainEvent
    {
        /// <summary>
        /// Olay ID
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Olayın oluşturulma zamanı
        /// </summary>
        public DateTime CreatedAt { get; }

        /// <summary>
        /// Yeni bir domain olayı oluşturur
        /// </summary>
        protected DomainEvent()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }
    }
} 