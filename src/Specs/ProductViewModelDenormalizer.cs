using Shoon;
using SimpleCqrs.Eventing;

namespace Specs
{
    public class ProductViewModelDenormalizer : SqlDenormalizer,
                                                IHandleDomainEvents<ProductCreatedEvent>,
                                                IHandleDomainEvents<ProductNameSetEvent>
    {
        public void Handle(ProductCreatedEvent domainEvent)
        {
            Insert(domainEvent);
        }

        public void Handle(ProductNameSetEvent domainEvent)
        {
            Update(domainEvent);
        }
    }
}