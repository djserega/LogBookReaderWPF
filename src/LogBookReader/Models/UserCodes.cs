using System.ComponentModel.DataAnnotations;

namespace LogBookReader.Models
{
    public class UserCodes : IModels.IUserCodes, IModels.IModelsBase
    {
        [Key]
        public int Code { get; set; }
        public string Name { get; set; }
    }
}
