using Car_Rental_Management.Models;
using Microsoft.EntityFrameworkCore;

namespace Car_Rental_Management.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<CarModal>CarModals { get; set; }
        public DbSet<Booking>Bookings { get; set; }
        public DbSet<Customer>Customers { get; set; }
        public DbSet<Car>Cars { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
