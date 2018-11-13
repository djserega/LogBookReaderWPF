using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogBookReader.Filters
{
    public class FilterComputerCodes : IModels.IComputerCodes
    {
        public FilterComputerCodes() { }
        public FilterComputerCodes(Models.ComputerCodes computerCode)
        {
            Fill(computerCode);
        }

        public bool IsChecked { get; set; }
        public int Code { get; set; }
        public string Name { get; set; }

        public void Fill(Models.ComputerCodes computerCode)
        {
            Code = computerCode.Code;
            Name = computerCode.Name;
        }

    }
}
