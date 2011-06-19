using System.Collections.Generic;
using System.Configuration;
using Simple.Data;
using SimpleCqrs.Eventing;

namespace Shoon
{
    public class SqlDenormalizer
    {
        protected dynamic TheDatabaseTable
        {
            get { return Database.OpenConnection(GetTheConnectionString())["Products"]; }
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

        private static string GetTheConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["Simple.Data.Properties.Settings.DefaultConnectionString"].ConnectionString;
        }

        private static IDictionary<string, object> GetTheDataToUpdateInTheTable(DomainEvent domainEvent)
        {
            return (new UpdatableValuesBuilder().GetTheDataToUpdateInTheTable(domainEvent));
        }

        private bool ThisRecordHasBeenInserted(DomainEvent domainEvent)
        {
            return (bool) TheDatabaseTable.FindAllById(domainEvent.AggregateRootId).Any();
        }
    }
}