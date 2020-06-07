using System;
using System.Collections.Generic;
using System.Text;

namespace ParsehubParse.Models
{
    public class MercadoLibreItem
    {
        public string Id { get; set; }

        public string Category { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public string SKU { get; set; }

        public string State { get; set; }

        public int Stock { get; set; }

        public int AvailityStock { get; set; }

        public string TypePublication { get; set; }

        public string Condition { get; set; }

        public string ShippingType { get; set; }

        public double ShippingPrice { get; set; }

        public string ShippingMode { get; set; }

        public string ShippingMethod { get; set; }

        public string PickUp { get; set; }

        public string Warranty { get; set; }

        public string CreatedDate { get; set; }

        public List<string> Images { get; set; }
    }
}
