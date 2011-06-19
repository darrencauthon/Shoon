using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Simple.Data;
using Simple.Data.SqlServer;
using SimpleCqrs.Eventing;

namespace Shoon
{
    public class SqlDenormalizer
    {
        private readonly dynamic db;
        private readonly IEnumerable<string> columns;

        public SqlDenormalizer()
        {
            db = Database.Open();

            var connectionString = ConfigurationManager.ConnectionStrings["Simple.Data.Properties.Settings.DefaultConnectionString"].ConnectionString;
            var sqlConnectionProvider = new SqlConnectionProvider(connectionString);
            var sqlSchemaProvider = new SqlSchemaProvider(sqlConnectionProvider);
            var table = sqlSchemaProvider.GetTables().First();
            columns = sqlSchemaProvider.GetColumns(table).Select(x => x.ActualName);
        }

        protected virtual void Insert(DomainEvent domainEvent)
        {
            db.Products.Insert(domainEvent);
        }

        protected virtual void Update(DomainEvent domainEvent)
        {
            var data = GetTheDataToUpdateInTheTable(domainEvent);

            db.Products.UpdateByAggregateRootId(data);
        }

        protected virtual void Upsert(DomainEvent domainEvent)
        {
            if (ThisRecordHasAlreadyBeenAdded(domainEvent))
                Update(domainEvent);
            else
                Insert(domainEvent);
        }

        private static object GetValue(DomainEvent domainEvent, string column)
        {
            return GetThePropertyOnThisObject(domainEvent, column).GetValue(domainEvent, null);
        }

        private static PropertyInfo GetThePropertyOnThisObject(object @object, string propertyName)
        {
            var type = @object.GetType();
            return type.GetProperties()
                .FirstOrDefault(x => x.Name == propertyName);
        }

        private Dictionary<string, object> GetTheDataToUpdateInTheTable(DomainEvent domainEvent)
        {
            var tableColumnsToUpdate = GetTheTableColumnsThatNeedToBeUpdated(domainEvent);

            return BuildADataObjectThatHasAllUpdatableData(domainEvent, tableColumnsToUpdate);
        }

        private Dictionary<string, object> BuildADataObjectThatHasAllUpdatableData(DomainEvent domainEvent, IEnumerable<string> tableColumnsToUpdate)
        {
            var dictionary = new Dictionary<string, object>();
            foreach (var property in tableColumnsToUpdate)
                dictionary[property] = GetValue(domainEvent, property);
            return dictionary;
        }

        private IEnumerable<string> GetTheTableColumnsThatNeedToBeUpdated(DomainEvent domainEvent)
        {
            return domainEvent.GetType().GetProperties()
                .Select(x => x.Name)
                .Where(property => columns.Contains(property));
        }

        private bool ThisRecordHasAlreadyBeenAdded(DomainEvent domainEvent)
        {
            return (bool) db.Products.FindAllByAggregateRootId(domainEvent.AggregateRootId).Any();
        }
    }
}