using Newtonsoft.Json;

namespace Api.Auth.Models
{
    public class TokenResponse
    {
        [JsonProperty("token")]
        public string Token { get; set; } = string.Empty;

        [JsonProperty("expiration")]
        public DateTime Expiration { get; set; }
    }
}
