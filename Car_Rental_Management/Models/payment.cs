using System;
using System.ComponentModel.DataAnnotations;

namespace Car_Rental_Management.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }

        [Required]
        public int BookingId { get; set; }   

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [Required]
        public string PaymentMethod { get; set; }   

        public DateTime PaymentDate { get; set; } = DateTime.Now;
    }
}
