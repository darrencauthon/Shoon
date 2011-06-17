using Simple.Data;
using SimpleCqrs.Eventing;

namespace Shoon
{
    public class SqlDenormalizer
    {
        private readonly dynamic db;

        public SqlDenormalizer()
        {
            db = Database.Open();
        }

        public void Insert(DomainEvent domainEvent)
        {
            db.Products.Insert(AggregateRootId: domainEvent.AggregateRootId);
        }
    }
}