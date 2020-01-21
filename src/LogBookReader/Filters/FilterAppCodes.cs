namespace LogBookReader.Filters
{
    public class FilterAppCodes : IModels.IAppCodes, IFilters.IFilterBase
    {
        public FilterAppCodes() { }

        public FilterAppCodes(Models.AppCodes appCode)
        {
            Fill(appCode);
        }

        public bool IsChecked { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }

        public void Fill(Models.AppCodes appCode)
        {
            Code = appCode.Code;
            Name = appCode.Name;
        }
    }
}
