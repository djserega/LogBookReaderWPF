using System.ComponentModel.DataAnnotations;
using LogBookReader.IModels;

namespace LogBookReader.Models
{
    public class EventCodes : IEventCodes
    {
        [Key]
        public int Code { get; set; }
        public string Name { get; set; }
    }
}
