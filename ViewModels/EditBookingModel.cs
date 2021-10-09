using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestCoWorking.ViewModels
{
    public class EditBookingModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Введіть назву бронювання")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Не вказаний Email")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Введіть правильний формат пошти")]
        public string DevEmail { get; set; }

        [Required(ErrorMessage = "Введіть кількість місць")]
        public int? EmployeerCount { get; set; }

        [Required(ErrorMessage = "Встановіть дату початку")]
        public DateTime Start { get; set; }

        [Required(ErrorMessage = "Встановіть дату завершення")]
        public DateTime End { get; set; }

        public string Description { get; set; }
    }
}
