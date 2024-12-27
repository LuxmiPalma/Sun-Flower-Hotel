using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sun_Flower_Hotel.Model
{
    public class Room
    {
        [Key]
        public int RoomNumber { get; set; }// Primary Key: Unique identifier for the room.

        [Required]
        [MaxLength(50)]
        public string ?Type { get; set; } // Room type (e.g., Suite, Deluxe).

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; } // Price per night for the room.

        public bool IsAvailable { get; set; } // Availability status of the room.
        [Range(0, int.MaxValue, ErrorMessage = "Extra beds must be non-negative.")]
        public int ExtraBeds { get; set; } // Number of extra beds available.

        [Range(1, int.MaxValue, ErrorMessage = "Size must be a positive value.")]
        public int Size { get; set; } // Size of the room in square meters.

        [Range(1, int.MaxValue, ErrorMessage = "Maximum guests must be at least 1.")]
        public int MaxGuests { get; set; } // Maximum number of guests allowed.
        public List<Booking> Bookings { get; set; } = new List<Booking>(); // Navigation property for Bookings.
    }
}
