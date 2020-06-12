using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ParsehubParse.Models
{
    public class Books
    {
        [JsonProperty("products")]
        public ICollection<Book> BooksCollection { get; set; }
    }
}
