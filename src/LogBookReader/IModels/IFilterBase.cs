using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogBookReader.IModels
{
    interface IFilterBase
    {
        bool IsChecked { get; set; }
        int Code { get; set; }
        string Name { get; set; }
    }
}
