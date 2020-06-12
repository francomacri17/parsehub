using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ParsehubParse.BusinessLogic.Data;
using ParsehubParse.BusinessLogic.Logic.Helper;
using ParsehubParse.Models;

namespace ParsehubParse.BusinessLogic.Logic
{
    public class ProductsBusinessLogic
    {
        public async Task GeneratedExcelProductsAsync()
        {
            var productsBaseUrl = ConfigurationManager.AppSettings["ParseHubListProductsBaseUrl"];
            var parseApiKey = ConfigurationManager.AppSettings["ParseHubApiKey"];

            var path = String.Format(productsBaseUrl, parseApiKey);

            var parseHub = new ParseHub();
            var product = await parseHub.GetProductsAsync(path);

            var mercadoLibreItems = new List<MercadoLibreItem>();

            foreach (var item in product.ProductsCollection)
            {
                if (item != null && (item.Price_1 != null || item.Price_2 != null))
                {
                    var price = DataNormalizeHelper.GetProductPrice(item);
                    if (price < 200)
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
                        mercadoLibreItem.Price = DeterminateProductPrice(item);
                        mercadoLibreItem.ShippingMethod = "Estándar a domicilio";
                        mercadoLibreItem.ShippingMode = "MercadoEnvios2";
                        mercadoLibreItem.ShippingPrice = 109.00;
                        mercadoLibreItem.ShippingType = "Sí";
                        mercadoLibreItem.SKU = "";
                        mercadoLibreItem.State = "Activa";
                        mercadoLibreItem.Stock = 3;
                        mercadoLibreItem.Title = DataNormalizeHelper.SortTitle(item.Name);
                        mercadoLibreItem.TypePublication = "Premium";
                        mercadoLibreItem.Warranty = "Sin garantía";

                        mercadoLibreItems.Add(mercadoLibreItem);
                    }
                }
            }

            DataNormalizeHelper.GnerateExcelFile(mercadoLibreItems);
        }

        public double DeterminateProductPrice(Product product)
        {
            try
            {
                var documentaryPrice = Convert.ToDouble(ConfigurationManager.AppSettings["DocumentaryPrice"]);
                var shipingPrice = Convert.ToDouble(ConfigurationManager.AppSettings["ShipingPrice"]);
                var dollarPrice = Convert.ToDouble(ConfigurationManager.AppSettings["DollarPrice"]);
                var mercadoLibreTax = Convert.ToDouble(ConfigurationManager.AppSettings["MercadoLibreTax"]);
                var kiloPrice1to5 = Convert.ToDouble(ConfigurationManager.AppSettings["KiloPrice1to5"]);
                var kiloPrice5to10 = Convert.ToDouble(ConfigurationManager.AppSettings["KiloPrice5to10"]);
                var kiloPrice10to20 = Convert.ToDouble(ConfigurationManager.AppSettings["KiloPrice10to20"]);

                var finalPrice = 0.00;
                var productPrice = DataNormalizeHelper.GetProductPrice(product);
                var weight = DataNormalizeHelper.GetWeightOnKilos(product);
                var kiloPrice = weight <= 5 ? kiloPrice1to5 : weight <= 10 ? kiloPrice5to10 : kiloPrice10to20;
                var prodcutGain = productPrice <= 50 ? 25.00 :
                                    productPrice <= 100 ? 30.00 :
                                        productPrice <= 200 ? 35 :
                                            (productPrice * 0.60) + 200;

                finalPrice = Math.Round((((productPrice + prodcutGain
                                            + (weight * kiloPrice)
                                                + documentaryPrice) * dollarPrice)
                                                    / mercadoLibreTax) + shipingPrice);

                return finalPrice;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public double DeterminateBooktPrice(Double bookPrice)
        {
            try
            {
                var shipingPrice = Convert.ToDouble(ConfigurationManager.AppSettings["BookShipingPrice"]);
                var dollarPrice = Convert.ToDouble(ConfigurationManager.AppSettings["DollarPrice"]);
                var mercadoLibreTax = Convert.ToDouble(ConfigurationManager.AppSettings["MercadoLibreTax"]);
                var shippingBookPrice = 10.00;

                var finalPrice = 0.00;
                var prodcutGain = 10.00;

                finalPrice = Math.Round((((bookPrice + prodcutGain + shippingBookPrice)
                                                * dollarPrice)
                                                    / mercadoLibreTax) + shipingPrice);

                return finalPrice;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public async Task GeneratedExcelBooksAsync()
        {
            var booksBaseUrl = ConfigurationManager.AppSettings["ParseHubListBooksBaseUrl"];
            var parseApiKey = ConfigurationManager.AppSettings["ParseHubApiKey"];

            var path = String.Format(booksBaseUrl, parseApiKey);

            var parseHub = new ParseHub();
            var books = await parseHub.GetBooksAsync(path);
            var mercadoLibreItems = new List<MercadoLibreItem>();

            foreach (var item in books.BooksCollection)
            {
                if (item != null)
                {
                    var price = DataNormalizeHelper.GetBookPrice(item);

                    if (price > 0)
                    {
                        if (price < 200)
                        {
                            var paperback = "";
                            var ISBN10 = "";
                            var editor = "";

                            foreach (var detail in item.Details)
                            {
                                foreach (var dcictionary in detail)
                                {
                                    if (dcictionary.Value.Contains("Paperback"))
                                    {
                                        paperback = Regex.Match(dcictionary.Value, @"\d+").Value;
                                    }
                                    if (dcictionary.Value.Contains("ISBN-10"))
                                    {
                                        ISBN10 = dcictionary.Value.Replace("ISBN-10: ", "");
                                    }
                                    if (dcictionary.Value.Contains("Editor"))
                                    {
                                        editor = dcictionary.Value.Replace("Editor: ", "");
                                    }
                                }
                            }

                            var mercadoLibreItem = new MercadoLibreItem();
                            mercadoLibreItem.AvailityStock = 10;
                            mercadoLibreItem.Category = "";
                            mercadoLibreItem.Condition = "Nuevo";
                            mercadoLibreItem.CreatedDate = DateTime.UtcNow.ToString("dd/MM/yyyy hh:MM");
                            mercadoLibreItem.Description = DataNormalizeHelper.GetBookDescription(item.Name, item.Description_1);
                            mercadoLibreItem.Id = "";
                            mercadoLibreItem.Images = DataNormalizeHelper.GetListImages(item.Image_1);
                            mercadoLibreItem.PickUp = "Sí";
                            mercadoLibreItem.Price = DeterminateBooktPrice(price);
                            mercadoLibreItem.ShippingMethod = "Estándar a domicilio";
                            mercadoLibreItem.ShippingMode = "MercadoEnvios2";
                            mercadoLibreItem.ShippingPrice = 109.00;
                            mercadoLibreItem.ShippingType = "Sí";
                            mercadoLibreItem.SKU = "";
                            mercadoLibreItem.State = "Activa";
                            mercadoLibreItem.Stock = 3;
                            mercadoLibreItem.Title = DataNormalizeHelper.SortTitle(item.Name);
                            mercadoLibreItem.TypePublication = "Premium";
                            mercadoLibreItem.Warranty = "Sin garantía";
                            mercadoLibreItem.Author = item.Author != null ? item.Author : "";
                            mercadoLibreItem.Editor = editor;
                            mercadoLibreItem.Isbn10 = ISBN10;
                            mercadoLibreItem.Paperback = paperback;
                            
                            mercadoLibreItems.Add(mercadoLibreItem);
                        }
                    }
                }

                DataNormalizeHelper.GnerateExcelBooksFile(mercadoLibreItems);
            }
        }
    }
}
