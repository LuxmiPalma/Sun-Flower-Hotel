using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sun_Flower_Hotel.Model
{
    public class BookingService
    {
        [Key]
        public int BookingServiceId { get; set; } // Primary Key

        [ForeignKey(nameof(Booking))]
        public int BookingId { get; set; } // Foreign Key to Booking

        [ForeignKey(nameof(Service))]
        public int ServiceId { get; set; } // Foreign Key to Service

        public int Quantity { get; set; } // Quantity of the service (e.g., 2 breakfasts)
        public string? ServiceName { get; set; } // Name of the Booking Service

        public Booking? Booking { get; set; } // Navigation Property
        public Service ?Service { get; set; } // Navigation Property
    }
}
