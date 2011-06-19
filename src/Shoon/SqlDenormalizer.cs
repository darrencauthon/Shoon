using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using Simple.Data;
using Simple.Data.Ado.Schema;
using Simple.Data.SqlServer;
using SimpleCqrs.Eventing;

namespace Shoon
{
    public class SqlDenormalizer
    {
        private readonly dynamic db;
        private IEnumerable<string> columns;

        public SqlDenormalizer()
        {
            db = Database.Open();

            var connectionString = ConfigurationManager.ConnectionStrings["Simple.Data.Properties.Settings.DefaultConnectionString"].ConnectionString;
            var sqlConnectionProvider = new SqlConnectionProvider(connectionString);
            var sqlSchemaProvider = new SqlSchemaProvider(sqlConnectionProvider);
            var table = sqlSchemaProvider.GetTables().First();
            columns = sqlSchemaProvider.GetColumns(table).Select(x=>x.ActualName);

        }

        public void Insert(DomainEvent domainEvent)
        {
            db.Products.Insert(domainEvent);
        }

        public void Update(DomainEvent domainEvent)
        {
            var dictionary = new Dictionary<string, object>();
            foreach (var property in domainEvent.GetType().GetProperties().Select(x=>x.Name))
                if (columns.Contains(property))
                dictionary[property] = GetValue(domainEvent, property);

            db.Products.UpdateByAggregateRootId(dictionary);
        }

        private object GetValue(DomainEvent domainEvent, string column)
        {
            try
            {
                return GetThePropertyOnThisObject(domainEvent, column).GetValue(domainEvent, null);
            }
            catch
            {
                return null;
            }
        }

        private static PropertyInfo GetThePropertyOnThisObject(object @object, string propertyName)
        {
            var type = @object.GetType();
            return type.GetProperties()
                .FirstOrDefault(x => x.Name == propertyName);
        }

        protected void Upsert(DomainEvent domainEvent)
        {
            Insert(domainEvent);
        }
    }
}