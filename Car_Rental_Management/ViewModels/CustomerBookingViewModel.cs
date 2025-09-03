namespace Car_Rental_Management.ViewModels
{
    public class CustomerBookingViewModel
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }

        public List<BookingViewModel> Bookings { get; set; } = new List<BookingViewModel>();
    }
}
