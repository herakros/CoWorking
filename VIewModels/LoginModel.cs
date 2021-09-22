using System.ComponentModel.DataAnnotations;

namespace TestCoWorking.VIewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Не указан Email")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Введіть правильний формат пошти")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
