using System.ComponentModel.DataAnnotations;

namespace TokaAPI.Models.ViewModel
{
    public class LoginVM
    {
        [Required(ErrorMessage = "El usuario es obligatorio.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La clave es obligatoria.")]
        public string Clave { get; set; }
    }
}