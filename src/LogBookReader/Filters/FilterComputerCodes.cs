namespace LogBookReader.Filters
{
    public class FilterComputerCodes : IModels.IComputerCodes, IFilters.IFilterBase
    {
        public FilterComputerCodes() { }
        public FilterComputerCodes(Models.ComputerCodes computerCode)
        {
            Fill(computerCode);
        }

        public bool IsChecked { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }

        public void Fill(Models.ComputerCodes computerCode)
        {
            Code = computerCode.Code;
            Name = computerCode.Name;
        }

    }
}
