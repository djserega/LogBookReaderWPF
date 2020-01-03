using System.ComponentModel.DataAnnotations;

namespace LogBookReader.Models
{
    public class AppCodes : IModels.IAppCodes
    {
        [Key]
        public int Code { get; set; }
        public string Name { get; set; }
    }
}
