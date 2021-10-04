using System.ComponentModel.DataAnnotations;

namespace Eis.Identity.Api.Models 
{
    public class AppUser 
    {
        [Key]
        [Required]
        public int Id { get; set; } 

        [Required]
        public string Name { get; set; }

        [Required]
        public string ObjectId { get; set; }
    }
}