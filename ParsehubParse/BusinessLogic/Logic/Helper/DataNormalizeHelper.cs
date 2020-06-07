using ParsehubParse.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
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
            string removableChars = Regex.Escape(@"【】");
            string pattern = "[" + removableChars + "]";

            return Regex.Replace(str, pattern, "");
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
                if (description_2_collection.Count() > 0)
                {
                    foreach (var item in description_2_collection)
                    {
                        description += item.Values;
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
            return  $"{initialDescription} {Environment.NewLine} " +
                    $"Titulo {Environment.NewLine}" +
                    $"{title}{Environment.NewLine}" +
                    $"-------------------------------------------------------" +
                    $"{Environment.NewLine}" +
                    $"{description}{Environment.NewLine}" +
                    $"{ Environment.NewLine}" +
                    $"{finalDescription}";
        }

        public static double DeterminePrice(Product product)
        {
            double pesosPrice = 0.0;

            if (product.Price_1 == null && product.Price_2 == null)
            {
                return 0;
            }

            var dollarPrice = product.Price_1 != null ? 
                                Convert.ToDouble(product.Price_1) : Convert.ToDouble(product.Price_2);

            var weight = GetWeightOnKilos(product);


            return pesosPrice;
        }

        public static double GetWeightOnKilos(Product product)
        {
            var weightStr = "";
            double weight = 0;

            int i = 0;
            while (i < product.Details.Count())
            {
                if (product.Details[i].Detail_title == "Peso del producto")
                {
                    weightStr = product.Details[i].Detail_value;
                }

                i++;
            }

            if (weightStr.Contains("pounds"))
            {
                weightStr.Replace(" pounds", "");
                weight = Convert.ToDouble(weightStr);
                weight = Math.Round((weight / 35.274) * 2, MidpointRounding.AwayFromZero) / 2;

            }

            if (weightStr.Contains("onzas"))
            {
                weightStr.Replace(" onzas", "");

                weight = Convert.ToDouble(weightStr);
                weight = Math.Round((weight / 2.205) * 2, MidpointRounding.AwayFromZero) / 2;
            }

            return weight;
        } 
    }
}
