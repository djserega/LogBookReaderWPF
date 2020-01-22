using System.ComponentModel.DataAnnotations;

namespace LogBookReader.Models
{
    public class EventCodes : IModels.IEventCodes, IModels.IModelsBase
    {
        [Key]
        public int Code { get; set; }
        public string Name { get; set; }
    }
}
