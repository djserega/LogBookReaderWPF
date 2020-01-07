using System.ComponentModel.DataAnnotations;

namespace LogBookReader.Models
{
    public class UserCodes : IModels.IUserCodes
    {
        [Key]
        public int Code { get; set; }
        public string Name { get; set; }
    }
}
