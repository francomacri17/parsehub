using Newtonsoft.Json;
using ParsehubParse.BusinessLogic.Logic;
using ParsehubParse.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ParsehubParse
{
    class Program
    {
        static HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            Console.WriteLine("Generando productos Excel");

            var productsBusinessLogic = new ProductsBusinessLogic();
            await productsBusinessLogic.GeneratedExcelProductsAsync();
        }
    }
}
