using System.IO;
using System.Data.Entity;

namespace LogBookReader.EF
{
    public class ReaderContext : DbContext
    {
        public ReaderContext() : base("DefaultConnection")
        {
            if (!Database.Exists())
                throw new FileNotFoundException();
        }

        public DbSet<Models.AppCodes> AppCodes { get; set; }
        public DbSet<Models.ComputerCodes> ComputerCodes { get; set; }
        public DbSet<Models.EventCodes> EventCodes { get; set; }
        public DbSet<Models.EventLog> EventLog { get; set; }
        public DbSet<Models.UserCodes> UserCodes { get; set; }
    }
}
