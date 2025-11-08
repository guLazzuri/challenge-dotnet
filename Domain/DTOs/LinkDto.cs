namespace challenge.Domain.DTOs
{
    /// <summary>
    /// Representa um link HATEOAS
    /// </summary>
    public class LinkDto
    {
        /// <summary>
        /// URL do link
        /// </summary>
        public string Href { get; set; } = string.Empty;

        /// <summary>
        /// Tipo de relação do link (ex: self, next, prev, first, last)
        /// </summary>
        public string Rel { get; set; } = string.Empty;

        /// <summary>
        /// Método HTTP do link
        /// </summary>
        public string Method { get; set; } = "GET";

        public LinkDto() { }

        public LinkDto(string href, string rel, string method = "GET")
        {
            Href = href;
            Rel = rel;
            Method = method;
        }
    }
}

