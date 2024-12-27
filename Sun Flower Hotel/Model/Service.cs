using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sun_Flower_Hotel.Model
{
    public class Service
    {
        [Key]
        public int ServiceId { get; set; } // Primary Key

        [Required]
        [MaxLength(100)]
        public string? Name { get; set; } // e.g., Spa, Breakfast

        [Required]
        public decimal Price { get; set; } // Cost of the service

        [MaxLength(500)]
        public string? Description { get; set; } // Optional service description

        // navigation property for the many-to-many relationship
        public List<BookingService> BookingServices { get; set; } = new List<BookingService>();
    }
}
    

