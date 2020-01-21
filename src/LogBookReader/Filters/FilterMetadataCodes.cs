using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogBookReader.Filters
{
    public class FilterMetadataCodes : IModels.IMetadataCodes, IFilters.IFilterBase
    {
        public FilterMetadataCodes() { }
        public FilterMetadataCodes(Models.MetadataCodes metadataCode) { Fill(metadataCode); }

        public bool IsChecked { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }
        public string Uuid { get; set; }

        public void Fill(Models.MetadataCodes metadataCode)
        {
            Code = metadataCode.Code;
            Name = metadataCode.Name;
            Uuid = metadataCode.Uuid;
        }
    }
}
