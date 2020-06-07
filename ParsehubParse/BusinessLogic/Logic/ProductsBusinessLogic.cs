using System;
using System.Collections.Generic;
using System.Configuration;
using ParsehubParse.BusinessLogic.Data;
using ParsehubParse.BusinessLogic.Logic.Helper;
using ParsehubParse.Models;

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
            var product = await parseHub.GetProductsAsync(path);

            foreach (var item in product.ProductsCollection)
            {
                var mercadoLibreItem = new MercadoLibreItem();
                mercadoLibreItem.AvailityStock = 10;
                mercadoLibreItem.Category = "";
                mercadoLibreItem.Condition = "Nuevo";
                mercadoLibreItem.CreatedDate = DateTime.UtcNow.ToString("dd/MM/yyyy hh:MM");
                var description = DataNormalizeHelper.NormalizeDescription(item.Description_1, item.Description_2);
                mercadoLibreItem.Description = DataNormalizeHelper.GetProductDescription(item.Name, description);
                mercadoLibreItem.Id = "";
                mercadoLibreItem.Images = DataNormalizeHelper.GetListImages(item);
                mercadoLibreItem.PickUp = "Sí";
                //mercadoLibreItem.Price = DataNormalizeHelper.DeterminePrice()
            }
        }
    }
}
