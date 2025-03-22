using Newtonsoft.Json;

namespace API.Models.Default.Search
{
    public class SearchResponseModel
    {
        [JsonProperty("ruc", NullValueHandling = NullValueHandling.Ignore)]
        public string Ruc { get; set; }

        [JsonProperty("nombre_o_razon_social")]
        public string NombreORazonSocial { get; set; }

        [JsonProperty("direccion")]
        public string Direccion { get; set; }

        [JsonProperty("estado")]
        public string Estado { get; set; }

        [JsonProperty("condicion")]
        public string Condicion { get; set; }

        [JsonProperty("ubigeo_sunat")]
        public string UbigeoSunat { get; set; }

    }

    
}