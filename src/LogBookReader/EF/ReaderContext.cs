using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogBookReader.EF
{
    public class ReaderContext : DbContext
    {
        public ReaderContext() : base("DefaultConnection") { }

        public DbSet<Models.AppCodes> AppCodes { get; set; }
        public DbSet<Models.EventLog> EventLog { get; set; }
        public DbSet<Models.ComputerCodes> ComputerCodes { get; set; }
    }
}
