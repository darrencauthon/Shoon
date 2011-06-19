using System;
using TechTalk.SpecFlow;

namespace Specs.Steps
{
    [Binding]
    public class SqlDenormalizerSteps
    {
        [When(@"an event to set the price of product '(.*)' to (.*) without a create event")]
        public void x(string id, int price)
        {
            var sqlDenormalizer = ScenarioContext.Current.Get<TestingRuntime>()
                .ServiceLocator.Resolve<ProductViewModelDenormalizer>();
            sqlDenormalizer.Handle(new ProductPriceSetEvent {AggregateRootId = new Guid(id), Price = price});
        }
    }
}