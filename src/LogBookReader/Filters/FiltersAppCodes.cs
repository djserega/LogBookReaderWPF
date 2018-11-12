using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogBookReader.Filters
{
    
    public class FiltersAppCodes : NotifyPropertyChangedClass, IModels.IAppCodes
    {
        public FiltersAppCodes() { }

        public FiltersAppCodes(Models.AppCodes appCode)
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
