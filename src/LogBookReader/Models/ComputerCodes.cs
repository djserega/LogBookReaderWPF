using System.ComponentModel.DataAnnotations;

namespace LogBookReader.Models
{
    public class ComputerCodes : IModels.IComputerCodes, IModels.IModelsBase
    {
        [Key]
        public int Code { get; set; }
        public string Name { get; set; }
    }
}
