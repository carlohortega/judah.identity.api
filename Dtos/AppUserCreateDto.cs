using System.ComponentModel.DataAnnotations;

namespace Eis.Identity.Api.Dtos
{
    public class AppUserCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string ObjectId { get; set; }
    }
}