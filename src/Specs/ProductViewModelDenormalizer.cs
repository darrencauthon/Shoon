using Shoon;
using SimpleCqrs.Eventing;

namespace Specs
{
    public class ProductViewModelDenormalizer : SqlDenormalizer, IHandleDomainEvents<ProductCreatedEvent>
    {
        public void Handle(ProductCreatedEvent domainEvent)
        {
            Insert(domainEvent);
        }
    }
}