using System.ComponentModel.DataAnnotations;

namespace Car_Rental_Management.Models
{
    public class Location
    {
        [Key]
        public int LocationId { get; set; }

        [Required(ErrorMessage = "Location name is required")]
        [MaxLength(100)]
        public string LocationName { get; set; }

        [MaxLength(100)]
        public string Address { get; set; }
    }
}
