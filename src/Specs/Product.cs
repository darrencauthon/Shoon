using System;
using SimpleCqrs.Domain;
using Specs.Steps;

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
    }
}