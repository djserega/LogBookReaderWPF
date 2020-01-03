using System.Data.Entity;

namespace LogBookReader.EF
{
    public class ReaderContext : DbContext
    {
        public ReaderContext() : base("DefaultConnection") { }

        public DbSet<Models.AppCodes> AppCodes { get; set; }
        public DbSet<Models.ComputerCodes> ComputerCodes { get; set; }
        public DbSet<Models.EventCodes> EventCodes { get; set; }
        public DbSet<Models.EventLog> EventLog { get; set; }
    }
}
