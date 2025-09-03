namespace Car_Rental_Management.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalCars { get; set; }
        public int AvailableCars { get; set; }
        public int TotalBookings { get; set; }
        public int ActiveCustomers { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
