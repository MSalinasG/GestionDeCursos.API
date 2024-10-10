using System.ComponentModel.DataAnnotations;

namespace GestionDeCursos.Api.Areas.Auth.Models
{
    public class LoginModel
    {
        [Required]
        [StringLength(20)]
        public string? Username { get; set; }

        [Required]
        [StringLength(20)]
        public string? Password { get; set; }
    }
}
