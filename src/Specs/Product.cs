using System;
using SimpleCqrs.Domain;

namespace Specs
{
    public class Product : AggregateRoot
    {
        public Product()
        {
        }

        public Product(Guid id, string sku)
        {
            Apply(new ProductCreatedEvent {AggregateRootId = id, Sku = sku});
        }

        public void OnProductCreated(ProductCreatedEvent productCreatedEvent)
        {
            Id = productCreatedEvent.AggregateRootId;
        }

        public void SetName(string name)
        {
            Apply(new ProductNameSetEvent {Name = name});
        }

        public void MarkAsInactive()
        {
            Apply(new ProductMarkedAsInactiveEvent());
        }
    }
}