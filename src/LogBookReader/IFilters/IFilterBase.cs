namespace LogBookReader.IFilters
{
    public interface IFilterBase
    {
        bool IsChecked { get; set; }
        int Code { get; set; }
        string Name { get; set; }
    }
}
