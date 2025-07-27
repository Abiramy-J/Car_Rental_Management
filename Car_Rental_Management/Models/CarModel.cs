using System.ComponentModel.DataAnnotations;

namespace Car_Rental_Management.Models
{
    public class CarModel
    {
        public int CarModelID { get; set; }

        [Required(ErrorMessage = "Model Name is required")]
        [MaxLength(100)]
        public string ModelName { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public string Type { get; set; }
    }
}
