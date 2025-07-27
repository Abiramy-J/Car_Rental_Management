using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Car_Rental_Management.Models;

namespace Car_Rental_Management.ViewModels
{
    public class CarVM
    {
        public Car Car { get; set; }

        [BindNever]
        public IEnumerable<SelectListItem> CarModelList { get; set; }

        [BindNever]
        public List<SelectListItem> StatusList { get; set; }
    }
}
