using System.Configuration;

namespace Shoon
{
    public class ConnectionStringRetriever
    {
        public string GetTheConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["Simple.Data.Properties.Settings.DefaultConnectionString"].ConnectionString;
        }
    }
}