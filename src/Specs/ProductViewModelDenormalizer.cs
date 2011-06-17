using Simple.Data;
using SimpleCqrs.Eventing;

namespace Specs
{
    public class ProductViewModelDenormalizer : IHandleDomainEvents<ProductCreatedEvent>
    {
        public void Handle(ProductCreatedEvent domainEvent)
        {
            var db = Database.Open();
            db.Products.Insert(AggregateRootId: domainEvent.AggregateRootId);
        }
    }
}