using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestCoWorking.ViewModels
{
    public class AddBookingModel
    {
        [Required(ErrorMessage = "Введіть назву бронювання")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введіть дату початку")]
        public DateTime? Start { get; set; }

        [Required(ErrorMessage = "Введіть дату завершення")]
        public DateTime? End { get; set; }

        [Required(ErrorMessage = "Введіть кількість місць")]
        public int? EmployeerCount { get; set; }

        public string Description { get; set; }
    }
}
