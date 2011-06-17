using Simple.Data;
using TechTalk.SpecFlow;

namespace Specs.Steps
{
    [Binding]
    public class SimpleDataSteps
    {
        [BeforeScenario]
        public void Setup()
        {
            var db = Database.Open();
            ScenarioContext.Current["Database"] = db;
        }
    }
}