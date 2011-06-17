using System.Collections.Generic;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Specs.Steps
{
    [Binding]
    public class ProductViewModelSteps
    {
        [Given(@"the product view model table is empty")]
        public void GivenTheProductTableIsEmpty()
        {
            var database = ScenarioContext.Current["Database"] as dynamic;
            database.Products.DeleteAll();
        }

        [Then(@"the following product view models should exist in the Product table")]
        public void ThenTheFollowingProductViewModelsShouldExistInTheProductTable(Table table)
        {
            var database = ScenarioContext.Current["Database"] as dynamic;
            
            IEnumerable<ProductViewModel> productViewModels = database.Products.All().Cast<ProductViewModel>();
            table.CompareToSet(productViewModels);
        }
    }
}