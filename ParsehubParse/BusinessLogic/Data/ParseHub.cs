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
        public async Task<Products> GetProductsAsync(string path)
        {
            try
            {
                HttpClient client = new HttpClient();

                Products products = null;
                var response = await client.GetAsync(path);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    byte[] data = DataNormalizeHelper.DecompressGzip(await response.Content.ReadAsStreamAsync());
                    var jsonString = System.Text.Encoding.UTF8.GetString(data);
                    
                    jsonString = DataNormalizeHelper.RemoveSpecialCharacters(jsonString);

                    products = JsonConvert.DeserializeObject<Products>(jsonString);
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
