using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TestCoWorking.ViewModels
{
    public class PlaceModel
    {
        [Required(ErrorMessage = "Введіть кількість місць")]
        public int? Count { get; set; }
    }
}
