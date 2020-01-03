namespace LogBookReader.Filters
{
    public class FilterEventCodes : IModels.IEventCodes, IFilters.IFilterBase
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
