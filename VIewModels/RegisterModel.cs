using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestCoWorking.VIewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Не вказана пошта")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не вказаний нік")]
        public string NickName { get; set; }

        [Required(ErrorMessage = "Не вказаний пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Пароль введено не вірно")]
        public string ConfirmPassword { get; set; }

        public string Role { get; set; }
    }
}
