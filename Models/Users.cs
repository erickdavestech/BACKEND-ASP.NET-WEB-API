using System.ComponentModel.DataAnnotations;

namespace BACKEND_ASP.NET_WEB_API.Models
{
    public class User
    {
        [Key]
        public int Id{ get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }

}
