using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Car_Rental_Management.Models
{
    public class Car
    {
        [Key]
        public int CarId { get; set; }

        [Required]
        public int CarModelId { get; set; }  

        [ForeignKey("CarModelId")]
        public CarModal CarModel { get; set; }  

        [Required(ErrorMessage = "Car Name is required")]
        [MaxLength(100)]
        public string CarName { get; set; }

        [MaxLength(200)]
        public string ImageUrl { get; set; }

        public bool IsAvailable { get; set; }

        public ICollection<Booking> Bookings { get; set; }
    }
}
