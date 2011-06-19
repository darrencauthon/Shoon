using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Simple.Data.SqlServer;
using SimpleCqrs.Eventing;

namespace Shoon
{
    public class UpdatableValuesBuilder
    {
        public IDictionary<string, object> GetTheDataToUpdateInTheTable(DomainEvent domainEvent)
        {
            var tableColumnsToUpdate = GetTheTableColumnsThatNeedToBeUpdated(domainEvent);

            return BuildADataObjectThatHasAllUpdatableData(domainEvent, tableColumnsToUpdate);
        }

        protected IEnumerable<string> ColumnsInTheDatabaseTable
        {
            get { return GetTheColumnsInTheTable(); }
        }

        private IEnumerable<string> GetTheTableColumnsThatNeedToBeUpdated(DomainEvent domainEvent)
        {
            return domainEvent.GetType().GetProperties()
                .Select(x => x.Name)
                .Where(property => ColumnsInTheDatabaseTable.Contains(property));
        }

        private static IEnumerable<string> GetTheColumnsInTheTable()
        {
            var connectionString = GetTheConnectionString();
            var sqlConnectionProvider = new SqlConnectionProvider(connectionString);
            var sqlSchemaProvider = new SqlSchemaProvider(sqlConnectionProvider);
            var table = sqlSchemaProvider.GetTables().First();
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

        private static string GetTheConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["Simple.Data.Properties.Settings.DefaultConnectionString"].ConnectionString;
        }
    }
}