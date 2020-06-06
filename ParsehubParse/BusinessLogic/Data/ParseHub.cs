using Newtonsoft.Json;
using ParsehubParse.BusinessLogic.Logic.Helper;
using ParsehubParse.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ParsehubParse.BusinessLogic.Data
{
    public class ParseHub
    {
        public async Task<List<Product>> GetProductAsync(string path)
        {
            try
            {
                HttpClient client = new HttpClient();

                List<Product> products = null;
                var response = await client.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var jsonString = DataNormalizeHelper.Decompress(content);
                    products = JsonConvert.DeserializeObject<List<Product>>(jsonString);
                }
                return products;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}
