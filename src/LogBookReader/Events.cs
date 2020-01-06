using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogBookReader.Events
{
    public delegate void ChangeIsLoadingEventLog(bool newValue);
    public class ChangeIsLoadingEventLogEvents : EventArgs
    {
        public static event ChangeIsLoadingEventLog ChangeIsLoadingEventLog;
        public static void ChangeValue(bool newValue)
        {
            ChangeIsLoadingEventLog?.Invoke(newValue);
        }
    }
}
