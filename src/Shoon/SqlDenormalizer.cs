using System.Collections.Generic;
using Simple.Data;
using SimpleCqrs;
using SimpleCqrs.Eventing;

namespace Shoon
{
    public class SqlDenormalizer
    {
        private readonly IConnectionStringRetriever connectionStringRetriever;
        private readonly UpdatableValuesBuilder updateValuesBuilder;
        private const string tableName = "Products";

        public SqlDenormalizer()
        {
            connectionStringRetriever = ServiceLocator.Current.Resolve<IConnectionStringRetriever>();
            updateValuesBuilder = new UpdatableValuesBuilder(connectionStringRetriever, tableName);
        }

        protected dynamic TheDatabaseTable
        {
            get
            {
                var connectionString = connectionStringRetriever.GetTheConnectionString();

                return Database.OpenConnection(connectionString)[tableName];
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
            return (updateValuesBuilder.GetTheDataToUpdateInTheTable(domainEvent));
        }

        private bool ThisRecordHasBeenInserted(DomainEvent domainEvent)
        {
            return (bool) TheDatabaseTable.FindAllById(domainEvent.AggregateRootId).Any();
        }
    }
}