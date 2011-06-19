using System.Collections.Generic;
using Simple.Data;
using SimpleCqrs.Eventing;

namespace Shoon
{
    public class SqlDenormalizer
    {
        private readonly ConnectionStringRetriever connectionStringRetriever;

        public SqlDenormalizer()
        {
            connectionStringRetriever = new ConnectionStringRetriever();
        }

        protected dynamic TheDatabaseTable
        {
            get
            {
                var connectionString = new ConnectionStringRetriever().GetTheConnectionString();

                return Database.OpenConnection(connectionString)["Products"];
            }
        }

        protected virtual void Insert(DomainEvent domainEvent)
        {
            var data = GetTheDataToUpdateInTheTable(domainEvent);

            TheDatabaseTable.Insert(data);
        }

        protected virtual void Update(DomainEvent domainEvent)
        {
            var data = GetTheDataToUpdateInTheTable(domainEvent);

            TheDatabaseTable.UpdateById(data);
        }

        protected virtual void Upsert(DomainEvent domainEvent)
        {
            if (ThisRecordHasBeenInserted(domainEvent))
                Update(domainEvent);
            else
                Insert(domainEvent);
        }

        protected void Delete(DomainEvent domainEvent)
        {
            TheDatabaseTable.DeleteById(domainEvent.AggregateRootId);
        }

        private IDictionary<string, object> GetTheDataToUpdateInTheTable(DomainEvent domainEvent)
        {
            return (new UpdatableValuesBuilder(connectionStringRetriever).GetTheDataToUpdateInTheTable(domainEvent));
        }

        private bool ThisRecordHasBeenInserted(DomainEvent domainEvent)
        {
            return (bool) TheDatabaseTable.FindAllById(domainEvent.AggregateRootId).Any();
        }
    }
}