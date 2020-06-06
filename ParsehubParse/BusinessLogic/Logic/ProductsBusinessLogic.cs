using System;
using System.Configuration;
using ParsehubParse.BusinessLogic.Data;

namespace ParsehubParse.BusinessLogic.Logic
{
    public class ProductsBusinessLogic
    {
        public async System.Threading.Tasks.Task GeneratedExcelProductsAsync()
        {
            var productsBaseUrl = ConfigurationManager.AppSettings["ParseHubListProductsBaseUrl"];
            var parseApiKey = ConfigurationManager.AppSettings["ParseHubApiKey"];

            var path = String.Format(productsBaseUrl, parseApiKey);

            var parseHub = new ParseHub();
            var products = await parseHub.GetProductAsync(path);

            products = null;
        }
    }
}
