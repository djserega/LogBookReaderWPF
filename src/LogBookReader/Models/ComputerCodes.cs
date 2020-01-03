using System.ComponentModel.DataAnnotations;

namespace LogBookReader.Models
{
    public class ComputerCodes : IModels.IComputerCodes
    {
        [Key]
        public int Code { get; set; }
        public string Name { get; set; }
    }
}
