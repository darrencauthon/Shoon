using SimpleCqrs.Domain;
using TechTalk.SpecFlow;

namespace Specs.Steps
{
    [Binding]
    public class TestingRuntimeSteps
    {
        [BeforeScenario]
        public void Setup()
        {
            var runtime = new TestingRuntime();
            runtime.Start();

            ScenarioContext.Current.Set(runtime);
            ScenarioContext.Current.Set(runtime.ServiceLocator.Resolve<IDomainRepository>());
        }

        [AfterScenario]
        public void Teardown()
        {
            var runtime = ScenarioContext.Current.Get<TestingRuntime>();
            runtime.Shutdown();
        }
    }
}