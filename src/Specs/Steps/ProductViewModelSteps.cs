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
            database.ProductViewModel.DeleteAll();
        }

        [Given(@"the product view model table has the following data")]
        public void GivenTheProductViewModelTableHasTheFollowingData(Table table)
        {
            var database = ScenarioContext.Current["Database"] as dynamic;

            database.ProductViewModel.DeleteAll();

            var products = table.CreateSet<ProductViewModel>();

            foreach (var product in products)
                database.ProductViewModel.Insert(product);
        }

        [Then(@"the following product view models should exist in the Product table")]
        public void ThenTheFollowingProductViewModelsShouldExistInTheProductTable(Table table)
        {
            var database = ScenarioContext.Current["Database"] as dynamic;

            IEnumerable<ProductViewModel> productViewModels = database.ProductViewModel.All().Cast<ProductViewModel>();
            table.CompareToSet(productViewModels);
        }
    }
}