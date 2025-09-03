using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Car_Rental_Management.Models
{
    [Table("Car_Bookings")]
    public class Car_Booking
    {
        [Key]
        public int CarBookingId { get; set; }  

        [Required]
        [ForeignKey("Car")]
        public int CarId { get; set; }   

        [Required]
        [ForeignKey("Booking")]
        public int BookingId { get; set; }  

        [Required]
        public DateTime AssignedDate { get; set; } = DateTime.Now; 

        public virtual Car Car { get; set; }
        public virtual Booking Booking { get; set; }
    }
}
