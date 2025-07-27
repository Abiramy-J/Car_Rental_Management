using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Car_Rental_Management.Models
{
    public class Car
    {
        [Key]
        public int CarID { get; set; }

        [Required]
        [Display(Name = "Car Model")]
        public int CarModelID { get; set; }

        [ForeignKey("CarModelID")]
        public CarModel? CarModel { get; set; }

        [Required]
        public string Color { get; set; }

        [Required]
        [Range(0, 999999)]
        public int Mileage { get; set; }

        [Required]
        [Range(100, 99999)]
        [Display(Name = "Daily Rent Rate")]
        public decimal DailyRate { get; set; }

        [Display(Name = "Main Image URL")]
        public string? ImageUrl { get; set; }

        public string? ImageUrl2 { get; set; }
        public string? ImageUrl3 { get; set; }
        public string? ImageUrl4 { get; set; }

        [Display(Name = "Video URL (optional)")]
        public string? VideoUrl { get; set; }

        public string? Description { get; set; }

        [Required]
        public string Status { get; set; } // Available, Maintenance
    }
}