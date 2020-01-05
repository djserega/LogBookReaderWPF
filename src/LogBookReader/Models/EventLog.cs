using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogBookReader.Models
{
    [Table("EventLog")]
    public class EventLog : IModels.IEventLog
    {
        [Key]
        public int RowID { get; set; }
        public int Severity { get; set; }
        public long Date { get; set; }
        public int ConnectID { get; set; }
        public int Session { get; set; }
        public int TransactionStatus { get; set; }
        public int TransactionDate { get; set; }
        public int TransactionID { get; set; }
        public int UserCode { get; set; }
        public int ComputerCode { get; set; }
        public int AppCode { get; set; }
        public int EventCode { get; set; }
        public string Comment { get; set; }
        public string MetadataCodes { get; set; }
        public int SessionDataSplitCode { get; set; }
        public int DataType { get; set; }
        public string Data { get; set; }
        public string DataPresentation { get; set; }
        public int WorkServerCode { get; set; }
        public int PrimaryPortCode { get; set; }
        public int SecondaryPortCode { get; set; }
    }
}
