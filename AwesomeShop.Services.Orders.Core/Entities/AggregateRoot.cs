using AwesomeShop.Services.Orders.Core.Events;
using System;
using System.Collections.Generic;

namespace AwesomeShop.Services.Orders.Core.Entities
{
    public class AggregateRoot : IEntityBase
    {
        private List<IDomainEvent> _events = new();

        public IEnumerable<IDomainEvent> Events => _events;

        public Guid Id { get; protected set; }

        protected void AddEvent(IDomainEvent @event)
        {
            _events ??= new List<IDomainEvent>();
            _events.Add(@event);
        }
    }
}
