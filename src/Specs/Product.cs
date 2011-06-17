using System;
using SimpleCqrs.Domain;

namespace Specs
{
    public class Product : AggregateRoot
    {
        public Product()
        {
        }

        public Product(Guid id)
        {
            Apply(new ProductCreatedEvent {AggregateRootId = id});
        }

        public void OnProductCreated(ProductCreatedEvent productCreatedEvent)
        {
            Id = productCreatedEvent.AggregateRootId;
        }
    }
}