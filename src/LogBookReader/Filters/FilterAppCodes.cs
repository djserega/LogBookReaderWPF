using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogBookReader.Filters
{
    
    public class FilterAppCodes : NotifyPropertyChangedClass, IModels.IAppCodes
    {
        public FilterAppCodes() { }

        public FilterAppCodes(Models.AppCodes appCode)
        {
            Fill(appCode);
        }

        public bool IsChecked { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }

        public void Fill(Models.AppCodes appCode)
        {
            Code = appCode.Code;
            Name = appCode.Name;
        }
    }
}
