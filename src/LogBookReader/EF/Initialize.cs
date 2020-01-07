using System;
using System.IO;

namespace LogBookReader.EF
{
    internal class Initialize
    {
        private readonly string _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        internal bool FindCreateConnectionFile()
        {
            string pathConfig = Path.Combine(_baseDirectory, "dbConnection.config");
            FileInfo configFile = new FileInfo(pathConfig);
            if (!configFile.Exists)
            {
                try
                {
                    using (StreamWriter stream = configFile.CreateText())
                    {
                        stream.WriteLine("<connectionStrings>");
                        stream.WriteLine("  <add name=\"DefaultConnection\"");
                        stream.WriteLine("       connectionString=\"Data Source=1Cv8.lgd; Read Only=True; FailIfMissing=False\"");
                        stream.WriteLine("       providerName=\"System.Data.SQLite\"/>");
                        stream.WriteLine("</connectionStrings>");
                        stream.Flush();
                    }
                    return true;

                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
                return true;
        }
    }
}
