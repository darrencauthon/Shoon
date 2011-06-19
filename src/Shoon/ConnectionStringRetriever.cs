namespace Shoon
{
    public interface IConnectionStringRetriever
    {
        string GetTheConnectionString();
    }

    public class ConnectionStringRetriever : IConnectionStringRetriever
    {
        private readonly string connectionString;

        public ConnectionStringRetriever(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public string GetTheConnectionString()
        {
            return connectionString;
        }
    }
}