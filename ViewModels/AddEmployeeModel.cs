using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestCoWorking.ViewModels
{
    public class AddEmployeeModel
    {
        public int BookingId { get; set; }

        [Required(ErrorMessage = "Введіть повне ім'я користувача")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Введіть пошту")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Введіть правильний формат пошти")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введіть короткий опис")]
        public string Description { get; set; }
    }
}
