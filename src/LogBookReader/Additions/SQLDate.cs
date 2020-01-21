using System;

namespace LogBookReader.Additions
{
    public static class SQLDate
    {
        public static long DateToSQLite(this DateTime date)
        {
            long dateSqlite = (long)(date - DateTime.MinValue).TotalMilliseconds * 10;

            return dateSqlite;
        }
        public static DateTime DateToSQLite(this long SQLDate)
        {
            DateTime date = DateTime.MinValue.AddSeconds(SQLDate / 10000);

            return date;
        }
    }
}
