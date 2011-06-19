using Shoon;
using SimpleCqrs.Eventing;

namespace Specs
{
    public class ProductViewModelDenormalizer : SqlDenormalizer,
                                                IHandleDomainEvents<ProductCreatedEvent>,
                                                IHandleDomainEvents<ProductNameSetEvent>,
                                                IHandleDomainEvents<ProductMarkedAsInactiveEvent>
    {
        public void Handle(ProductCreatedEvent domainEvent)
        {
            Insert(domainEvent);
        }

        public void Handle(ProductNameSetEvent domainEvent)
        {
            Update(domainEvent);
        }

        public void Handle(ProductPriceSetEvent domainEvent)
        {
            Upsert(domainEvent);
        }

        public void Handle(ProductMarkedAsInactiveEvent domainEvent)
        {
            Delete(domainEvent);
        }
    }
}