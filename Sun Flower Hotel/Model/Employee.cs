using System.ComponentModel.DataAnnotations;

namespace Sun_Flower_Hotel.Model
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; } // Primary Key

        [Required]
        [MaxLength(100)]
        public string? Name { get; set; } // Employee Name

        [MaxLength(50)]
        public string? Role { get; set; } // e.g., Receptionist, Manager

        [MaxLength(15)]
        public string? ContactNumber { get; set; } // Optional Contact Number

        [MaxLength(100)]
        public string? Email { get; set; } // Optional Email
        public ICollection<Feedback> ?Feedbacks { get; set; } // Employee-related feedbacks
                                                             // Navigation properties
        public ICollection<Booking>? Bookings { get; set; } // Employee's bookings

        // Navigation Property (optional)
    }
}