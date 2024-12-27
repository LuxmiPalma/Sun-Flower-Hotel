using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sun_Flower_Hotel.Model
{
    public class Feedback
    {
        [Key]
        public int FeedbackId { get; set; } // Primary Key

        [ForeignKey(nameof(Guest))]
        public int GuestId { get; set; } // Foreign Key to Guest

        [ForeignKey(nameof(Booking))]
        public int? BookingId { get; set; } // Foreign Key to Booking (nullable)
                                            // Relationship with Employee
        public int? EmployeeId { get; set; } // Foreign Key (Nullable)
        public Employee? Employee { get; set; } // Navigation Property
        [Required]
        [MaxLength(1000)]
        public string? Comments { get; set; } // Feedback comments

        [Range(1, 5)]
        public int Rating { get; set; } // Rating (1 to 5 stars)

        public Guest? Guest { get; set; } // Navigation property
        public Booking ?Booking { get; set; } // Navigation property (optional)
    }
}
