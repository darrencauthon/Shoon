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

        [When(@"the name of the product '(.*)' is set to '(.*)'")]
        public void WhenTheNameOfTheProductIsSetToApplesauceCleaner(string id, string name)
        {
            var domainRepository = ScenarioContext.Current.Get<IDomainRepository>();
            var product = domainRepository.GetById<Product>(new Guid(id));
            product.SetName(name);
            domainRepository.Save(product);
        }

        [When(@"the price of the product '(.*)' is set to ")]
        public void WhenThePriceOfTheProduct68EC20BA_4DF3_4328_9E7E_60DEEDB3DF29IsSetTo4_00()
        {
            ScenarioContext.Current.Pending();
        }

        public class GetDataFromTable
        {
            public Guid AggregateRootId { get; set; }
            public string Sku { get; set; }
        }
    }
}