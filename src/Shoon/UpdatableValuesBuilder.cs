using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Simple.Data.SqlServer;
using SimpleCqrs.Eventing;

namespace Shoon
{
    public class UpdatableValuesBuilder
    {
        private readonly IConnectionStringRetriever connectionStringRetriever;

        public UpdatableValuesBuilder(IConnectionStringRetriever connectionStringRetriever)
        {
            this.connectionStringRetriever = connectionStringRetriever;
        }

        public IDictionary<string, object> GetTheDataToUpdateInTheTable(DomainEvent domainEvent, string tableName)
        {
            var tableColumnsToUpdate = GetTheTableColumnsThatNeedToBeUpdated(domainEvent, tableName);

            return BuildADataObjectThatHasAllUpdatableData(domainEvent, tableColumnsToUpdate);
        }

        private IEnumerable<string> GetTheTableColumnsThatNeedToBeUpdated(DomainEvent domainEvent, string tableName)
        {
            return domainEvent.GetType().GetProperties()
                .Select(x => x.Name)
                .Where(property => GetTheColumnsInTheTable(tableName).Contains(property));
        }

        private IEnumerable<string> GetTheColumnsInTheTable(string tableName)
        {
            var connectionString = GetTheConnectionString();
            var sqlConnectionProvider = new SqlConnectionProvider(connectionString);
            var sqlSchemaProvider = new SqlSchemaProvider(sqlConnectionProvider);
            var table = sqlSchemaProvider.GetTables().Single(x=>x.ActualName == tableName);
            return sqlSchemaProvider.GetColumns(table).Select(x => x.ActualName);
        }

        private static Dictionary<string, object> BuildADataObjectThatHasAllUpdatableData(DomainEvent domainEvent,
                                                                                          IEnumerable<string> tableColumnsToUpdate)
        {
            var dictionary = new Dictionary<string, object>();
            foreach (var property in tableColumnsToUpdate)
                dictionary[property] = GetValue(domainEvent, property);
            dictionary["Id"] = domainEvent.AggregateRootId;
            return dictionary;
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

        private string GetTheConnectionString()
        {
            return connectionStringRetriever.GetTheConnectionString();
        }
    }
}