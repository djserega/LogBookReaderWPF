using System.IO;
using System.Data.Entity;

namespace LogBookReader.EF
{
    public class ReaderContext : DbContext
    {
        public ReaderContext() : base("DefaultConnection")
        {
            if (!Database.Exists())
#if DEBUG
                throw new FileNotFoundException(Database.Connection.ConnectionString);
#else
                throw new FileNotFoundException();
#endif
        }

        public DbSet<Models.AppCodes> AppCodes { get; set; }
        public DbSet<Models.ComputerCodes> ComputerCodes { get; set; }
        public DbSet<Models.EventCodes> EventCodes { get; set; }
        public DbSet<Models.EventLog> EventLog { get; set; }
        public DbSet<Models.UserCodes> UserCodes { get; set; }
        public DbSet<Models.MetadataCodes> MetadataCodes { get; set; }

        internal string DataSourceConnectionString
        {
            get
            {
                string connectioString = Database.Connection.ConnectionString;
                string result = string.Empty;

                string startPositionText = "Data Source";
                int startPosition = connectioString.IndexOf(startPositionText);
                if (startPosition >= 0)
                {
                    startPosition += startPositionText.Length;

                    int endPosition = connectioString.IndexOf(";", startPosition);
                    if (endPosition > 0)
                    {
                        result = connectioString.Substring(startPosition + 1, endPosition - startPosition - 1);
                    }
                }

                return result;
            }
        }
    }
}
