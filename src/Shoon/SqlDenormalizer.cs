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
        private readonly IEnumerable<string> columns;

        public SqlDenormalizer()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Simple.Data.Properties.Settings.DefaultConnectionString"].ConnectionString;
            var sqlConnectionProvider = new SqlConnectionProvider(connectionString);
            var sqlSchemaProvider = new SqlSchemaProvider(sqlConnectionProvider);
            var table = sqlSchemaProvider.GetTables().First();
            columns = sqlSchemaProvider.GetColumns(table).Select(x => x.ActualName);
        }

        protected dynamic TheDatabaseTable
        {
            get { return Database.Open()["Products"]; }
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

        private static Dictionary<string, object> BuildADataObjectThatHasAllUpdatableData(DomainEvent domainEvent, IEnumerable<string> tableColumnsToUpdate)
        {
            var dictionary = new Dictionary<string, object>();
            foreach (var property in tableColumnsToUpdate)
                dictionary[property] = GetValue(domainEvent, property);
            dictionary["Id"] = domainEvent.AggregateRootId;
            return dictionary;
        }

        private IEnumerable<string> GetTheTableColumnsThatNeedToBeUpdated(DomainEvent domainEvent)
        {
            return domainEvent.GetType().GetProperties()
                .Select(x => x.Name)
                .Where(property => columns.Contains(property));
        }

        private bool ThisRecordHasBeenInserted(DomainEvent domainEvent)
        {
            return (bool) TheDatabaseTable.FindAllById(domainEvent.AggregateRootId).Any();
        }
    }
}