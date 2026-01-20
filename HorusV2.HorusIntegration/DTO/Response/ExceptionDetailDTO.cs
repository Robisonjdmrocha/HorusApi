using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HorusV2.HorusIntegration.DTO.Response
{
    public class ExceptionDetailDTO
    {

        [JsonProperty("codigo")]
        public string? Codigo { get; set; }

        [JsonProperty("mensagem")]
        public string? Mensagem { get; set; }

        [JsonProperty("caminho")]
        public string? Caminho { get; set; }

        [JsonProperty("valorRejeitado")]
        public string? ValorRejeitado { get; set; }
    }
}
