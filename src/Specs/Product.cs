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
    }
}