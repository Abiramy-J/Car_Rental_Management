using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Car_Rental_Management.Models
{
    [Table("Customer_Bookings")]
    public class Customer_Booking
    {
        [Key]
        public int CustomerBookingId { get; set; } 

        [Required]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }  

        [Required]
        [ForeignKey("Booking")]
        public int BookingId { get; set; }  

        [Required]
        public DateTime BookingDate { get; set; } = DateTime.Now; 

        public virtual Customer Customer { get; set; }
        public virtual Booking Booking { get; set; }
    }
}
