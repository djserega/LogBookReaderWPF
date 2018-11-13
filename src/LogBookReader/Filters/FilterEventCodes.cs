using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogBookReader.Filters
{
    public class FilterEventCodes : IModels.IEventCodes
    {
        public FilterEventCodes(Models.EventCodes eventCodes)
        {
            Fill(eventCodes);
        }

        public bool IsChecked { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }

        public void Fill(Models.EventCodes eventCodes)
        {
            Code = eventCodes.Code;
            Name = eventCodes.Name;
        }
    }
}
