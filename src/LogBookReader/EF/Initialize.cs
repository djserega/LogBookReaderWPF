using System;
using System.IO;

namespace LogBookReader.EF
{
    internal class Initialize
    {
        private readonly string _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        internal bool FindCreateConnectionFile(string dataSource = "1Cv8.lgd", bool overwriteFile = false)
        {
            string pathConfig = Path.Combine(_baseDirectory, "dbConnection.config");
            FileInfo configFile = new FileInfo(pathConfig);
            if (!configFile.Exists || overwriteFile)
            {
                try
                {
                    using (StreamWriter stream = configFile.CreateText())
                    {
                        stream.WriteLine("<connectionStrings>");
                        stream.WriteLine("  <clear/> ");
                        stream.WriteLine("  <add name=\"DefaultConnection\"");
                        stream.WriteLine($"       connectionString=\"Data Source={dataSource}; Read Only=True; FailIfMissing=False\"");
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

        internal bool ChangeDataSourceConfigSQLite(string fileName)
        {
            return FindCreateConnectionFile(fileName, true);
        }
    }
}
