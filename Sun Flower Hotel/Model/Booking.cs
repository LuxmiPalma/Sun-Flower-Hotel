using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sun_Flower_Hotel.Model
{
    public class Booking
    {
        [Key]
        public int BookingId { get; set; }

        [ForeignKey(nameof(Room))]
        public int RoomNumber { get; set; }

        [ForeignKey(nameof(Guest))]
        public int GuestId { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        public Room ?Room { get; set; }
        public Guest? Guest { get; set; }

        // Relationship with Employee
        public int EmployeeId { get; set; } // Foreign Key
        public Employee? Employee { get; set; } // Navigation Property


        public List<BookingService> BookingServices { get; set; } = new List<BookingService>();
        // Navigation Property for related invoices
        public ICollection<PaymentInvoice> ? PaymentInvoices { get; set; }
    }
}
