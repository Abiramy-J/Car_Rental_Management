using System.ComponentModel.DataAnnotations;

namespace Car_Rental_Management.Models
{
    public class Customer
    {
        [Key]
        [Required]
        public int CustomerId { get; set; }  // ✅ Primary Key
        [Required (ErrorMessage ="Name Is required")]

        public string Name { get; set; }
        [Required(ErrorMessage = "E-mail Is required")]

        public string Email { get; set; }
        [Required(ErrorMessage = "Password Is required")]

        public string Password { get; set; }
        [Required(ErrorMessage = "Phone Number Is required")]

        public string PhoneNumber { get; set; }

        // Add navigation properties if needed
        public ICollection<Booking> Bookings { get; set; }
    }

}
