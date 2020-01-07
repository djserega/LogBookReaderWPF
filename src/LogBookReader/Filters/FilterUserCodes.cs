namespace LogBookReader.Filters
{
    public class FilterUserCodes : IModels.IUserCodes, IFilters.IFilterBase
    {
        public FilterUserCodes() { }
        public FilterUserCodes(Models.UserCodes userCode) { Fill(userCode); }

        public int Code { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }

        public void Fill(Models.UserCodes userCode)
        {
            Code = userCode.Code;
            Name = userCode.Name;
        }
    }
}
