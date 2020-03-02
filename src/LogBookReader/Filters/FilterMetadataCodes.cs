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

        public string Child { get; set; }
        public string Parent { get; set; }

        public void Fill(Models.MetadataCodes metadataCode)
        {
            Code = metadataCode.Code;
            Name = metadataCode.Name;
            Uuid = metadataCode.Uuid;

            int positionDot = Name.IndexOf('.');
            if (positionDot > 0)
            {
                Parent = Name.Substring(0, positionDot);
                Child = Name.Substring(positionDot + 1, Name.Length - positionDot - 1);
            }
        }
    }
}
