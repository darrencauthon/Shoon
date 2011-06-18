using System.Configuration;
using System.Linq;
using Simple.Data;
using Simple.Data.SqlServer;
using SimpleCqrs.Eventing;

namespace Shoon
{
    public class SqlDenormalizer
    {
        private readonly dynamic db;

        public SqlDenormalizer()
        {
            db = Database.Open();

            var connectionString = ConfigurationManager.ConnectionStrings["Simple.Data.Properties.Settings.DefaultConnectionString"].ConnectionString;
            var sqlConnectionProvider = new SqlConnectionProvider(connectionString);
            var sqlSchemaProvider = new SqlSchemaProvider(sqlConnectionProvider);
            var table = sqlSchemaProvider.GetTables().First();
            var columns = sqlSchemaProvider.GetColumns(table);

        }

        public void Insert(DomainEvent domainEvent)
        {
            db.Products.Insert(domainEvent);
        }

        public void Update(DomainEvent domainEvent)
        {
            db.Products.UpdateByAggregateRootId(domainEvent);
        }
    }
}