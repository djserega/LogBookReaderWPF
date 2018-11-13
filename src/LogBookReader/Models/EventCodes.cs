using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
