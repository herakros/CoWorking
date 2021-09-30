using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestCoWorking.ViewModels
{
    public class EditUserModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Не вказана пошта")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Введіть правильний формат пошти")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Не вказаний нік")]
        public string NickName { get; set; }

        public string Role { get; set; }
    }
}
