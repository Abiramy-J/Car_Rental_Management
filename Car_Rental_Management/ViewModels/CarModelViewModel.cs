using System.ComponentModel.DataAnnotations;

namespace Car_Rental_Management.ViewModels
{
    public class CarModelViewModel
    {
        [Key]
      
        public int CarModelId { get; set; }

        [Required]
        [Display(Name = "Model Name")]
        public string ModelName { get; set; }

        [Required (ErrorMessage ="Please Enter the Menufacturer")]
        [Display(Name = "Manufacturer")]
        public string Manufacturer { get; set; }

        [Required]
        [Range(2000, 2100)]
        public int Year { get; set; }

        [Required]
        [Display(Name = "Transmission Type")]
        public string TransmissionType { get; set; }

        [Required]
        [Range(2, 20)]
        [Display(Name = "Seating Capacity")]
        public int SeatingCapacity { get; set; }

        [Required]
        [Display(Name = "Price Per Day (Rs.)")]
        public decimal PricePerDay { get; set; }
        
    }
}
