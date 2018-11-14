namespace LogBookReader.IModels
{
    public interface IEventLog
    {
        int AppCode { get; set; }
        string Comment { get; set; }
        int ComputerCode { get; set; }
        int ConnectID { get; set; }
        string Data { get; set; }
        string DataPresentation { get; set; }
        int DataType { get; set; }
        int Date { get; set; }
        int EventCode { get; set; }
        string MetadataCodes { get; set; }
        int PrimaryPortCode { get; set; }
        int RowID { get; set; }
        int SecondaryPortCode { get; set; }
        int Session { get; set; }
        int SessionDataSplitCode { get; set; }
        int Severity { get; set; }
        int TransactionDate { get; set; }
        int TransactionID { get; set; }
        int TransactionStatus { get; set; }
        int UserCode { get; set; }
        int WorkServerCode { get; set; }
    }
}