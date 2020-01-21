using System.ComponentModel.DataAnnotations;

namespace LogBookReader.Models
{
    public class MetadataCodes : IModels.IMetadataCodes
    {
        [Key]
        public int Code { get; set; }
        public string Name { get; set; }
        public string Uuid { get; set; }
    }
}
