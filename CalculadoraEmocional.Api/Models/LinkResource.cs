namespace CalculadoraEmocional.Api.Models
{
    public class LinkResource
    {
        public string Rel { get; set; } = string.Empty;    // ex.: "self", "post-checkin"
        public string Href { get; set; } = string.Empty;   // ex.: "/api/v1/..."
        public string Method { get; set; } = string.Empty; // ex.: "GET", "POST"
    }
}
