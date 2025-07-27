using Car_Rental_Management.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Car_Rental_Management.ViewModels
{
    public class CarFilterVM
    { // Filtering
        public int? SelectedCarModelID { get; set; }
        public decimal? MinRate { get; set; }
        public decimal? MaxRate { get; set; }
        public string Status { get; set; }
        public string Keyword { get; set; }

        // Sorting
        public string SortOrder { get; set; }

        // Data for dropdowns and results
        public List<SelectListItem> CarModelList { get; set; }
        public List<SelectListItem> StatusList { get; set; }
        public List<Car> CarList { get; set; }
    }
}
