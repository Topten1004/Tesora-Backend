namespace NFTBlockchain.Services.Models
{
    public class Metadata
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string? External_url { get; set; }
        public List<Attribute>? Attributes { get; set; }
    }

}
