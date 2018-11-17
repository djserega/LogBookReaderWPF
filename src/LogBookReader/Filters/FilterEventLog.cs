using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogBookReader.Filters
{
    public class FilterEventLog : IModels.IEventLog
    {
        public FilterEventLog() { }
        public FilterEventLog(Models.EventLog eventLog)
        {
            Fill(eventLog);
        }

        #region Properties

        public int RowID { get; set; }
        public int Severity { get; set; }
        public int Date { get; set; }
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

        #endregion

        public string ComputerName { get; set; }
        public string AppName { get; set; }

        public void Fill(Models.EventLog eventLog)
        {
            RowID = eventLog.RowID;
            Severity = eventLog.Severity;
            Date = eventLog.Date;
            ConnectID = eventLog.ConnectID;
            Session = eventLog.Session;
            TransactionStatus = eventLog.TransactionStatus;
            TransactionDate = eventLog.TransactionDate;
            TransactionID = eventLog.TransactionID;
            UserCode = eventLog.UserCode;
            ComputerCode = eventLog.ComputerCode;
            AppCode = eventLog.AppCode;
            EventCode = eventLog.EventCode;
            Comment = eventLog.Comment;
            MetadataCodes = eventLog.MetadataCodes;
            SessionDataSplitCode = eventLog.SessionDataSplitCode;
            DataType = eventLog.DataType;
            Data = eventLog.Data;
            DataPresentation = eventLog.DataPresentation;
            WorkServerCode = eventLog.WorkServerCode;
            PrimaryPortCode = eventLog.PrimaryPortCode;
            SecondaryPortCode = eventLog.SecondaryPortCode;
        }
    }
}
