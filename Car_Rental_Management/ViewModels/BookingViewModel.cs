namespace Car_Rental_Management.ViewModels
{
    public class BookingViewModel
    {
        public int BookingId { get; set; }
        public string CarName { get; set; }
        public string CarModel { get; set; }
        public string CustomerName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
        public bool IsPaid { get; set; }   
    }
}
