using System.ComponentModel.DataAnnotations;

namespace FaultTracking.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Kullanıcı adı boş bırakılamaz!")]
        [Display(Name = "Kullanıcı Adı")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Şifre boş bırakılamaz!")]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }
    }
}
