using System.Collections.Generic;
using Newtonsoft.Json;

namespace AcumaticaMetalsAPI
{
    public class CategoriesResponse : MetalsAPIResponse
    {
        [JsonProperty("categories")]
        public List<Category> Categories { get; set; }
    }

    public class Category
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("product_tax_code")]
        public string ProductTaxCode { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}
