using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogBookReader.Models
{
    public class AppCodes
    {
        [Key]
        public int Code { get; set; }
        public string Name { get; set; }
    }
}
