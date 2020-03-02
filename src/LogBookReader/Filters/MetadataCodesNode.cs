using System.Collections.ObjectModel;

namespace LogBookReader.Filters
{
    public class MetadataCodesNode
    {
        public bool IsChecked { get; set; }
        public string Name { get; set; }
        public ObservableCollection<MetadataCodesNode> Nodes { get; set; } = new ObservableCollection<MetadataCodesNode>();
    }
}
