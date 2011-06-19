using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Shoon;
using SimpleCqrs;
using TechTalk.SpecFlow;

namespace Specs.Steps
{
    [Binding]
    public class ConnectionStringRetrieverSteps
    {
        [Given(@"I have loaded a connection string retriever")]
        public void GivenIHaveLoadedAConnectionStringRetriever()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["Simple.Data.Properties.Settings.DefaultConnectionString"].ConnectionString;
         
            var runtime = ScenarioContext.Current.Get<TestingRuntime>();
            var serviceLocator = runtime.ServiceLocator;
            serviceLocator.Register<IConnectionStringRetriever>(new ConnectionStringRetriever(connectionString));
        }

    }
}
