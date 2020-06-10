using ClosedXML.Excel;
using ParsehubParse.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace ParsehubParse.BusinessLogic.Logic.Helper
{
    public class DataNormalizeHelper
    {
        public static byte[] DecompressGzip(Stream streamInput)
        {
            Stream streamOutput = new MemoryStream();
            int iOutputLength = 0;

            try
            {
                byte[] readBuffer = new byte[4096];
                /// read from input stream and write to gzip stream
                using (GZipStream streamGZip = new GZipStream(streamInput, CompressionMode.Decompress))
                {
                    int i;

                    while ((i = streamGZip.Read(readBuffer, 0, readBuffer.Length)) != 0)
                    {
                        streamOutput.Write(readBuffer, 0, i);
                        iOutputLength = iOutputLength + i;
                    }
                }
            }
            catch (Exception ex)
            {
                // todo: handle exceptio
            }
            /// read uncompressed data from output stream into a byte array
            byte[] buffer = new byte[iOutputLength];
            streamOutput.Position = 0;
            streamOutput.Read(buffer, 0, buffer.Length);

            return buffer;
        }

        public static string RemoveSpecialCharacters(string str)
        {
            string removableChars = Regex.Escape(@"【】★√●✅🥉✓");
            string pattern = "[" + removableChars + "]";

            return Regex.Replace(str, pattern, " ");
        }

        public static string SortTitle(string title)
        {
            if (title.Length > 40)
            {
                title = title.Substring(0, 37);
                title = title + "...";
            }
            return title;
        }

        public static string NormalizeDescription(string description_1, 
                                                    List<Dictionary<string,string>> description_2_collection)
        {
            string description = "";
            if(description_1 != null)
            {
                description = description_1;
            }

            if (description_2_collection != null)
            {
                if (description.Length < 250)
                {
                    if (description_2_collection.Count() > 0)
                    {
                        foreach (var item in description_2_collection)
                        {
                            foreach (var dictionary in item)
                            {
                                description += $"{dictionary.Value} \n";
                            }
                        }
                    }
                }
            }

            return description;
        }

        public static List<string> GetListImages(Product product)
        {
            var images = new List<string>();
            images.Add(product.Image_1);
            images.Add(product.Image_2);
            images.Add(product.Image_3);
            images.Add("C:\\Users\\Admin\\Pictures\\USA direct\\Banner1.png");
            images.Add("C:\\Users\\Admin\\Pictures\\USA direct\\LogoOrange.png");
            return images;
        }

        public static string GetProductDescription(string title, string description)
        {
            var initialDescription = ConfigurationManager.AppSettings["InitialDescription"];
            var finalDescription = ConfigurationManager.AppSettings["FinalDescription"];
            return  $"{initialDescription} \n \n" +
                    $"Titulo:  \n" +
                    $"{title} \n" +
                    $"\n" +
                    $"{description} \n" +
                    $"\n" +
                    $"{finalDescription} \n";
        }

        public static double GetProductPrice(Product product)
        {
            double dollarPrice = 0.0;
            var tax = Convert.ToDouble(ConfigurationManager.AppSettings["Tax"]);

            if (product.Price_1 == null && product.Price_2 == null)
            {
                return dollarPrice;
            }

            dollarPrice = product.Price_1 != null ? 
                                Convert.ToDouble(product.Price_1) : Convert.ToDouble(product.Price_2);



            return dollarPrice / tax;
        }

        public static double GetWeightOnKilos(Product product)
        {
            var weightStr = "";
            double weight = 2.00;

            int i = 0;

            if(product.Details != null)
            {
                while (i < product.Details.Count())
                {
                    if (product.Details[i].Detail_title == "Peso del producto")
                    {
                        weightStr = product.Details[i].Detail_value;
                        i = product.Details.Count();
                    }

                    i++;
                }
            }
            else if(product.Details_1 != null)
            {
                while (i < product.Details_1.Count())
                {
                    if (product.Details_1[i].Detail_title == "Peso del producto")
                    {
                        weightStr = product.Details_1[i].Detail_value;
                        i = product.Details_1.Count();
                    }

                    i++;
                }
            }

            if (weightStr != "")
            {

                if (weightStr.Contains("pounds"))
                {
                    weightStr = weightStr.Replace(" pounds", "");
                    weight = Convert.ToDouble(weightStr);
                    weight = Math.Round(((weight / 2.205) * 2), MidpointRounding.AwayFromZero) / 2;

                }

                if (weightStr.Contains("onzas"))
                {
                    weightStr = weightStr.Replace(" onzas", "");
                    weight = Convert.ToDouble(weightStr);
                    weight = Math.Round((weight / 35.274) * 2, MidpointRounding.AwayFromZero) / 2;
                }
            }

            return weight;
        } 

        public static bool GnerateExcelFile(List<MercadoLibreItem> mercadoLibreItems)
        {
            try
            {
                using (var workbook = new XLWorkbook())
                {
                    var destinationFolder = ConfigurationManager.AppSettings["DestinationFolder"];
                    var path = $"{destinationFolder}\\products.xlsx";
                    var worksheet = workbook.Worksheets.Add("Products");
                    var currentRow = 1;

                    worksheet.Cell(currentRow, 1).Value = "Id";
                    worksheet.Cell(currentRow, 2).Value = "Categoria";
                    worksheet.Cell(currentRow, 3).Value = "Titulo";
                    worksheet.Cell(currentRow, 4).Value = "Descripcion";
                    worksheet.Cell(currentRow, 5).Value = "Precio";
                    worksheet.Cell(currentRow, 6).Value = "SKU";
                    worksheet.Cell(currentRow, 7).Value = "Estado";
                    worksheet.Cell(currentRow, 8).Value = "Stock";
                    worksheet.Cell(currentRow, 9).Value = "Disponibilidad de stock";
                    worksheet.Cell(currentRow, 10).Value = "Tipo de publicacion";
                    worksheet.Cell(currentRow, 11).Value = "Condicion";
                    worksheet.Cell(currentRow, 12).Value = "Envio Gratis";
                    worksheet.Cell(currentRow, 13).Value = "Precio de envio";
                    worksheet.Cell(currentRow, 14).Value = "Modo envio";
                    worksheet.Cell(currentRow, 15).Value = "Metodo de envio";
                    worksheet.Cell(currentRow, 16).Value = "Retiro en persona";
                    worksheet.Cell(currentRow, 17).Value = "Garantia";
                    worksheet.Cell(currentRow, 18).Value = "Fecha de creacion";
                    worksheet.Cell(currentRow, 19).Value = "Última Actualización";
                    worksheet.Cell(currentRow, 20).Value = "Resultado";
                    worksheet.Cell(currentRow, 21).Value = "Resultado Observaciones";
                    worksheet.Cell(currentRow, 22).Value = "Imagen 1";
                    worksheet.Cell(currentRow, 23).Value = "Imagen 2";
                    worksheet.Cell(currentRow, 24).Value = "Imagen 3";
                    worksheet.Cell(currentRow, 25).Value = "Imagen 4";
                    worksheet.Cell(currentRow, 26).Value = "Imagen 5";


                    foreach (var item in mercadoLibreItems)
                    {
                        currentRow++;
                        worksheet.Cell(currentRow, 1).Value = item.Id;
                        worksheet.Cell(currentRow, 2).Value = item.Category;
                        worksheet.Cell(currentRow, 3).Value = item.Title;
                        worksheet.Cell(currentRow, 4).Value = item.Description;
                        worksheet.Cell(currentRow, 5).Value = item.Price;
                        worksheet.Cell(currentRow, 6).Value = item.SKU;
                        worksheet.Cell(currentRow, 7).Value = item.State;
                        worksheet.Cell(currentRow, 8).Value = item.Stock;
                        worksheet.Cell(currentRow, 9).Value = item.AvailityStock;
                        worksheet.Cell(currentRow, 10).Value = item.TypePublication;
                        worksheet.Cell(currentRow, 11).Value = item.Condition;
                        worksheet.Cell(currentRow, 12).Value = item.ShippingType;
                        worksheet.Cell(currentRow, 13).Value = item.ShippingPrice;
                        worksheet.Cell(currentRow, 14).Value = item.ShippingMode;
                        worksheet.Cell(currentRow, 15).Value = item.ShippingMethod;
                        worksheet.Cell(currentRow, 16).Value = item.PickUp;
                        worksheet.Cell(currentRow, 17).Value = item.Warranty;
                        worksheet.Cell(currentRow, 18).Value = item.CreatedDate;
                        worksheet.Cell(currentRow, 19).Value = item.LastUpdated;
                        worksheet.Cell(currentRow, 20).Value = item.Result;
                        worksheet.Cell(currentRow, 21).Value = item.ResultObservations;
                        worksheet.Cell(currentRow, 22).Value = item.Images[0] != null ? item.Images[0] : "";
                        worksheet.Cell(currentRow, 23).Value = item.Images[1] != null ? item.Images[1] : "";
                        worksheet.Cell(currentRow, 24).Value = item.Images[2] != null ? item.Images[2] : "";
                        worksheet.Cell(currentRow, 25).Value = item.Images[3] != null ? item.Images[3] : "";
                        worksheet.Cell(currentRow, 26).Value = item.Images[4] != null ? item.Images[4] : "";
                    }

                    using (var stream = new MemoryStream())
                    {
                        Console.WriteLine($"Excel file created , you can find the file {path}");
                        workbook.SaveAs(path);

                        return true;
                    }
                }
            }
            catch (Exception e)
            {

                throw;
            }
        }
    }
}
