using System;
using SimpleCqrs.Domain;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Specs.Steps
{
    [Binding]
    public class ProductSteps
    {
        [When(@"a product is created with the following data")]
        public void WhenTheProductCreatedEventIsHandledWithTheFollowingData(Table table)
        {
            var data = table.CreateInstance<GetDataFromTable>();

            var product = new Product(data.AggregateRootId, data.Sku);

            var domainRepository = ScenarioContext.Current.Get<IDomainRepository>();
            domainRepository.Save(product);
        }

        public class GetDataFromTable
        {
            public Guid AggregateRootId { get; set; }
            public string Sku { get; set; }
        }
    }
}